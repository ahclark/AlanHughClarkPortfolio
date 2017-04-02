#pragma pack_matrix(row_major)

struct LIGHT
{
	float4 pos;
	float4 dir;
	float4 color;
	float4 radius;
};

float3 dirLightFunc(LIGHT light, float3 surfaceNorm)
{
	float3 ratio;
	ratio = saturate(dot(-light.dir.xyz, surfaceNorm));
	return ratio * light.color;
}

float3 pointLightFunc(LIGHT light, float3 surfaceNorm, float3 surfacePos)
{
	float3 dir;
	dir = light.pos.xyz - surfacePos;
	float ratio;
	ratio = dot(normalize(dir), surfaceNorm);
	float atten;
	atten = 1.0f - saturate(length(light.pos.xyz - surfacePos) / light.radius.z);
	return ratio * light.color.xyz * atten;
}

float3 spotLightFunc(LIGHT light, float3 surfaceNorm, float3 surfacePos)
{
	float3 dir;
	dir = normalize(light.pos.xyz - surfacePos);
	float surfaceRatio;
	surfaceRatio = saturate(dot(-dir, normalize(light.dir.xyz)));
	float spotFactor;
	spotFactor = (surfaceRatio > light.radius.y) ? 1 : 0;
	float lightRatio;
	lightRatio = saturate(dot(dir, normalize(surfaceNorm)));

	return spotFactor * lightRatio * light.color.xyz;
}