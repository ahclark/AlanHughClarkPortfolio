Shader "Path Tracing/PathTraced"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _Color;
            sampler2D _MainTex;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 position : SV_POSITION;
            };

            float4 _MainTex_ST;

            v2f vert (appdata_base vertData)
            {
                v2f output;
                output.position = UnityObjectToClipPos(vertData.vertex);
                output.uv = TRANSFORM_TEX(vertData.texcoord, _MainTex);
                return output;
            }

            fixed4 frag (v2f fragInput) : SV_Target
            {
                fixed4 finalColor = tex2D(_MainTex, fragInput.uv) * _Color;
                return finalColor;
            }
            ENDCG
        }
    }
}
