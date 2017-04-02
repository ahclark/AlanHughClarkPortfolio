#pragma pack_matrix(row_major)

struct INPUT_VERTEX
{
	float2 UV : TEXCOORD;
	float3 coordinate : POSITION;
	float3 normal : NORMAL;
	float4 color : COLOR;
};

struct OUTPUT_VERTEX
{
	float2 uvOUT : TEXCOORD;
	float3 normOUT : NORMAL;
	float4 colorOUT : COLOR;
	float4 worldPos : WORLDPOS;
	float4 projectedCoordinate : SV_POSITION;
};

cbuffer OBJECT : register(b0)
{
	float4x4 localMatrix;
}

cbuffer SCENE : register(b1)
{
	float4x4 viewMatrix;
	float4x4 projMatrix;
}

OUTPUT_VERTEX main( INPUT_VERTEX fromVertexBuffer )
{
	OUTPUT_VERTEX sendToRasterizer = (OUTPUT_VERTEX)0;

	float4 localPos;
	localPos.x = fromVertexBuffer.coordinate.x;
	localPos.y = fromVertexBuffer.coordinate.y;
	localPos.z = fromVertexBuffer.coordinate.z;
	localPos.w = 1;

	localPos = mul(localPos, localMatrix);
	sendToRasterizer.worldPos = localPos;

	localPos = mul(localPos, viewMatrix);
	localPos = mul(localPos, projMatrix);

	sendToRasterizer.projectedCoordinate = localPos;

	sendToRasterizer.uvOUT = fromVertexBuffer.UV;

	sendToRasterizer.normOUT = fromVertexBuffer.normal;


	return sendToRasterizer;
}