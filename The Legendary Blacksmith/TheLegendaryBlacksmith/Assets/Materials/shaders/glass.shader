// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Shader created with Shader Forge Beta 0.25 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.25;sub:START;pass:START;ps:flbk:Transparent/Diffuse,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:False,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:2,dpts:2,wrdp:False,ufog:True,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|diff-2-RGB,spec-9-RGB,gloss-9-A,normal-3-RGB,amspl-49-OUT,alpha-2-A,refract-27-OUT;n:type:ShaderForge.SFN_Color,id:2,x:33085,y:32579,ptlb:color,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Tex2d,id:3,x:33468,y:32678,ptlb:bump,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Color,id:9,x:33341,y:32402,ptlb:specular/gloss,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Slider,id:20,x:33352,y:32897,ptlb:refraction,min:0,cur:0.3834586,max:1;n:type:ShaderForge.SFN_ComponentMask,id:26,x:33305,y:32724,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-3-RGB;n:type:ShaderForge.SFN_Multiply,id:27,x:33127,y:32799|A-26-OUT,B-20-OUT;n:type:ShaderForge.SFN_Cubemap,id:33,x:33435,y:33016,ptlb:reflection;n:type:ShaderForge.SFN_Color,id:34,x:33435,y:33184,ptlb:reflection_color,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:35,x:33183,y:33042|A-33-RGB,B-34-RGB;n:type:ShaderForge.SFN_Fresnel,id:41,x:33682,y:32917|EXP-47-OUT;n:type:ShaderForge.SFN_Slider,id:47,x:33975,y:32981,ptlb:fresnel,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Multiply,id:49,x:32986,y:32966|A-35-OUT,B-41-OUT;proporder:2-3-9-20-33-34-47;pass:END;sub:END;*/

Shader "Shader Forge/glass" {
    Properties {
        _color ("color", Color) = (0.5,0.5,0.5,1)
        _bump ("bump", 2D) = "bump" {}
        _speculargloss ("specular/gloss", Color) = (0.5,0.5,0.5,1)
        _refraction ("refraction", Range(0, 1)) = 0.3834586
        _reflection ("reflection", Cube) = "_Skybox" {}
        _reflectioncolor ("reflection_color", Color) = (0.5,0.5,0.5,1)
        _fresnel ("fresnel", Range(0, 1)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _color;
            uniform sampler2D _bump; uniform float4 _bump_ST;
            uniform float4 _speculargloss;
            uniform float _refraction;
            uniform samplerCUBE _reflection;
            uniform float4 _reflectioncolor;
            uniform float _fresnel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_100 = i.uv0;
                float3 node_3 = UnpackNormal(tex2D(_bump,TRANSFORM_TEX(node_100.rg, _bump)));
                float3 normalLocal = node_3.rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_3.rgb.rg*_refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
///////// Gloss:
                float4 node_9 = _speculargloss;
                float gloss = exp2(node_9.a*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = node_9.rgb;
                float3 specularAmb = ((texCUBE(_reflection,viewReflectDirection).rgb*_reflectioncolor.rgb)*pow(1.0-max(0,dot(normalDirection, viewDirection)),_fresnel)) * specularColor;
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor + specularAmb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_2 = _color;
                finalColor += diffuseLight * node_2.rgb;
                finalColor += specular;
/// Final Color:
                return fixed4(lerp(sceneColor.rgb, finalColor,node_2.a),1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _GrabTexture;
            uniform float4 _color;
            uniform sampler2D _bump; uniform float4 _bump_ST;
            uniform float4 _speculargloss;
            uniform float _refraction;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                float4 screenPos : TEXCOORD5;
                LIGHTING_COORDS(6,7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.screenPos = o.pos;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_101 = i.uv0;
                float3 node_3 = UnpackNormal(tex2D(_bump,TRANSFORM_TEX(node_101.rg, _bump)));
                float3 normalLocal = node_3.rgb;
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (node_3.rgb.rg*_refraction);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
///////// Gloss:
                float4 node_9 = _speculargloss;
                float gloss = exp2(node_9.a*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float3 specularColor = node_9.rgb;
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_2 = _color;
                finalColor += diffuseLight * node_2.rgb;
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * node_2.a,0);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
