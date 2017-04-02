textureCUBE Texture : register (t0);

SamplerState filters[1] : register(s0);

float4 main(float3 initialCoordinate : POSITION) : SV_TARGET
{
	return Texture.Sample(filters[0], initialCoordinate);
}