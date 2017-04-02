Texture2DMS<float4, 4> Texture : register (t0);

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
	float4 result;
	result = float4(0.0f, 0.0f, 0.0f, 0.0f);
	for (int i = 0; i < 4; i++)
	{
		result += Texture.Load(input.projectedCoordinate, i);
	}
	result = result / 4;

	float temp;
	temp = dot(result.rgb, float3(0.3, 0.59, 0.11));
	result.r = temp;
	result.g = temp;
	result.b = temp;
	result.a = 1.0f;

	return result;
}