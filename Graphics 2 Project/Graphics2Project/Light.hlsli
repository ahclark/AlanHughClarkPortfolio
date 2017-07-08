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
	ratio = saturate(dot(-normalize(light.dir.xyz), surfaceNorm));
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
	float spotFactor = 0.0f;
	//spotFactor = (surfaceRatio > light.radius.y) ? 0.75f : 0.0f;
	if (surfaceRatio > light.radius.y)
		spotFactor = 0.75f * (1.0f - (1.0f - surfaceRatio) * 1.0f / (1.0f - light.radius.y));
	float lightRatio;
	lightRatio = saturate(dot(dir, normalize(surfaceNorm)));

	return spotFactor * lightRatio * light.color.xyz;
}