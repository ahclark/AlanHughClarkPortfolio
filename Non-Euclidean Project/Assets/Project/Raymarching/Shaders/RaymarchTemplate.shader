// https://www.youtube.com/watch?v=QDfqgG8HJDQ
// https://kev.town/raymarching-toolkit/reference/raymarcher/
// https://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/scriptable-objects
// https://iquilezles.org/www/articles/distfunctions/distfunctions.htm

/* [DistanceFunctions] */

/* [DistanceOperations] */

/* [DomainOperations] */

/* [DistanceTransformations] */

/* [MapInverseMatrixMultiplication] */

/* [MapDistanceFunctions] */

/* [MapDistanceOperations] */

/* [MaterialFunctions] */

/* [MaterialSwitchCase] */

Shader "Hidden/RaymarchTemplate"
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
			uniform const int _MaxStep;
			uniform float _DrawDistance;
			uniform float _DistanceMargin;
			uniform float _Bias;
			uniform float _Scale;
			uniform float _Power;

			/* [ObjectInverseMatrices] */

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

			/* [DistanceFunctions] */

			/* [DistanceOperations] */

			/* [DomainOperations] */

			/* [DistanceTransformations] */

			// This is the distance field function.  The distance field represents the closest distance to the surface
			// of any object we put in the scene.  If the given point (point p) is inside of an object, we return a
			// negative answer.
			float2 map(float3 p)
			{
				/* [MapInverseMatrixMultiplication] */

				/* [MapDistanceFunctions] */

				float2 ret = float2(0.0, 0.0);

				/* [MapDistanceOperations] */

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

				/*[unroll(p_maxStep)]*/ for (int i = 0; i < p_maxStep; ++i)
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

			/* [MaterialFunctions] */

			fixed4 CalculateColor(float p_matID, float3 p_position, float3 p_normal, float3 p_rayDirection, out float p_reflect, out float p_refract)
			{
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// force initialize output variables to avoid warnings
				p_reflect = 0.0;
				p_refract = 0.0;

				/* [MaterialSwitchCase] */

				return returnValue;
			}

			fixed4 PerformanceTestRaymarch(float3 ro, float3 rd, float s)
			{
				float t = 0; // current distance traveled along ray

				for (int i = 0; i < _MaxStep; ++i)
				{
					float3 p = ro + rd * t; // World space position of sample
					float2 d = map(p);      // Sample of distance field (see map())

					// If the sample <= 0, we have hit something (see map()).
					if (d.x < 0.001 || t > _DrawDistance)
					{
						// Simply return the number of steps taken, mapped to a color ramp.
						float perf = (float)i / _MaxStep;
						return fixed4(tex2D(_ColorRamp, float2(perf, 0)).xyz, 1);
					}

					t += d;
				}

				// By this point the loop guard (i < _MaxStep) is false.  Therefore
				// we have reached _MaxStep steps.
				return fixed4(tex2D(_ColorRamp, float2(1, 0)).xyz, 1);
			}

			// Raymarch along given ray
			// ro: ray origin
			// rd: ray direction
			fixed4 raymarch(float3 p_rayOrigin, float3 p_rayDirection, float p_depth)
			{
				float depth = p_depth;
				float3 rayOrigin = p_rayOrigin;
				float3 rayDirection = p_rayDirection;
				fixed4 mainColor = fixed4(0, 0, 0, 0);
				fixed4 reflectColor = fixed4(0, 0, 0, 0);
				fixed4 refractColor = fixed4(0, 0, 0, 0);
				fixed4 returnValue = fixed4(0, 0, 0, 0);

				// main color
				float2 intersectMain = CalculateIntersection(_MaxStep, _DrawDistance, _DistanceMargin, rayOrigin, rayDirection, depth);
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
					float3 reflectRayOrigin = mainPosition + mainNormal * _DistanceMargin;
					float3 reflectRayDirection = normalize(reflect(rayDirection, mainNormal));

					float2 intersectReflect = CalculateIntersection(_MaxStep, _DrawDistance, _DistanceMargin, reflectRayOrigin, reflectRayDirection, depth);

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
					float3 refractRayOrigin = mainPosition + ((-mainNormal) * (_DistanceMargin * 2.0));

					float refractiveIndex = 1.33; // refractive index of water = 4/3 = 1.33
					float3 refractRayDirection = refract(normalize(rayDirection), normalize(mainNormal), 1.0 / refractiveIndex);

					float2 intersectRefract = CalculateIntersection(_MaxStep, _DrawDistance, _DistanceMargin, refractRayOrigin, refractRayDirection, depth);

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
