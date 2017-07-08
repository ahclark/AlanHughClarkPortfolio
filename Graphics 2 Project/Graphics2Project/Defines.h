#pragma once

#include <DirectXMath.h>
using namespace DirectX;

#include <d3d11.h>
#pragma comment (lib, "d3d11.lib")

#include <iostream>
#include <fstream>
#include <ctime>
#include <vector>
using namespace std;

#include "XTime.h"
#include "DDSTextureLoader.h"

#include "Skybox_VS.csh"
#include "Skybox_PS.csh"
#include "Stage_VS.csh"
#include "Stage_PS.csh"
#include "Star_VS.csh"
#include "Star_PS.csh"
#include "Rock_VS.csh"
#include "Rock_PS.csh"
#include "Texture_PS.csh"
#include "Portal_PS.csh"
#include "Physics_CS.csh"

#define BACKBUFFER_WIDTH	1280
#define BACKBUFFER_HEIGHT	720
#define PI 3.14159265359f

class DEMO_APP;
DEMO_APP* currentApp;

const float orange[4] = { 1.0f, 0.5f, 0.0f, 1.0f };
const float green[4] = { 0.0f, 0.55f, 0.0f, 1.0f };
const float blue[4] = { 0.0f, 0.0f, 0.55f, 1.0f };
const XMFLOAT4 XMyellow = { 1.0f, 1.0f, 0.0f, 1.0f };
const XMFLOAT4 XMwhite = { 1.0f, 1.0f, 1.0f, 1.0f };

struct VERTEX
{
	XMFLOAT2 uv;
	XMFLOAT3 pos;
	XMFLOAT3 norm;
	XMFLOAT4 color;
};

struct SEND_TO_VRAM_OBJECT
{
	XMMATRIX localMatrix;
};
struct SEND_TO_VRAM_SCENE
{
	XMMATRIX viewMatrix;
	XMMATRIX projMatrix;
};

struct SEND_TO_VRAM_LIGHT
{
	XMFLOAT4 pos;
	XMFLOAT4 dir;
	XMFLOAT4 color;
	XMFLOAT4 radius;
};

struct particle
{
	float posX;
	float posY;
	float posZ;
	float radius;
	float velX;
	float velY;
	float velZ;
	float mass;
	float deltaTime;
	XMFLOAT3 buffer;
};

class Object
{
private:
	ID3D11Buffer*					m_VertexBuffer;
	ID3D11Buffer*					m_IndexBuffer;
	ID3D11VertexShader*				m_VertexShader;
	ID3D11PixelShader*				m_PixelShader;
	ID3D11ComputeShader*			m_ComputeShader;
	ID3D11InputLayout*				m_Layout;

public:
	Object();
	~Object();
	bool Initialize(ID3D11Device* p_device,
					VERTEX* p_data,
					UINT p_dataSize,
					unsigned int* p_indicies,
					UINT p_indiciesSize,
					const void* p_VS,
					UINT p_VSSize,
					const void* p_PS,
					UINT p_PSSize,
					const void* p_CS,
					UINT p_CSSize);
	bool Initialize(ID3D11Device* p_device,
					vector<VERTEX> p_data, UINT p_dataSize,
					unsigned int* p_indicies,
					UINT p_indiciesSize,
					const void* p_VS,
					UINT p_VSSize,
					const void* p_PS,
					UINT p_PSSize);
	bool Draw(	ID3D11DeviceContext* p_deviceContext,
				ID3D11Buffer* p_ObjectShaderBuffer,
				SEND_TO_VRAM_OBJECT p_ToObject,
				ID3D11Buffer* p_LightBuffer,
				vector<SEND_TO_VRAM_LIGHT> p_ToLight,
				ID3D11Buffer* p_structBuffer,
				particle p_ToPhysics,
				UINT p_stride,
				UINT p_offset,
				UINT p_indexCount,
				UINT p_startIndex);
	bool Draw(	ID3D11DeviceContext* p_deviceContext,
				ID3D11Buffer* p_ObjectShaderBuffer,
				vector<SEND_TO_VRAM_OBJECT> p_ToObject,
				ID3D11Buffer* p_LightBuffer,
				vector<SEND_TO_VRAM_LIGHT> p_ToLight,
				ID3D11Buffer* p_structBuffer,
				particle p_ToPhysics,
				UINT p_stride,
				UINT p_offset,
				UINT p_indexCount,
				UINT p_startIndex);
};

Object::Object()
{
	m_VertexBuffer = nullptr;
	m_IndexBuffer = nullptr;
	m_VertexShader = nullptr;
	m_PixelShader = nullptr;
	m_ComputeShader = nullptr;
	m_Layout = nullptr;
}

Object::~Object()
{
	if (m_VertexBuffer != nullptr)
		m_VertexBuffer->Release();
	if (m_IndexBuffer != nullptr)
		m_IndexBuffer->Release();
	if (m_VertexShader != nullptr)
		m_VertexShader->Release();
	if (m_PixelShader != nullptr)
		m_PixelShader->Release();
	if (m_ComputeShader != nullptr)
		m_ComputeShader->Release();
	if (m_Layout != nullptr)
		m_Layout->Release();
}

bool Object::Initialize(ID3D11Device* p_device,
						VERTEX* p_data,
						UINT p_dataSize,
						unsigned int* p_indicies,
						UINT p_indiciesSize,
						const void* p_VS,
						UINT p_VSSize,
						const void* p_PS,
						UINT p_PSSize,
						const void* p_CS,
						UINT p_CSSize)
{
	D3D11_BUFFER_DESC bufferDesc;
	ZeroMemory(&bufferDesc, sizeof(D3D11_BUFFER_DESC));
	bufferDesc.Usage = D3D11_USAGE_IMMUTABLE;
	bufferDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	bufferDesc.CPUAccessFlags = NULL;
	bufferDesc.ByteWidth = p_dataSize;

	D3D11_SUBRESOURCE_DATA subResource;
	subResource.pSysMem = p_data;
	subResource.SysMemPitch = 0;
	subResource.SysMemSlicePitch = 0;

	p_device->CreateBuffer(&bufferDesc, &subResource, &m_VertexBuffer);

	D3D11_BUFFER_DESC indexBufferDesc;
	ZeroMemory(&indexBufferDesc, sizeof(D3D11_BUFFER_DESC));
	indexBufferDesc.Usage = D3D11_USAGE_IMMUTABLE;
	indexBufferDesc.BindFlags = D3D11_BIND_INDEX_BUFFER;
	indexBufferDesc.CPUAccessFlags = NULL;
	indexBufferDesc.ByteWidth = p_indiciesSize;

	if (p_indicies != nullptr)
	{
		D3D11_SUBRESOURCE_DATA indexSubResource;
		indexSubResource.pSysMem = p_indicies;
		indexSubResource.SysMemPitch = 0;
		indexSubResource.SysMemSlicePitch = 0;

		p_device->CreateBuffer(&indexBufferDesc, &indexSubResource, &m_IndexBuffer);
	}

	p_device->CreateVertexShader(p_VS, p_VSSize, NULL, &m_VertexShader);
	p_device->CreatePixelShader(p_PS, p_PSSize, NULL, &m_PixelShader);

	if (p_CS != nullptr)
		p_device->CreateComputeShader(p_CS, p_CSSize, NULL, &m_ComputeShader);

	D3D11_INPUT_ELEMENT_DESC layoutDesc[] =
	{
		{ "TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "NORMAL", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "COLOR", 0, DXGI_FORMAT_R32G32B32A32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 }
	};

	p_device->CreateInputLayout(layoutDesc, 4, p_VS, p_VSSize, &m_Layout);

	return true;
}

bool Object::Initialize(ID3D11Device* p_device,
						vector<VERTEX> p_data,
						UINT p_dataSize,
						unsigned int* p_indicies,
						UINT p_indiciesSize,
						const void* p_VS,
						UINT p_VSSize,
						const void* p_PS,
						UINT p_PSSize)
{
	D3D11_BUFFER_DESC bufferDesc;
	ZeroMemory(&bufferDesc, sizeof(D3D11_BUFFER_DESC));
	bufferDesc.Usage = D3D11_USAGE_IMMUTABLE;
	bufferDesc.BindFlags = D3D11_BIND_VERTEX_BUFFER;
	bufferDesc.CPUAccessFlags = NULL;
	bufferDesc.ByteWidth = p_dataSize;

	D3D11_SUBRESOURCE_DATA subResource;
	subResource.pSysMem = p_data.data();
	subResource.SysMemPitch = 0;
	subResource.SysMemSlicePitch = 0;

	p_device->CreateBuffer(&bufferDesc, &subResource, &m_VertexBuffer);

	D3D11_BUFFER_DESC indexBufferDesc;
	ZeroMemory(&indexBufferDesc, sizeof(D3D11_BUFFER_DESC));
	indexBufferDesc.Usage = D3D11_USAGE_IMMUTABLE;
	indexBufferDesc.BindFlags = D3D11_BIND_INDEX_BUFFER;
	indexBufferDesc.CPUAccessFlags = NULL;
	indexBufferDesc.ByteWidth = p_indiciesSize;

	if (p_indicies != nullptr)
	{
		D3D11_SUBRESOURCE_DATA indexSubResource;
		indexSubResource.pSysMem = p_indicies;
		indexSubResource.SysMemPitch = 0;
		indexSubResource.SysMemSlicePitch = 0;

		p_device->CreateBuffer(&indexBufferDesc, &indexSubResource, &m_IndexBuffer);
	}

	p_device->CreateVertexShader(p_VS, p_VSSize, NULL, &m_VertexShader);
	p_device->CreatePixelShader(p_PS, p_PSSize, NULL, &m_PixelShader);

	D3D11_INPUT_ELEMENT_DESC layoutDesc[] =
	{
		{ "TEXCOORD", 0, DXGI_FORMAT_R32G32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "POSITION", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "NORMAL", 0, DXGI_FORMAT_R32G32B32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 },
		{ "COLOR", 0, DXGI_FORMAT_R32G32B32A32_FLOAT, 0, D3D11_APPEND_ALIGNED_ELEMENT, D3D11_INPUT_PER_VERTEX_DATA, 0 }
	};

	p_device->CreateInputLayout(layoutDesc, 4, p_VS, p_VSSize, &m_Layout);

	return true;
}

bool Object::Draw(	ID3D11DeviceContext* p_deviceContext,
					ID3D11Buffer* p_ObjectShaderBuffer,
					SEND_TO_VRAM_OBJECT p_ToObject,
					ID3D11Buffer* p_LightBuffer,
					vector<SEND_TO_VRAM_LIGHT> p_ToLight,
					ID3D11Buffer* p_structBuffer,
					particle p_ToPhysics,
					UINT p_stride,
					UINT p_offset,
					UINT p_indexCount,
					UINT p_startIndex)
{
	p_deviceContext->IASetVertexBuffers(0, 1, &m_VertexBuffer, &p_stride, &p_offset);

	p_deviceContext->IASetIndexBuffer(m_IndexBuffer, DXGI_FORMAT_R32_UINT, 0);

	D3D11_MAPPED_SUBRESOURCE mapSubresource;

	p_deviceContext->Map(p_ObjectShaderBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_OBJECT), &p_ToObject, sizeof(SEND_TO_VRAM_OBJECT));
	p_deviceContext->Unmap(p_ObjectShaderBuffer, NULL);
	p_deviceContext->VSSetConstantBuffers(0, 1, &p_ObjectShaderBuffer);

	p_deviceContext->VSSetShader(m_VertexShader, NULL, 0);

	p_deviceContext->Map(p_LightBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_LIGHT) * p_ToLight.size(), p_ToLight.data(), sizeof(SEND_TO_VRAM_LIGHT) * p_ToLight.size());
	p_deviceContext->Unmap(p_LightBuffer, NULL);
	p_deviceContext->PSSetConstantBuffers(1, 1, &p_LightBuffer);

	p_deviceContext->PSSetShader(m_PixelShader, NULL, 0);

	p_deviceContext->IASetInputLayout(m_Layout);

	p_deviceContext->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

	if (m_IndexBuffer != nullptr)
	{
		p_deviceContext->DrawIndexed(p_indexCount, p_startIndex, 0);
	}
	else
	{
		p_deviceContext->Draw(p_indexCount, p_startIndex);
	}

	return true;
}

bool Object::Draw(	ID3D11DeviceContext* p_deviceContext,
					ID3D11Buffer* p_ObjectShaderBuffer,
					vector<SEND_TO_VRAM_OBJECT> p_ToObject,
					ID3D11Buffer* p_LightBuffer,
					vector<SEND_TO_VRAM_LIGHT> p_ToLight,
					ID3D11Buffer* p_structBuffer,
					particle p_ToPhysics,
					UINT p_stride,
					UINT p_offset,
					UINT p_indexCount,
					UINT p_startIndex)
{
	p_deviceContext->IASetVertexBuffers(0, 1, &m_VertexBuffer, &p_stride, &p_offset);

	p_deviceContext->IASetIndexBuffer(m_IndexBuffer, DXGI_FORMAT_R32_UINT, 0);

	D3D11_MAPPED_SUBRESOURCE mapSubresource;

	p_deviceContext->Map(p_ObjectShaderBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_OBJECT) * p_ToObject.size(), p_ToObject.data(), sizeof(SEND_TO_VRAM_OBJECT) * p_ToObject.size());
	p_deviceContext->Unmap(p_ObjectShaderBuffer, NULL);
	p_deviceContext->VSSetConstantBuffers(0, 1, &p_ObjectShaderBuffer);

	p_deviceContext->VSSetShader(m_VertexShader, NULL, 0);

	p_deviceContext->Map(p_LightBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_LIGHT) * p_ToLight.size(), p_ToLight.data(), sizeof(SEND_TO_VRAM_LIGHT) * p_ToLight.size());
	p_deviceContext->Unmap(p_LightBuffer, NULL);
	p_deviceContext->PSSetConstantBuffers(1, 1, &p_LightBuffer);

	p_deviceContext->PSSetShader(m_PixelShader, NULL, 0);

	p_deviceContext->IASetInputLayout(m_Layout);

	p_deviceContext->IASetPrimitiveTopology(D3D_PRIMITIVE_TOPOLOGY_TRIANGLELIST);

	if (m_IndexBuffer != nullptr)
	{
		p_deviceContext->DrawIndexedInstanced(p_indexCount, 5, p_startIndex, 0, 0);
	}

	return true;
}

// Stand-Alone functions
bool LoadOBJ(char* p_filename, vector<VERTEX>* p_data, unsigned int* p_indicies)
{
	int vertexCount = 0;
	int textureCount = 0;
	int normalCount = 0;
	int faceCount = 0;
	char buffer;

	fstream file;
	file.open(p_filename);
	if (file.is_open())
	{
		vector<XMFLOAT4> verticies;
		vector<XMFLOAT3> UVs;
		vector<XMFLOAT3> normals;
		vector<VERTEX> tempData;

		int currBufferIndex;
		char valueBuffer[16];

		int currIndiciesIndex = 0;
		int numUnique = 0;

		file.get(buffer);
		while (buffer != 'v' && buffer != 'f')
			file.get(buffer);
		while (!file.eof())
		{
			file.get(buffer);
			if (buffer == 'v')
			{
				file.get(buffer);
				if (buffer == ' ')
				{
					XMFLOAT4 temp = { 0.0f, 0.0f, 0.0f, 0.0f };
					ZeroMemory(&valueBuffer, sizeof(valueBuffer));
					currBufferIndex = 0;
					while (buffer == ' ') // handle more than one space after element descriptor
						file.get(buffer);
					while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
					{
						valueBuffer[currBufferIndex] = buffer;
						file.get(buffer);
						currBufferIndex++;
					}
					temp.x = (float)atof(valueBuffer);
					ZeroMemory(&valueBuffer, sizeof(valueBuffer));
					currBufferIndex = 0;
					file.get(buffer);
					while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
					{
						valueBuffer[currBufferIndex] = buffer;
						file.get(buffer);
						currBufferIndex++;
					}
					temp.y = (float)atof(valueBuffer);
					ZeroMemory(&valueBuffer, sizeof(valueBuffer));
					currBufferIndex = 0;
					file.get(buffer);
					while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
					{
						valueBuffer[currBufferIndex] = buffer;
						file.get(buffer);
						currBufferIndex++;
					}
					temp.z = -(float)atof(valueBuffer);

					if (buffer == '\n')
					{
						temp.w = 1.0f;
					}
					else if (buffer == ' ')
					{
						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						file.get(buffer);
						while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						temp.w = (float)atof(valueBuffer);
					}
					verticies.push_back(temp);
				}
				else if (buffer == 't')
				{
					file.get(buffer);
					if (buffer == ' ')
					{
						XMFLOAT3 temp = { 0.0f, 0.0f, 0.0f };
						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						while (buffer == ' ') // handle more than one space after element descriptor
							file.get(buffer);
						while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						temp.x = (float)atof(valueBuffer);
						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						file.get(buffer);
						while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						temp.y = -(float)atof(valueBuffer);

						if (buffer == '\n')
						{
							temp.z = 0.0f;
						}
						else if (buffer == ' ')
						{
							ZeroMemory(&valueBuffer, sizeof(valueBuffer));
							currBufferIndex = 0;
							file.get(buffer);
							while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
							{
								valueBuffer[currBufferIndex] = buffer;
								file.get(buffer);
								currBufferIndex++;
							}
							temp.z = -(float)atof(valueBuffer);
						}
						UVs.push_back(temp);
					}
				}
				else if (buffer == 'n')
				{
					file.get(buffer);
					if (buffer == ' ')
					{
						XMFLOAT3 temp = { 0.0f, 0.0f, 0.0f };
						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						while (buffer == ' ') // handle more than one space after element descriptor
							file.get(buffer);
						while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						temp.x = (float)atof(valueBuffer);
						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						file.get(buffer);
						while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						temp.y = (float)atof(valueBuffer);
						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						file.get(buffer);
						while (buffer >= 48 && buffer <= 57 || buffer == 45 || buffer == 46)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						temp.z = -(float)atof(valueBuffer);

						normals.push_back(temp);
					}
				}
			}
			else if (buffer == 'f')
			{
				file.get(buffer);
				if (buffer == ' ')
				{
					while (buffer == ' ') // handle more than one space after element descriptor
						file.get(buffer);
					for (int i = 0; buffer !='\n'; i++)
					//while (buffer != '\n')
					{
						VERTEX tempVert;
						ZeroMemory(&tempVert, sizeof(VERTEX));

						ZeroMemory(&valueBuffer, sizeof(valueBuffer));
						currBufferIndex = 0;
						if (buffer == ' ')
							file.get(buffer);
						if (buffer == '\n')
							break;
						while (buffer >= 48 && buffer <= 57)
						{
							valueBuffer[currBufferIndex] = buffer;
							file.get(buffer);
							currBufferIndex++;
						}
						tempVert.pos.x = verticies[((unsigned int)atof(valueBuffer) - 1)].x;
						tempVert.pos.y = verticies[((unsigned int)atof(valueBuffer) - 1)].y;
						tempVert.pos.z = verticies[((unsigned int)atof(valueBuffer) - 1)].z;

						file.get(buffer);
						if (buffer == '/')
						{
							tempVert.uv = XMFLOAT2(0.0f, 0.0f);
						}
						else
						{
							ZeroMemory(&valueBuffer, sizeof(valueBuffer));
							currBufferIndex = 0;
							while (buffer >= 48 && buffer <= 57)
							{
								valueBuffer[currBufferIndex] = buffer;
								file.get(buffer);
								currBufferIndex++;
							}
							tempVert.uv.x = UVs[((unsigned int)atof(valueBuffer) - 1)].x;
							tempVert.uv.y = UVs[((unsigned int)atof(valueBuffer) - 1)].y;
						}
						if (buffer == '/')
						{
							ZeroMemory(&valueBuffer, sizeof(valueBuffer));
							currBufferIndex = 0;
							file.get(buffer);
							while (buffer >= 48 && buffer <= 57)
							{
								valueBuffer[currBufferIndex] = buffer;
								file.get(buffer);
								currBufferIndex++;
							}
							tempVert.norm = normals[((unsigned int)atof(valueBuffer) - 1)];
						}
						tempVert.color = XMFLOAT4(0.0f, 0.0f, 0.0f, 1.0f);

						if (i == 3) // 4th run through
						{
							p_indicies[currIndiciesIndex] = p_indicies[currIndiciesIndex - 1];
							p_indicies[currIndiciesIndex + 1] = p_indicies[currIndiciesIndex - 3];
							currIndiciesIndex += 2;
						}

						bool unique = true;
						for (unsigned int j = 0; j < tempData.size(); j++)
						{
							if (tempVert.uv.x == tempData[j].uv.x && tempVert.uv.y == tempData[j].uv.y &&
								tempVert.pos.x == tempData[j].pos.x && tempVert.pos.y == tempData[j].pos.y && tempVert.pos.z == tempData[j].pos.z &&
								tempVert.norm.x == tempData[j].norm.x && tempVert.norm.y == tempData[j].norm.y && tempVert.norm.z == tempData[j].norm.z &&
								tempVert.color.x == tempData[j].color.x && tempVert.color.y == tempData[j].color.y && tempVert.color.z == tempData[j].color.z)
							{
								unique = false;
								p_indicies[currIndiciesIndex] = j;
								break;
							}
						}
						if (unique)
						{
							tempData.push_back(tempVert);
							p_indicies[currIndiciesIndex] = numUnique;
							numUnique++;
						}
						currIndiciesIndex++;
					}
					swap(p_indicies[currIndiciesIndex - 2], p_indicies[currIndiciesIndex - 1]);
				}
			}
		}
		file.close();

		tempData.shrink_to_fit();
		swap(*p_data, tempData);

		return true;
	}
	else
		return false;
}