// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/Raymarch"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			
			#include "UnityCG.cginc"

			// Provided by our script
			uniform float4x4 _FrustumCornersES;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform float4x4 _CameraInvViewMatrix;
			uniform float3 _CameraWS;
			uniform float3 _LightDir;
			uniform sampler2D _ColorRamp;
			uniform float4x4 _TorusInvMatrix;
			uniform float4x4 _BoxInvMatrix;
			uniform float4x4 _SphereInvMatrix;
			uniform float4x4 _PlaneInvMatrix;
			uniform float4x4 _Plane2InvMatrix;
			uniform float _Bias;
			uniform float _Scale;
			uniform float _Power;

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

			// Torus
			// t.x: diameter
			// t.y: thickness
			// Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
			float sdTorus(float3 p, float2 t)
			{
				float2 q = float2(length(p.xz) - t.x, p.y);
				return length(q) - t.y;
			}

			// Box
			// b: size of box in x/y/z
			// Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
			float sdBox(float3 p, float3 b)
			{
				float3 d = abs(p) - b;
				return min(max(d.x, max(d.y, d.z)), 0.0) +
					length(max(d, 0.0));
			}

			float sdSphere(float3 p, float s)
			{
				return length(p) - s;
			}

			float sdPlane(float3 p, float4 n)
			{
				// n must be normalized
				return dot(p, n.xyz) + n.w;
			}

			// Union
			// Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
			//float opU(float d1, float d2)
			//{
			//	return min(d1, d2);
			//}

			// Union (with material data)
			float2 opU(float2 d1, float2 d2)
			{
				return (d1.x < d2.x) ? d1 : d2;
			}

			// Subtraction
			// Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
			float opS(float d1, float d2)
			{
				return max(-d1, d2);
			}

			// Intersection
			// Adapted from: http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
			float opI(float d1, float d2)
			{
				return max(d1, d2);
			}

			float opDisplace(float3 p, float primitive)
			{
				float displacement = /*sin(1 * p.x)**/sin(4 * p.y)/**sin(1 * p.z)*/;
				return primitive + displacement;
			}

			// This is the distance field function.  The distance field represents the closest distance to the surface
			// of any object we put in the scene.  If the given point (point p) is inside of an object, we return a
			// negative answer.
			float2 map(float3 p)
			{
				float4 p_torus = mul(_TorusInvMatrix, float4(p, 1));
				float4 p_box = mul(_BoxInvMatrix, float4(p, 1));
				float4 p_sphere = mul(_SphereInvMatrix, float4(p, 1));
				float4 p_plane = mul(_PlaneInvMatrix, float4(p, 1));
				float4 p_plane2 = mul(_Plane2InvMatrix, float4(p, 1));

				float2 d_torus = float2(sdTorus(p_torus, float2(1, 0.2)), 1);
				float2 d_box = float2(sdBox(p_box, float3(0.75, 0.5, 0.5)), 2);
				float2 d_sphere = float2(sdSphere(p_sphere, 1), 3);
				float2 d_plane = float2(sdPlane(p_plane, normalize(float4(0, 1, 0, 0))), 4);
				float2 d_plane2 = float2(sdPlane(p_plane2, normalize(float4(0, 1, 0, 0))), 5);
				//float2 d_plane2 = float2(opDisplace(p_plane2, sdPlane(p_plane2, normalize(float4(0, 1, 0, 0)))), 5);

				float2 ret = opU(d_torus, d_box);
				ret = opU(ret, d_sphere);
				ret = opU(ret, d_plane2);

				if (-0.001 >= d_plane.x)
				{
					d_plane.x *= -1.0;
				}
				ret = opU(ret, d_plane);

				return ret;
			}

			float3 CalculateNormal(in float3 pos)
			{
				// epsilon - used to approximate dx when taking the derivative
				const float2 eps = float2(0.001, 0.0);

				// The idea here is to find the "gradient" of the distance field at pos
				// Remember, the distance field is not boolean - even if you are inside an object
				// the number is negative, so this calculation still works.
				// Essentially you are approximating the derivative of the distance field at this point.
				float3 nor = float3(
					map(pos + eps.xyy).x - map(pos - eps.xyy).x,
					map(pos + eps.yxy).x - map(pos - eps.yxy).x,
					map(pos + eps.yyx).x - map(pos - eps.yyx).x);
				return normalize(nor);
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

				[unroll(p_maxStep)] for (int i = 0; i < p_maxStep; ++i)
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
				}

				return returnValue;
			}

			// MatID: 0
			fixed4 DefaultColor()
			{
				return fixed4(0, 0, 0, 0);
			}

			// MatID: 1
			fixed4 TorusColor(float3 p_position, float3 p_normal)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// Lambertian Lighting
				float light = dot(-_LightDir.xyz, p_normal);

				fixed3 color = tex2D(_ColorRamp, float2(0.5, 0)).xyz;
				color = color * clamp(light, 0.1, 1.0);

				returnValue = fixed4(color, 1);

				return returnValue;
			}

			// MatID: 2
			fixed4 BoxColor(float3 p_position, float3 p_normal)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// Lambertian Lighting
				float light = dot(-_LightDir.xyz, p_normal);

				fixed3 color = tex2D(_ColorRamp, float2(0.25, 0)).xyz;
				color = color * clamp(light, 0.1, 1.0);

				returnValue = fixed4(color, 1);

				return returnValue;
			}

			// MatID: 3
			fixed4 SphereColor(float3 p_position, float3 p_normal)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// Lambertian Lighting
				float light = dot(-_LightDir.xyz, p_normal);

				fixed3 color = tex2D(_ColorRamp, float2(0.75, 0)).xyz;
				color = color * clamp(light, 0.1, 1.0);

				returnValue = fixed4(color, 1);

				return returnValue;
			}

			// MatID: 4
			fixed4 PlaneColor(float3 p_position, float3 p_normal, float3 p_rayDirection, out float p_reflect, out float p_refract)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				//float bias = 0.5;
				//float scale = 0.5;
				//float power = 5;
				float fresnel = _Bias + _Scale * pow(1.0 - abs(dot(p_rayDirection, p_normal)), _Power);
				//fresnel = smoothstep(0.0, 1.0, fresnel);

				p_reflect = fresnel;
				//p_reflect = smoothstep(0.0, 1.0, dot(p_rayDirection, p_normal));
				p_refract = 0.5;

				// Lambertian Lighting
				float light = dot(-_LightDir.xyz, p_normal);

				//fixed3 color = tex2D(_ColorRamp, float2(0.675, 0)).xyz;
				fixed3 color = fixed3(0.0, 0.0, 0.2);
				color = color * clamp(light, 0.1, 1.0);

				returnValue = fixed4(color, 1);

				return returnValue;
			}

			// MatID: 5
			fixed4 Plane2Color(float3 p_position, float3 p_normal)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// Lambertian Lighting
				float light = dot(-_LightDir.xyz, p_normal);

				//fixed3 color = (tex2D(_ColorRamp, float2(0.1, 0)).xyz) * 0.5;
				fixed3 color = fixed3(0.8, 0.52, 0.25);
				color = color * clamp(light, 0.1, 1.0);

				returnValue = fixed4(color, 1);

				return returnValue;
			}

			fixed4 CalculateColor(float p_matID, float3 p_position, float3 p_normal, float3 p_rayDirection, out float p_reflect, out float p_refract)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// force initialize output variables to avoid warnings
				p_reflect = 0.0;
				p_refract = 0.0;

				switch (p_matID)
				{
				case 0:
					returnValue = DefaultColor();
					break;
				case 1:
					returnValue = TorusColor(p_position, p_normal);
					break;
				case 2:
					returnValue = BoxColor(p_position, p_normal);
					break;
				case 3:
					returnValue = SphereColor(p_position, p_normal);
					break;
				case 4:
					returnValue = PlaneColor(p_position, p_normal, p_rayDirection, p_reflect, p_refract);
					break;
				case 5:
					returnValue = Plane2Color(p_position, p_normal);
					break;
				default:
					returnValue = DefaultColor();
					break;
				}

				return returnValue;
			}

			fixed4 PerformanceTestRaymarch(float3 ro, float3 rd, float s)
			{
				const int maxstep = 64;
				const float drawdist = 40; // draw distance in unity units
				float t = 0; // current distance traveled along ray

				for (int i = 0; i < maxstep; ++i)
				{
					float3 p = ro + rd * t; // World space position of sample
					float2 d = map(p);      // Sample of distance field (see map())

					// If the sample <= 0, we have hit something (see map()).
					if (d.x < 0.001 || t > drawdist)
					{
						// Simply return the number of steps taken, mapped to a color ramp.
						float perf = (float)i / maxstep;
						return fixed4(tex2D(_ColorRamp, float2(perf, 0)).xyz, 1);
					}

					t += d;
				}

				// By this point the loop guard (i < maxstep) is false.  Therefore
				// we have reached maxstep steps.
				return fixed4(tex2D(_ColorRamp, float2(1, 0)).xyz, 1);
			}

			// Raymarch along given ray
			// ro: ray origin
			// rd: ray direction
			fixed4 raymarch(float3 p_rayOrigin, float3 p_rayDirection, float p_depth)
			{
				const int maxStep = 80;
				const float drawDistance = 80; // draw distance in unity units
				float distanceMargin = 0.001;
				float depth = p_depth;
				float3 rayOrigin = p_rayOrigin;
				float3 rayDirection = p_rayDirection;
				fixed4 mainColor = fixed4(0, 0, 0, 0);
				fixed4 reflectColor = fixed4(0, 0, 0, 0);
				fixed4 refractColor = fixed4(0, 0, 0, 0);
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// main color
				float2 intersectMain = CalculateIntersection(maxStep, drawDistance, distanceMargin, rayOrigin, rayDirection, depth);
				if (0.0 == intersectMain.y)
				{
					// draw nothing
					return fixed4(0, 0, 0, 0);
				}

				float mainReflectValue = 0.0;
				float mainRefractValue = 0.0;
				float3 mainPosition = rayOrigin + rayDirection * intersectMain.x;
				float3 mainNormal = CalculateNormal(mainPosition);
				mainColor = CalculateColor(intersectMain.y, mainPosition, mainNormal, rayDirection, mainReflectValue, mainRefractValue);

				// reflection color
				if (0.0 < mainReflectValue)
				{
					float3 reflectRayOrigin = mainPosition + mainNormal * distanceMargin;
					float3 reflectRayDirection = normalize(reflect(rayDirection, mainNormal));

					float2 intersectReflect = CalculateIntersection(maxStep, drawDistance, distanceMargin, reflectRayOrigin, reflectRayDirection, depth);

					if (0.0 != intersectReflect.y)
					{
						float reflectReflectValue = 0.0;
						float reflectRefractValue = 0.0;
						float3 reflectPosition = reflectRayOrigin + reflectRayDirection * intersectReflect.x;
						float3 reflectNormal = CalculateNormal(reflectPosition);
						reflectColor = CalculateColor(intersectReflect.y, reflectPosition, reflectNormal, reflectRayDirection, reflectReflectValue, reflectRefractValue);
					}
					else
					{
						reflectColor = fixed4(0.4, 0.4, 1.0, 1);
					}
				}

				// refraction color
				if (0.0 < mainRefractValue)
				{
					float3 refractRayOrigin = mainPosition + ((-mainNormal) * (distanceMargin * 2.0));

					float refractiveIndex = 1.33; // refractive index of water = 4/3 = 1.33
					float3 refractRayDirection = refract(normalize(rayDirection), normalize(mainNormal), 1.0 / refractiveIndex);

					float2 intersectRefract = CalculateIntersection(maxStep, drawDistance, distanceMargin, refractRayOrigin, refractRayDirection, depth);

					if (0.0 != intersectRefract.y)
					{
						float refractReflectValue = 0.0;
						float refractRefractValue = 0.0;
						float3 refractPosition = refractRayOrigin + refractRayDirection * intersectRefract.x;
						float3 refractNormal = CalculateNormal(refractPosition);
						refractColor = CalculateColor(intersectRefract.y, refractPosition, refractNormal, refractRayDirection, refractReflectValue, refractRefractValue);
					}
				}

				returnValue = lerp(mainColor, refractColor, mainRefractValue);
				returnValue = lerp(returnValue, reflectColor, mainReflectValue);
				returnValue.w = mainColor.w;

				return returnValue;
			}

			v2f vert(appdata v)
			{
				v2f o;

				// Index passed via custom blit function in RaymarchGeneric.cs
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
			
			//sampler2D _MainTex;

			//fixed4 frag(v2f i) : SV_Target
			//{
			//	fixed4 col = fixed4(i.ray, 1);
			//	return col;
			//}

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
