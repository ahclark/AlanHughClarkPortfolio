#include "Light.hlsli"

texture2D Texture : register (t0);

SamplerState filters[1] : register(s0);

cbuffer LIGHTBUFFER : register(b1)
{
	LIGHT light;
}

float4 main(float2 UVcoord : TEXCOORD, float3 normal : NORMAL, float4 color : COLOR) : SV_TARGET
{
	float4 result, textureColor;
	result = float4(0.0f, 0.0f, 0.0f, 0.0f);
	textureColor = Texture.Sample(filters[0], UVcoord);

	result.xyz = dirLightFunc(light, normal) * textureColor;
	result.w = 1.0f;

	//return result;
	return textureColor;
}