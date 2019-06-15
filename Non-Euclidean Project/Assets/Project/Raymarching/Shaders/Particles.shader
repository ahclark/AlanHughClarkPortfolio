Shader "Raymarch/Particles"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off
		ZWrite Off
		ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#pragma require 2darray
			
			#include "UnityCG.cginc"

			// Provided by our script
			uniform float4x4 _FrustumCornersES;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float4x4 _CameraInvViewMatrix;
			uniform float3 _CameraWS;
			uniform int _MaxStep;
			uniform float _DrawDistance;
			uniform float _DistanceMargin;
			uniform float _FixedStep;
			uniform float3 _LightDir;
			uniform float4x4 _OriginInvMatrix;
			uniform sampler2D _ColorRamp;
			uniform int _ParticleCount;
			uniform int _NumVoxels;
			uniform int _UseVoxels;

			Texture2DArray<float4> _VoxelTextures; 

			// Input to vertex shader
			struct appdata
			{
				// Remember, the z value here contains the index of _FrustumCornersES to use
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Output of vertex shader / input to fragment shader
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 ray : TEXCOORD1;
			};

			// Particle's data
			struct Particle
			{
				float3 position;
				float3 velocity;
			};

			struct Voxel
			{
				int numParticles;
				float3 positiveBound;
				float3 negativeBound;
			};

			// Particle's data, shared with the compute shader
			StructuredBuffer<Particle> particleBuffer;
			StructuredBuffer<Voxel> voxelBuffer;

			// Box
			// b: size of box in x/y/z
			// Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
			float sdBox(float3 p, float3 b)
			{
				float3 d = abs(p) - b;
				return min(max(d.x, max(d.y, d.z)), 0.0) +
					length(max(d, 0.0));
			}

			float sdParticle(float3 p, float3 d)
			{
				return distance(p, d) - 0.05;
			}

			// Union (with material data)
			float2 opU(float2 d1, float2 d2)
			{
				return (d1.x < d2.x) ? d1 : d2;
			}

			// This is the distance field function.  The distance field represents the closest distance to the surface
			// of any object we put in the scene.  If the given point (point p) is inside of an object, we return a
			// negative answer.
			float2 map(float3 p)
			{
				//float4 p_particle = float4(p, 1);
				float2 d_particle = float2(sdParticle(p, particleBuffer[0].position), 1);
				float2 ret = d_particle;

				if (-1 == _UseVoxels)
				{
					for (int i = 1; i < _ParticleCount; i++)
					{
						d_particle = float2(sdParticle(p, particleBuffer[i].position), 1);
						ret = opU(ret, d_particle);
					}
				}
				else if (1 == _UseVoxels)
				{
					//uint width, height, elements;
					//_VoxelTextures.GetDimensions(width, height, elements);
					for (int i = 0; i < _NumVoxels; i++)
					{
						//if (p.x < voxelBuffer[i].positiveBound.x && p.x > voxelBuffer[i].negativeBound.x &&
						//	p.y < voxelBuffer[i].positiveBound.y && p.y > voxelBuffer[i].negativeBound.y &&
						//	p.z < voxelBuffer[i].positiveBound.z && p.z > voxelBuffer[i].negativeBound.z)
						{
							for (int j = 0; j < voxelBuffer[i].numParticles; j++)
							{
								uint3 index;
								index.x = 0;
								index.y = j;
								index.z = i;

								float4 data = _VoxelTextures[index];
								float3 position = float3(data.x, data.y, data.z);
								d_particle = float2(sdParticle(p, position), 1);

								ret = opU(ret, d_particle);
							}
						}
					}
				}

				return ret;
			}

			float2 CalculateIntersection(const int p_maxStep, const float p_drawDistance, float p_distanceMargin, float3 p_rayOrigin, float3 p_rayDirection, float p_depth)
			{
				//float currentDistance = p_distanceMargin * 2.0; // current distance traveled along ray
				float currentDistance = 0.0;
				float depth = p_depth;
				float2 mapSample = float2(0, 0);
				float2 returnValue = float2(0, 0);
				float3 rayOrigin = p_rayOrigin;
				float3 rayDirection = p_rayDirection;

				for (int i = 0; i < p_maxStep; ++i)
				{
					// If we run past the depth buffer, stop and return nothing (transparent pixel)
					// this way raymarched objects and traditional meshes can coexist.
					if (currentDistance >= depth || currentDistance > p_drawDistance) // check draw distance in additon to depth
					{
						returnValue = float2(currentDistance, 0);
						break;
					}

					float3 position = rayOrigin + rayDirection * currentDistance;	// World space position of sample
					mapSample = map(position);										// Sample of distance field (see map())
																					// mapSample.x: distance field output
																					// mapSample.y: MatID

					// If the sample <= 0, we have hit something (see map()).
					if (p_distanceMargin > mapSample.x)
					{
						returnValue = float2(currentDistance, mapSample.y);
						break;
					}

					// If the sample > 0, we haven't hit anything yet so we should march forward
					// We step forward by distance d, because d is the minimum distance possible to intersect
					// an object (see map()).
					currentDistance += mapSample.x;

					//currentDistance += _FixedStep;
				}

				return returnValue;
			}

			// MatID: 0
			fixed4 DefaultColor()
			{
				return fixed4(0, 0, 0, 0);
			}

			// MatID: 1
			fixed4 ParticleColor(float3 p_position)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				fixed3 color = tex2D(_ColorRamp, float2(0.75, 0)).xyz;

				returnValue = fixed4(color, 1);

				return returnValue;
			}

			fixed4 CalculateColor(float p_matID, float3 p_position, float3 p_rayDirection)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				switch (p_matID)
				{
				case 0:
					returnValue = DefaultColor();
					break;
				case 1:
					returnValue = ParticleColor(p_position);
					break;
				default:
					returnValue = DefaultColor();
					break;
				}

				return returnValue;
			}

			// Raymarch along given ray
			// ro: ray origin
			// rd: ray direction
			fixed4 raymarch(float3 p_rayOrigin, float3 p_rayDirection, float p_depth)
			{
				const int maxStep = _MaxStep;
				const float drawDistance = _DrawDistance; // draw distance in unity units
				float distanceMargin = _DistanceMargin;
				float depth = p_depth;
				float3 rayOrigin = p_rayOrigin;
				float3 rayDirection = p_rayDirection;
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// main color
				float2 intersectMain = CalculateIntersection(maxStep, drawDistance, distanceMargin, rayOrigin, rayDirection, depth);
				if (0.0 == intersectMain.y)
				{
					// draw nothing
					return fixed4(0, 0, 0, 0);
				}

				float3 mainPosition = rayOrigin + rayDirection * intersectMain.x;
				returnValue = CalculateColor(intersectMain.y, mainPosition, rayDirection);

				return returnValue;
			}

			v2f vert(appdata v, uint instance_id : SV_InstanceID)
			{
				v2f o;

				// Index passed via custom blit function in RaymarchSmoke.cs
				half index = v.vertex.z;
				v.vertex.z = 0.1;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif

				// Get the eyespace view ray (normalized)
				o.ray = _FrustumCornersES[(int)index].xyz;

				// Dividing by z "normalizes" it in the z axis
				// Therefore multiplying the ray by some number i gives the viewspace position
				// of the point on the ray with [viewspace z]=i
				o.ray /= abs(o.ray.z);

				// Transform the ray from eyespace to worldspace
				// Note: _CameraInvViewMatrix was provided by the script
				o.ray = mul(_CameraInvViewMatrix, o.ray);
				return o;
			}

			uniform sampler2D _CameraDepthTexture;

			fixed4 frag(v2f i) : SV_Target
			{
				// ray direction
				float3 rd = normalize(i.ray.xyz);
				// ray origin (camera position)
				float3 ro = _CameraWS;

				float2 duv = i.uv;
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
				{
					duv.y = 1 - duv.y;
				}
				#endif

				// Convert from depth buffer (eye space) to true distance from camera
				// This is done by multiplying the eyespace depth by the length of the "z-normalized"
				// ray (see vert()).  Think of similar triangles: the view-space z-distance between a point
				// and the camera is proportional to the absolute distance.
				float depth = LinearEyeDepth(tex2D(_CameraDepthTexture, duv).r);
				depth *= length(i.ray.xyz);

				fixed3 col = tex2D(_MainTex,i.uv); // Color of the scene before this shader was run

				fixed4 add = raymarch(ro, rd, depth);
				//fixed4 add = PerformanceTestRaymarch(ro, rd, depth);

				// Returns final color using alpha blending
				return fixed4(col*(1.0 - add.w) + add.xyz * add.w,1.0);
			}
			ENDCG
		}
	}
}
