Shader "Hidden/Wiggle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}

		_X1 ("X 1", Float) = 10.0
		_X2 ("X 2", Float) = 5.0
		_X3 ("X 3", Float) = 0.05

		_Y1("Y 1", Float) = 10.0
		_Y2("Y 2", Float) = 5.0
		_Y3("Y 3", Float) = 0.05

		_Z1("Z 1", Float) = 10.0
		_Z2("Z 2", Float) = 5.0
		_Z3("Z 3", Float) = 0.05
	}
		SubShader
	{
		// No culling or depth
		//Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float _X1;
			float _X2;
			float _X3;

			float _Y1;
			float _Y2;
			float _Y3;

			float _Z1;
			float _Z2;
			float _Z3;

			v2f vert (appdata v)
			{
				v2f o;

				v.vertex.x += sin((v.vertex.y + _Time * _X1) * _X2) * _X3;
				v.vertex.y += sin((v.vertex.y + _Time * _Y1) * _Y2) * _Y3;
				v.vertex.z += cos((v.vertex.y + _Time * _Z1) * _Z2) * _Z3;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
