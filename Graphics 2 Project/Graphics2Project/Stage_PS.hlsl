Texture2D Texture : register (t0);

SamplerState filters[1] : register(s0);

struct PS_IN
{
	float2 uv : TEXCOORD;
	float3 norm : NORMAL;
	float4 color : COLOR;
	float4 worldPos : WORLDPOS;
	float4 projectedCoordinate : SV_POSITION;
};

float4 main(PS_IN input) : SV_TARGET
{
	return Texture.Sample(filters[0], input.projectedCoordinate);
}