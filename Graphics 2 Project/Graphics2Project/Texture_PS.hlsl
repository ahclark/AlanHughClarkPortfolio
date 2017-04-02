#include "Light.hlsli"

texture2D Texture : register (t0);

SamplerState filters[1] : register(s0);

struct PS_IN
{
	float2 uv : TEXCOORD;
	float3 norm : NORMAL;
	float4 color : COLOR;
	float4 worldPos : WORLDPOS;
	float4 projectedCoordinate : SV_POSITION;
};

cbuffer LIGHTBUFFER : register(b1)
{
	LIGHT light[3];
}

float4 main(PS_IN input) : SV_TARGET
{
	float4 result, textureColor;
	result = float4(0.0f, 0.0f, 0.0f, 0.0f);
	textureColor = Texture.Sample(filters[0], input.uv);
	float4 results[3];

	for (int i = 0; i < 3; i++)
	{
		result = float4(0.0f, 0.0f, 0.0f, 0.0f);

		if (light[i].pos.w == 0.0f)
		{
			result.xyz = dirLightFunc(light[i], input.norm) * textureColor;
		}
		if (light[i].pos.w == 1.0f)
		{
			result.xyz = pointLightFunc(light[i], input.norm, input.worldPos) * textureColor;
		}
		if (light[i].pos.w == 2.0f)
		{
			result.xyz = spotLightFunc(light[i], input.norm, input.worldPos) * textureColor;
		}
		result.w = 1.0f;
		results[i] = result;
	}

	result = float4(0.0f, 0.0f, 0.0f, 0.0f);
	for (int i = 0; i < 3; i++)
	{
		result += results[i];
	}

	return saturate(result);
}