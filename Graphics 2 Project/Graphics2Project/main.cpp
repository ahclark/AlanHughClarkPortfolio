//************************************************************
//************ INCLUDES & DEFINES ****************************
//************************************************************

#include "Defines.h"

//************************************************************
//************ SIMPLE WINDOWS APP CLASS **********************
//************************************************************

class DEMO_APP
{	
	HINSTANCE							application;
	WNDPROC								appWndProc;
	HWND								window;

	RECT								windowRect;
	int									width;
	int									height;
	int									centerX;
	int									centerY;

	ID3D11Device*						device;
	ID3D11DeviceContext*				deviceContext;
	IDXGISwapChain*						swapchain;
	ID3D11RenderTargetView*				renderTargetView;

	D3D11_VIEWPORT						viewport;
	XMMATRIX							viewMatrix;
	XMMATRIX							projectionMatrix;

	D3D11_VIEWPORT						miniViewport;
	XMMATRIX							miniViewMatrix;
	XMMATRIX							miniProjectionMatrix;

	Object								skybox;
	SEND_TO_VRAM_OBJECT					skyboxToObject;
	ID3D11Buffer*						skyboxObjectShaderBuffer;
	ID3D11ShaderResourceView*			skyboxShaderResourceView;

	Object								stage;
	SEND_TO_VRAM_OBJECT					stageToObject;
	ID3D11Buffer*						stageObjectShaderBuffer;
	ID3D11Texture2D*					stageTexture;
	ID3D11ShaderResourceView*			stageShaderResourceView;

	Object								wall;
	SEND_TO_VRAM_OBJECT					wallToObject;
	ID3D11Buffer*						wallObjectShaderBuffer;
	ID3D11ShaderResourceView*			wallShaderResourceView;

	Object								portal1;
	XMMATRIX							portal1ViewMatrix;
	SEND_TO_VRAM_OBJECT					portal1ToObject;
	ID3D11Buffer*						portal1ObjectShaderBuffer;
	ID3D11Texture2D*					portal1Texture;
	ID3D11RenderTargetView*				portal1RenderTargetView;
	ID3D11ShaderResourceView*			portal1ShaderResourceView;

	Object								portal2;
	XMMATRIX							portal2ViewMatrix;
	SEND_TO_VRAM_OBJECT					portal2ToObject;
	ID3D11Buffer*						portal2ObjectShaderBuffer;
	ID3D11Texture2D*					portal2Texture;
	ID3D11RenderTargetView*				portal2RenderTargetView;
	ID3D11ShaderResourceView*			portal2ShaderResourceView;

	ID3D11DepthStencilState*			shapeDepthStencilState;
	ID3D11DepthStencilState*			drawDepthStencilState;
	ID3D11DepthStencilState*			clipDepthStencilState;

	Object								star;
	SEND_TO_VRAM_OBJECT					starToObject;
	ID3D11Buffer*						starObjectShaderBuffer;

	Object								rock;
	vector<SEND_TO_VRAM_OBJECT>			rockToObject;
	ID3D11Buffer*						rockObjectShaderBuffer;
	ID3D11ShaderResourceView*			rockShaderResourceView;

	Object								flashlight;
	SEND_TO_VRAM_OBJECT					flashlightToObject;
	ID3D11Buffer*						flashlightObjectShaderBuffer;
	ID3D11ShaderResourceView*			flashlightShaderResourceView;

	Object								pointLight;
	SEND_TO_VRAM_OBJECT					pointLightToObject;
	ID3D11Buffer*						pointLightObjectShaderBuffer;
	vector<SEND_TO_VRAM_LIGHT>			ToLight;
	ID3D11Buffer*						ToLightBuffer;

	SEND_TO_VRAM_SCENE					cameraToScene;
	ID3D11Buffer*						cameraSceneShaderBuffer;

	Object								ball;
	SEND_TO_VRAM_OBJECT					ballToObject;
	ID3D11Buffer*						ballObjectShaderBuffer;
	ID3D11Buffer*						structBuffer;
	ID3D11ShaderResourceView*			ballShaderResourceView;
	ID3D11UnorderedAccessView*			ballUnorderedAccessView;
	particle							particles[6400];
	ID3D11ComputeShader*				ComputeShader;

	ID3D11Texture2D*					depthStencil;
	ID3D11DepthStencilView*				depthStencilView;

	ID3D11SamplerState*					samplerState;

	ID3D11RasterizerState*				MSAAEnabled;

	ID3D11RasterizerState*				FrontFaceCulling;

	XTime								timer;

	D3D11_MAPPED_SUBRESOURCE			mapSubresource;
	D3D11_SHADER_RESOURCE_VIEW_DESC		ballShaderResourceViewDesc;
	D3D11_UNORDERED_ACCESS_VIEW_DESC	ballUnorderedAccessViewDesc;

	bool								controlsEnabled;
	bool								showCursor;
	POINT								cursor;

	float								viewAngleX;
	float								viewAngleY;
	float								viewX;
	float								viewY;
	float								viewZ;

	int									MSAAcount = 4;

	int									indexOffset;
	float								timekeep;

public:
	DEMO_APP(HINSTANCE hinst, WNDPROC proc);
	bool Resize();
	bool Run();
	bool ShutDown();
};

//************************************************************
//************ CREATION OF OBJECTS & RESOURCES ***************
//************************************************************

DEMO_APP::DEMO_APP(HINSTANCE hinst, WNDPROC proc)
{
	// ****************** BEGIN WARNING ***********************// 
	// WINDOWS CODE, I DON'T TEACH THIS YOU MUST KNOW IT ALREADY! 
	application = hinst; 
	appWndProc = proc; 

	WNDCLASSEX  wndClass;
    ZeroMemory( &wndClass, sizeof( wndClass ) );
    wndClass.cbSize         = sizeof( WNDCLASSEX );
    wndClass.lpfnWndProc    = appWndProc;
    wndClass.lpszClassName  = L"DirectXApplication";
	wndClass.hInstance      = application;
    wndClass.hCursor        = LoadCursor( NULL, IDC_ARROW );
    wndClass.hbrBackground  = ( HBRUSH )( COLOR_WINDOWFRAME );
	//wndClass.hIcon			= LoadIcon(GetModuleHandle(NULL), MAKEINTRESOURCE(IDI_FSICON));
    RegisterClassEx( &wndClass );

	RECT window_size = { 0, 0, BACKBUFFER_WIDTH, BACKBUFFER_HEIGHT };
	AdjustWindowRect(&window_size, WS_OVERLAPPEDWINDOW, false);

	window = CreateWindow(	L"DirectXApplication", L"Graphics 2 Project",	WS_OVERLAPPEDWINDOW /*& ~(WS_THICKFRAME|WS_MAXIMIZEBOX) <- Disables resizing and maximizing the window*/,
							CW_USEDEFAULT, CW_USEDEFAULT, window_size.right-window_size.left, window_size.bottom-window_size.top,
							NULL, NULL,	application, this );

    ShowWindow( window, SW_SHOW );
	//********************* END WARNING ************************//

	srand((unsigned int)time(NULL));

	device = nullptr;
	deviceContext = nullptr;
	swapchain = nullptr;
	renderTargetView = nullptr;
	skyboxObjectShaderBuffer = nullptr;
	skyboxShaderResourceView = nullptr;
	stageObjectShaderBuffer = nullptr;
	stageTexture = nullptr;
	stageShaderResourceView = nullptr;
	wallObjectShaderBuffer = nullptr;
	wallShaderResourceView = nullptr;
	portal1ObjectShaderBuffer = nullptr;
	portal1Texture = nullptr;
	portal1RenderTargetView = nullptr;
	portal1ShaderResourceView = nullptr;
	portal2ObjectShaderBuffer = nullptr;
	portal2Texture = nullptr;
	portal2RenderTargetView = nullptr;
	portal2ShaderResourceView = nullptr;
	shapeDepthStencilState = nullptr;
	drawDepthStencilState = nullptr;
	clipDepthStencilState = nullptr;
	starObjectShaderBuffer = nullptr;
	rockObjectShaderBuffer = nullptr;
	rockShaderResourceView = nullptr;
	flashlightObjectShaderBuffer = nullptr;
	flashlightShaderResourceView = nullptr;
	ToLightBuffer = nullptr;
	cameraSceneShaderBuffer = nullptr;
	ballObjectShaderBuffer = nullptr;
	structBuffer = nullptr;
	ballShaderResourceView = nullptr;
	ballUnorderedAccessView = nullptr;
	ComputeShader = nullptr;
	depthStencil = nullptr;
	depthStencilView = nullptr;
	samplerState = nullptr;
	MSAAEnabled = nullptr;
	FrontFaceCulling = nullptr;

	DXGI_SWAP_CHAIN_DESC swapchainDesc;
	ZeroMemory(&swapchainDesc, sizeof(DXGI_SWAP_CHAIN_DESC));
	swapchainDesc.BufferCount = 1;
	swapchainDesc.BufferDesc.Format = DXGI_FORMAT_R8G8B8A8_UNORM;
	swapchainDesc.BufferUsage = DXGI_USAGE_RENDER_TARGET_OUTPUT;
	swapchainDesc.Flags = DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH;
	swapchainDesc.OutputWindow = window;
	swapchainDesc.SampleDesc.Count = MSAAcount;
	swapchainDesc.Windowed = TRUE;

	D3D11CreateDeviceAndSwapChain(	NULL,
									D3D_DRIVER_TYPE_HARDWARE,
									NULL,
									D3D11_CREATE_DEVICE_DEBUG,
									NULL,
									NULL,
									D3D11_SDK_VERSION,
									&swapchainDesc,
									&swapchain,
									&device,
									NULL,
									&deviceContext);

	ID3D11Resource* resource;
	swapchain->GetBuffer(0, __uuidof(resource), (void**)(&resource));
	device->CreateRenderTargetView(resource, NULL, &renderTargetView);
	resource->Release();

	ZeroMemory(&viewport, sizeof(D3D11_VIEWPORT));
	viewport.MinDepth = 0.0f;
	viewport.MaxDepth = 1.0f;
	viewport.Width = BACKBUFFER_WIDTH;
	viewport.Height = BACKBUFFER_HEIGHT;
	viewport.TopLeftX = 0.0f;
	viewport.TopLeftY = 0.0f;

	ZeroMemory(&miniViewport, sizeof(D3D11_VIEWPORT));
	miniViewport.MinDepth = 0.0f;
	miniViewport.MaxDepth = 1.0f;
	miniViewport.Width = BACKBUFFER_WIDTH / 4;
	miniViewport.Height = BACKBUFFER_HEIGHT / 4;
	miniViewport.TopLeftX = 0.0f;
	miniViewport.TopLeftY = 0.0f;

	//////////////////// v LIGHTS v ///////////////////
	// Directional
	SEND_TO_VRAM_LIGHT temp;
	temp.pos = XMFLOAT4(0.0f, 0.0f, 0.0f, 0.0f);
	temp.dir = XMFLOAT4(0.0f, -1.0f, 0.75f, 1.0f);
	temp.radius = XMFLOAT4(0.0f, 0.0f, 0.0f, 0.0f);
	temp.color = XMFLOAT4(0.6f, 0.6f, 0.6f, 1.0f);
	ToLight.push_back(temp);

	// Point
	temp.pos = XMFLOAT4(0.0f, 0.5f, 0.0f, 1.0f);
	temp.dir = XMFLOAT4(0.0f, 0.0f, 0.0f, 1.0f);
	temp.radius = XMFLOAT4(0.0f, 0.0f, 15.0f, 0.0f);
	temp.color = XMFLOAT4(1.0f, 0.001f, 0.001f, 1.0f);
	ToLight.push_back(temp);

	// Spot
	temp.pos = XMFLOAT4(0.0f, 0.0f, 0.0f, 2.0f);
	temp.dir = XMFLOAT4(0.0f, 0.0f, 0.0f, 1.0f);
	temp.radius = XMFLOAT4(0.99f, 0.925f, 10.0f, 0.0f);
	temp.color = XMFLOAT4(0.001f, 0.001f, 1.0f, 1.0f);
	ToLight.push_back(temp);

	D3D11_BUFFER_DESC ToLightBufferDesc;
	ZeroMemory(&ToLightBufferDesc, sizeof(D3D11_BUFFER_DESC));
	ToLightBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	ToLightBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT) * (UINT)ToLight.size();
	ToLightBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	ToLightBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&ToLightBufferDesc, NULL, &ToLightBuffer);
	//////////////////// ^ LIGHTS ^ ///////////////////

	//////////////////// v SKYBOX v ///////////////////
	VERTEX skyboxData[8] =
	{
		{ { 0.0f, 0.0f }, { -0.5f, 0.5f, 0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 0.0f }, { 0.5f, 0.5f, 0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 0.0f }, { 0.5f, 0.5f, -0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 0.0f }, { -0.5f, 0.5f, -0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },

		{ { 0.0f, 0.0f }, { -0.5f, -0.5f, 0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 0.0f }, { 0.5f, -0.5f, 0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 0.0f }, { 0.5f, -0.5f, -0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 0.0f }, { -0.5f, -0.5f, -0.5f }, { 0.0f, 0.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } }
	};
	unsigned int skyboxIndicies[36] =
	{
		// top
		0, 2, 1,
		0, 3, 2,

		// bottom
		4, 5, 6,
		4, 6, 7,

		// front
		3, 6, 2,
		3, 7, 6,

		// left
		0, 7, 3,
		0, 4, 7,

		// back
		1, 4, 0,
		1, 5, 4,

		// right
		2, 5, 1,
		2, 6, 5
	};

	skybox.Initialize(device, skyboxData, sizeof(skyboxData), skyboxIndicies, sizeof(skyboxIndicies), &Skybox_VS, sizeof(Skybox_VS), &Skybox_PS, sizeof(Skybox_PS), nullptr, 0);

	D3D11_BUFFER_DESC skyboxObjectShaderBufferDesc;
	ZeroMemory(&skyboxObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	skyboxObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	skyboxObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	skyboxObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	skyboxObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&skyboxObjectShaderBufferDesc, NULL, &skyboxObjectShaderBuffer);

	skyboxToObject.localMatrix = XMMatrixIdentity();

	CreateDDSTextureFromFile(device, L"Textures\\NorthernLights.dds", NULL, &skyboxShaderResourceView);
	//////////////////// ^ SKYBOX ^ ///////////////////

	//////////////////// v STAGE v ///////////////////
	VERTEX stageData[4] =
	{
		{ { 0.0f, 0.0f }, { -0.5f, 0.0f, 0.5f }, { 0.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 3.0f }, { 0.5f, 0.0f, 0.5f }, { 0.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 3.0f, 3.0f }, { 0.5f, 0.0f, -0.5f }, { 0.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 3.0f, 0.0f }, { -0.5f, 0.0f, -0.5f }, { 0.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } }
	};
	
	unsigned int stageIndicies[12] =
	{
		// top side
		0, 1, 2,
		0, 2, 3,
	
		// under side
		0, 2, 1,
		0, 3, 2
	};

	stage.Initialize(device, stageData, sizeof(stageData), stageIndicies, sizeof(stageIndicies), Stage_VS, sizeof(Stage_VS), Texture_PS, sizeof(Texture_PS), nullptr, 0);

	D3D11_BUFFER_DESC stageObjectShaderBufferDesc;
	ZeroMemory(&stageObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	stageObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	stageObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	stageObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	stageObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&stageObjectShaderBufferDesc, NULL, &stageObjectShaderBuffer);

	stageToObject.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(20.0f, 20.0f, 20.0f));
	stageToObject.localMatrix.r[3].m128_f32[1] = -1.5f;

	CreateDDSTextureFromFile(device, L"Textures\\checkerboard.dds", NULL, &stageShaderResourceView);
	//////////////////// ^ STAGE ^ ///////////////////

	//////////////////// v WALL v ///////////////////
	VERTEX wallData[8] =
	{
		{ { 0.0f, 0.0f }, { -2.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, -1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 3.0f, 0.0f }, { 2.0f, 1.0f, 0.0f }, { 0.0f, 0.0f, -1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 3.0f, 1.0f }, { 2.0f, -1.0f, 0.0f }, { 0.0f, 0.0f, -1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 1.0f }, { -2.0f, -1.0f, 0.0f }, { 0.0f, 0.0f, -1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },

		{ { 0.0f, 0.0f }, { -2.0f, 1.0f, 0.1f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 3.0f, 0.0f }, { 2.0f, 1.0f, 0.1f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 3.0f, 1.0f }, { 2.0f, -1.0f, 0.1f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } },
		{ { 0.0f, 1.0f }, { -2.0f, -1.0f, 0.1f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 0.0f, 1.0f } }
	};

	unsigned int wallIndicies[36] =
	{
		// front side
		0, 1, 2,
		0, 2, 3,

		// back side
		4, 6, 5,
		4, 7, 6,

		// top
		4, 5, 1,
		4, 1, 0,

		// right
		1, 5, 6,
		1, 6, 2,

		// bottom
		3, 2, 6,
		3, 6, 7,

		// left
		4, 0, 3,
		4, 3, 7
	};

	wall.Initialize(device, wallData, sizeof(wallData), wallIndicies, sizeof(wallIndicies), Stage_VS, sizeof(Stage_VS), Texture_PS, sizeof(Texture_PS), nullptr, 0);

	D3D11_BUFFER_DESC wallObjectShaderBufferDesc;
	ZeroMemory(&wallObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	wallObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	wallObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	wallObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	wallObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&wallObjectShaderBufferDesc, NULL, &wallObjectShaderBuffer);

	wallToObject.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(2.0f, 2.0f, 2.0f));
	wallToObject.localMatrix.r[3].m128_f32[0] = 3.5f;
	wallToObject.localMatrix.r[3].m128_f32[1] = 0.51f;
	wallToObject.localMatrix.r[3].m128_f32[2] = 7.0f;

	CreateDDSTextureFromFile(device, L"Textures\\concrete-wall.dds", NULL, &wallShaderResourceView);
	//////////////////// ^ WALL ^ ///////////////////

	//////////////////// v PORTALS v ///////////////////
	unsigned int portalNumVerts = 51;
	float portalAngle = 360.0f / (portalNumVerts - 1);
	VERTEX* portalData;
	portalData = new VERTEX[portalNumVerts];
	ZeroMemory(portalData, sizeof(VERTEX) * portalNumVerts);

	portalData[0].pos = XMFLOAT3(0.0f, 0.0f, 0.0f);
	portalData[0].color = XMFLOAT4(0.0f, 0.0f, 0.0f, 1.0f);

	for (unsigned int i = 0; i < portalNumVerts - 1; i++)
	{
		portalData[i + 1].pos = XMFLOAT3(0.6f * sin((float)(i * portalAngle) * (PI / 180.0f)), cos((float)(i * portalAngle) * (PI / 180.0f)), 0.0f);
		portalData[i + 1].color = XMFLOAT4(0.0f, 0.0f, 0.0f, 1.0f);
	}

	//unsigned int* portalIndicies;
	//portalIndicies = new unsigned int[(portalNumVerts - 1) * 3];
	unsigned int portalIndicies[150];
	unsigned int portalCurrIndex = 0;
	for (unsigned int i = 0; i < portalNumVerts - 2; i++)
	{
		portalIndicies[portalCurrIndex] = 0;
		portalCurrIndex++;
		portalIndicies[portalCurrIndex] = i + 1;
		portalCurrIndex++;
		portalIndicies[portalCurrIndex] = i + 2;
		portalCurrIndex++;
	}
	portalIndicies[portalCurrIndex] = 0;
	portalCurrIndex++;
	portalIndicies[portalCurrIndex] = portalNumVerts - 1;
	portalCurrIndex++;
	portalIndicies[portalCurrIndex] = 1;
	portalCurrIndex++;

	portal1.Initialize(device, portalData, sizeof(VERTEX) * portalNumVerts, portalIndicies, sizeof(portalIndicies), Stage_VS, sizeof(Stage_VS), Portal_PS, sizeof(Portal_PS), nullptr, 0);
	portal2.Initialize(device, portalData, sizeof(VERTEX) * portalNumVerts, portalIndicies, sizeof(portalIndicies), Stage_VS, sizeof(Stage_VS), Portal_PS, sizeof(Portal_PS), nullptr, 0);
	delete[] portalData;
	//delete[] portalIndicies;

	D3D11_BUFFER_DESC portalObjectShaderBufferDesc;
	ZeroMemory(&portalObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	portalObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	portalObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	portalObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	portalObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&portalObjectShaderBufferDesc, NULL, &portal1ObjectShaderBuffer);
	device->CreateBuffer(&portalObjectShaderBufferDesc, NULL, &portal2ObjectShaderBuffer);

	portal1ToObject.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(1.5f, 1.5f, 1.5f));
	portal1ToObject.localMatrix.r[3].m128_f32[0] = 1.0f;
	portal1ToObject.localMatrix.r[3].m128_f32[1] = 0.51f;
	portal1ToObject.localMatrix.r[3].m128_f32[2] = wallToObject.localMatrix.r[3].m128_f32[2];

	portal2ToObject.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(1.5f, 1.5f, 1.5f));
	portal2ToObject.localMatrix.r[3].m128_f32[0] = 6.0f;
	portal2ToObject.localMatrix.r[3].m128_f32[1] = 0.51f;
	portal2ToObject.localMatrix.r[3].m128_f32[2] = wallToObject.localMatrix.r[3].m128_f32[2];

	D3D11_DEPTH_STENCIL_DESC portalDepthStecilStateDesc;
	ZeroMemory(&portalDepthStecilStateDesc, sizeof(D3D11_DEPTH_STENCIL_DESC));
	portalDepthStecilStateDesc.DepthEnable = true;
	portalDepthStecilStateDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ZERO;
	portalDepthStecilStateDesc.DepthFunc = D3D11_COMPARISON_LESS;
	portalDepthStecilStateDesc.StencilEnable = true;
	portalDepthStecilStateDesc.StencilReadMask = 0xFF;
	portalDepthStecilStateDesc.StencilWriteMask = 0xFF;
	portalDepthStecilStateDesc.FrontFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
	portalDepthStecilStateDesc.FrontFace.StencilPassOp = D3D11_STENCIL_OP_REPLACE;
	portalDepthStecilStateDesc.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
	portalDepthStecilStateDesc.BackFace.StencilPassOp = D3D11_STENCIL_OP_REPLACE;
	portalDepthStecilStateDesc.BackFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
	device->CreateDepthStencilState(&portalDepthStecilStateDesc, &shapeDepthStencilState);

	ZeroMemory(&portalDepthStecilStateDesc, sizeof(D3D11_DEPTH_STENCIL_DESC));
	portalDepthStecilStateDesc.DepthEnable = true;
	portalDepthStecilStateDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
	portalDepthStecilStateDesc.DepthFunc = D3D11_COMPARISON_LESS;
	portalDepthStecilStateDesc.StencilEnable = true;
	portalDepthStecilStateDesc.StencilReadMask = 0xFF;
	portalDepthStecilStateDesc.StencilWriteMask = 0xFF;
	portalDepthStecilStateDesc.FrontFace.StencilFunc = D3D11_COMPARISON_EQUAL;
	portalDepthStecilStateDesc.FrontFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilFunc = D3D11_COMPARISON_EQUAL;
	portalDepthStecilStateDesc.BackFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
	device->CreateDepthStencilState(&portalDepthStecilStateDesc, &drawDepthStencilState);

	ZeroMemory(&portalDepthStecilStateDesc, sizeof(D3D11_DEPTH_STENCIL_DESC));
	portalDepthStecilStateDesc.DepthEnable = true;
	portalDepthStecilStateDesc.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
	portalDepthStecilStateDesc.DepthFunc = D3D11_COMPARISON_LESS;
	portalDepthStecilStateDesc.StencilEnable = true;
	portalDepthStecilStateDesc.StencilReadMask = 0xFF;
	portalDepthStecilStateDesc.StencilWriteMask = 0xFF;
	portalDepthStecilStateDesc.FrontFace.StencilFunc = D3D11_COMPARISON_NOT_EQUAL;
	portalDepthStecilStateDesc.FrontFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilFunc = D3D11_COMPARISON_NOT_EQUAL;
	portalDepthStecilStateDesc.BackFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
	portalDepthStecilStateDesc.BackFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
	device->CreateDepthStencilState(&portalDepthStecilStateDesc, &clipDepthStencilState);

	D3D11_TEXTURE2D_DESC portalTextureDesc;
	ZeroMemory(&portalTextureDesc, sizeof(D3D11_TEXTURE2D_DESC));
	portalTextureDesc.Width = BACKBUFFER_WIDTH;
	portalTextureDesc.Height = BACKBUFFER_HEIGHT;
	portalTextureDesc.MipLevels = 1;
	portalTextureDesc.ArraySize = 1;
	portalTextureDesc.Format = DXGI_FORMAT_R32G32B32A32_FLOAT;
	portalTextureDesc.SampleDesc.Count = MSAAcount;
	portalTextureDesc.Usage = D3D11_USAGE_DEFAULT;
	portalTextureDesc.BindFlags = D3D11_BIND_RENDER_TARGET | D3D11_BIND_SHADER_RESOURCE;
	portalTextureDesc.CPUAccessFlags = 0;
	portalTextureDesc.MiscFlags = 0;

	device->CreateTexture2D(&portalTextureDesc, NULL, &portal1Texture);
	device->CreateTexture2D(&portalTextureDesc, NULL, &portal2Texture);

	D3D11_RENDER_TARGET_VIEW_DESC renderTargetViewDesc;
	ZeroMemory(&renderTargetViewDesc, sizeof(D3D11_RENDER_TARGET_VIEW_DESC));
	renderTargetViewDesc.Format = portalTextureDesc.Format;
	renderTargetViewDesc.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2DMS;
	renderTargetViewDesc.Texture2D.MipSlice = 0;

	device->CreateRenderTargetView(portal1Texture, &renderTargetViewDesc, &portal1RenderTargetView);
	device->CreateRenderTargetView(portal2Texture, &renderTargetViewDesc, &portal2RenderTargetView);

	D3D11_SHADER_RESOURCE_VIEW_DESC shaderResourceViewDesc;
	ZeroMemory(&shaderResourceViewDesc, sizeof(D3D11_SHADER_RESOURCE_VIEW_DESC));
	shaderResourceViewDesc.Format = portalTextureDesc.Format;
	shaderResourceViewDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2DMS;
	shaderResourceViewDesc.Texture2D.MostDetailedMip = 0;
	shaderResourceViewDesc.Texture2D.MipLevels = 1;

	device->CreateShaderResourceView(portal1Texture, &shaderResourceViewDesc, &portal1ShaderResourceView);
	device->CreateShaderResourceView(portal2Texture, &shaderResourceViewDesc, &portal2ShaderResourceView);

	portal1ViewMatrix = XMMatrixIdentity();
	portal2ViewMatrix = XMMatrixIdentity();
	//////////////////// ^ PORTALS ^ ///////////////////

	//////////////////// v STAR v ///////////////////
	unsigned int numVertices = 12;
	float angle = 360.0f / (numVertices - 2);
	VERTEX starData[12];
	ZeroMemory(starData, sizeof(VERTEX) * numVertices);

	starData[0].pos = XMFLOAT3(0.0f, 0.0f, -0.25f);
	starData[0].color = XMFLOAT4(0.0f, 0.0f, 1.0f, 1.0f);

	starData[numVertices - 1].pos = XMFLOAT3(0.0f, 0.0f, 0.25f);
	starData[numVertices - 1].color = XMFLOAT4(0.0f, 0.0f, 1.0f, 1.0f);
	for (unsigned int i = 0; i < numVertices - 2; i++)
	{
		if ((i + 1) % 2 == 0)
		{
			starData[i + 1].pos = XMFLOAT3(0.5f * sin((float)(i * angle) * (PI / 180.0f)), 0.5f * cos((float)(i * angle) * (PI / 180.0f)), 0.0f);
			starData[i + 1].color = XMFLOAT4(1.0f, 0.0f, 0.0f, 1.0f);
		}
		else
		{
			starData[i + 1].pos = XMFLOAT3(sin((float)(i * angle) * (PI / 180.0f)), cos((float)(i * angle) * (PI / 180.0f)), 0.0f);
			starData[i + 1].color = XMFLOAT4(0.0f, 1.0f, 0.0f, 1.0f);
		}
	}

	unsigned int starIndices[60];
	unsigned int currIndex = 0;
	for (unsigned int i = 0; i < 9; i++)
	{
		starIndices[currIndex] = 0;
		currIndex++;
		starIndices[currIndex] = i + 1;
		currIndex++;
		starIndices[currIndex] = i + 2;
		currIndex++;
	}
	starIndices[currIndex] = 0;
	currIndex++;
	starIndices[currIndex] = 10;
	currIndex++;
	starIndices[currIndex] = 1;
	currIndex++;

	for (unsigned int i = 0; i < 9; i++)
	{
		starIndices[currIndex] = 11;
		currIndex++;
		starIndices[currIndex] = i + 2;
		currIndex++;
		starIndices[currIndex] = i + 1;
		currIndex++;
	}
	starIndices[currIndex] = 11;
	currIndex++;
	starIndices[currIndex] = 1;
	currIndex++;
	starIndices[currIndex] = 10;

	star.Initialize(device, starData, sizeof(starData), starIndices, sizeof(starIndices), Star_VS, sizeof(Star_VS), Star_PS, sizeof(Star_PS), nullptr, 0);

	D3D11_BUFFER_DESC starObjectShaderBufferDesc;
	ZeroMemory(&starObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	starObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	starObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	starObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	starObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&starObjectShaderBufferDesc, NULL, &starObjectShaderBuffer);

	starToObject.localMatrix = XMMatrixIdentity();
	starToObject.localMatrix.r[3].m128_f32[0] = 6.0f;
	starToObject.localMatrix.r[3].m128_f32[1] = 0.0f;
	starToObject.localMatrix.r[3].m128_f32[2] = 4.5f;
	//////////////////// ^ STAR ^ ///////////////////

	//////////////////// v ROCK v ///////////////////
	int numIndicies = 10000;
	vector<VERTEX> rockData;
	unsigned int* rockIndicies;
	rockIndicies = new unsigned int[numIndicies];

	LoadOBJ("Models\\Rock.obj", &rockData, rockIndicies);
	
	rock.Initialize(device, rockData, sizeof(VERTEX) * (UINT)rockData.size(), rockIndicies, sizeof(unsigned int) * numIndicies, Rock_VS, sizeof(Rock_VS), Rock_PS, sizeof(Rock_PS));
	delete[] rockIndicies;

	D3D11_BUFFER_DESC rockObjectShaderBufferDesc;
	ZeroMemory(&rockObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	rockObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	rockObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT) * 5;
	rockObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	rockObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&rockObjectShaderBufferDesc, NULL, &rockObjectShaderBuffer);

	for (int i = 0; i < 5; i++)
	{
		SEND_TO_VRAM_OBJECT temp;
		temp.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(0.75f, 0.75f, 0.75f));
		temp.localMatrix.r[3].m128_f32[0] = -7.0f;
		temp.localMatrix.r[3].m128_f32[1] = -1.5f;
		temp.localMatrix.r[3].m128_f32[2] = -7.0f + i * 2.5f;

		rockToObject.push_back(temp);
	}
	rockToObject.shrink_to_fit();

	CreateDDSTextureFromFile(device, L"Textures\\RockDiffuse.dds", NULL, &rockShaderResourceView);
	//////////////////// ^ ROCK ^ ///////////////////

	//////////////////// v FLASHLIGHT v ///////////////////
	numIndicies = 10000;
	vector<VERTEX> flashlightData;
	unsigned int* flashlightIndicies;
	flashlightIndicies = new unsigned int[numIndicies];

	LoadOBJ("Models\\Flashlight.obj", &flashlightData, flashlightIndicies);

	flashlight.Initialize(device, flashlightData, sizeof(VERTEX) * (UINT)flashlightData.size(), flashlightIndicies, sizeof(unsigned int) * numIndicies, Stage_VS, sizeof(Stage_VS), Texture_PS, sizeof(Texture_PS));
	delete[] flashlightIndicies;

	D3D11_BUFFER_DESC flashlightObjectShaderBufferDesc;
	ZeroMemory(&flashlightObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	flashlightObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	flashlightObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	flashlightObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	flashlightObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&flashlightObjectShaderBufferDesc, NULL, &flashlightObjectShaderBuffer);

	flashlightToObject.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(1.0f, 1.0f, 1.0f));

	CreateDDSTextureFromFile(device, L"Textures\\Flashlight.dds", NULL, &flashlightShaderResourceView);
	//////////////////// ^ FLASHLIGHT ^ ///////////////////

	//////////////////// v BALL PIT v ///////////////////
	D3D11_BUFFER_DESC ballObjectShaderBufferDesc;
	ZeroMemory(&ballObjectShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	ballObjectShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	ballObjectShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_OBJECT);
	ballObjectShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	ballObjectShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&ballObjectShaderBufferDesc, NULL, &ballObjectShaderBuffer);

	D3D11_BUFFER_DESC structBufferDesc;
	ZeroMemory(&structBufferDesc, sizeof(D3D11_BUFFER_DESC));
	structBufferDesc.BindFlags = D3D11_BIND_UNORDERED_ACCESS | D3D11_BIND_SHADER_RESOURCE;
	structBufferDesc.ByteWidth = sizeof(particle);
	structBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_READ | D3D11_CPU_ACCESS_WRITE;
	structBufferDesc.Usage = D3D11_USAGE_DEFAULT;
	structBufferDesc.MiscFlags = D3D11_RESOURCE_MISC_BUFFER_STRUCTURED;
	structBufferDesc.StructureByteStride = sizeof(particle);
	device->CreateBuffer(&structBufferDesc, NULL, &structBuffer);

	ZeroMemory(&particles, sizeof(particles));
	for (int i = 0; i < 25; i++)
	{
		//particles[i].posX = ((10.0f - 5.0f)*((float)rand() / RAND_MAX)) + 5.0f;
		//particles[i].posY = ((8.5f - -1.5f)*((float)rand() / RAND_MAX)) + -1.5f;
		//particles[i].posZ = ((-5.0f - -10.0f)*((float)rand() / RAND_MAX)) + -10.0f;
		particles[i].posX = -7.0f;
		particles[i].posY = -1.4f;
		particles[i].posZ = 7.0f;
		particles[i].radius = 0.1f;
		particles[i].velX = ((1.5f - -1.5f)*((float)rand() / RAND_MAX)) + -1.5f;
		particles[i].velY = 5.0f;
		particles[i].velZ = ((1.5f - -1.5f)*((float)rand() / RAND_MAX)) + -1.5f;
		particles[i].mass = 1.0f;
		particles[i].deltaTime = 0.0f;
	}

	ZeroMemory(&ballShaderResourceViewDesc, sizeof(D3D11_SHADER_RESOURCE_VIEW_DESC));
	ballShaderResourceViewDesc.Format = DXGI_FORMAT_UNKNOWN;
	ballShaderResourceViewDesc.ViewDimension = D3D11_SRV_DIMENSION_BUFFER;
	ballShaderResourceViewDesc.Buffer.ElementWidth = 1;
	device->CreateShaderResourceView(structBuffer, &ballShaderResourceViewDesc, &ballShaderResourceView);

	ZeroMemory(&ballUnorderedAccessViewDesc, sizeof(D3D11_UNORDERED_ACCESS_VIEW_DESC));
	ballUnorderedAccessViewDesc.Buffer.FirstElement = 0;
	ballUnorderedAccessViewDesc.Buffer.NumElements = 1;
	ballUnorderedAccessViewDesc.Buffer.Flags = D3D11_BUFFER_UAV_FLAG_COUNTER;
	ballUnorderedAccessViewDesc.Format = DXGI_FORMAT_UNKNOWN;
	ballUnorderedAccessViewDesc.ViewDimension = D3D11_UAV_DIMENSION_BUFFER;
	device->CreateUnorderedAccessView(structBuffer, &ballUnorderedAccessViewDesc, &ballUnorderedAccessView);

	VERTEX ballData[8] =
	{
		{ { 0.0f, 0.0f }, { -1.0f, 1.0f, -1.0f }, { 0.0f, 0.0f, -1.0f }, { 1.0f, 0.0f, 0.0f, 1.0f } },
		{ { 1.0f, 0.0f }, { 1.0f, 1.0f, -1.0f }, { 0.0f, 0.0f, -1.0f }, { 0.0f, 1.0f, 0.0f, 1.0f } },
		{ { 1.0f, 1.0f }, { 1.0f, -1.0f, -1.0f }, { 0.0f, 0.0f, -1.0f }, { 0.0f, 0.0f, 1.0f, 1.0f } },
		{ { 0.0f, 1.0f }, { -1.0f, -1.0f, -1.0f }, { 0.0f, 0.0f, -1.0f }, { 1.0f, 1.0f, 0.0f, 1.0f } },

		{ { 0.0f, 0.0f }, { -1.0f, 1.0f, 1.0f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 1.0f, 0.0f, 1.0f } },
		{ { 1.0f, 0.0f }, { 1.0f, 1.0f, 1.0f }, { 0.0f, 0.0f, 1.0f }, { 1.0f, 0.0f, 0.0f, 1.0f } },
		{ { 1.0f, 1.0f }, { 1.0f, -1.0f, 1.0f }, { 0.0f, 0.0f, 1.0f }, { 1.0f, 1.0f, 0.0f, 1.0f } },
		{ { 0.0f, 1.0f }, { -1.0f, -1.0f, 1.0f }, { 0.0f, 0.0f, 1.0f }, { 0.0f, 0.0f, 1.0f, 1.0f } }
	};

	unsigned int ballIndicies[36] =
	{
		// front side
		0, 1, 2,
		0, 2, 3,

		// back side
		4, 6, 5,
		4, 7, 6,

		// top
		4, 5, 1,
		4, 1, 0,

		// right
		1, 5, 6,
		1, 6, 2,

		// bottom
		3, 2, 6,
		3, 6, 7,

		// left
		4, 0, 3,
		4, 3, 7
	};

	ball.Initialize(device, ballData, sizeof(ballData), ballIndicies, sizeof(ballIndicies), Star_VS, sizeof(Star_VS), Star_PS, sizeof(Star_PS), Physics_CS, sizeof(Physics_CS));
	device->CreateComputeShader(Physics_CS, sizeof(Physics_CS), NULL, &ComputeShader);

	ballToObject.localMatrix = XMMatrixMultiply(XMMatrixIdentity(), XMMatrixScaling(particles[0].radius, particles[0].radius, particles[0].radius));
	//////////////////// ^ BALL PIT ^ ///////////////////

	D3D11_TEXTURE2D_DESC depthStencilDesc;
	ZeroMemory(&depthStencilDesc, sizeof(D3D11_TEXTURE2D_DESC));
	depthStencilDesc.Width = BACKBUFFER_WIDTH;
	depthStencilDesc.Height = BACKBUFFER_HEIGHT;
	depthStencilDesc.MipLevels = 1;
	depthStencilDesc.ArraySize = 1;
	depthStencilDesc.Format = DXGI_FORMAT_D24_UNORM_S8_UINT;
	depthStencilDesc.SampleDesc.Count = MSAAcount;
	depthStencilDesc.SampleDesc.Quality = 0;
	depthStencilDesc.Usage = D3D11_USAGE_DEFAULT;
	depthStencilDesc.BindFlags = D3D11_BIND_DEPTH_STENCIL;
	//depthStencilDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE | D3D11_CPU_ACCESS_READ;
	depthStencilDesc.MiscFlags = 0;
	device->CreateTexture2D(&depthStencilDesc, NULL, &depthStencil);

	D3D11_DEPTH_STENCIL_VIEW_DESC depthStencilViewDesc;
	ZeroMemory(&depthStencilViewDesc, sizeof(D3D11_DEPTH_STENCIL_VIEW_DESC));
	depthStencilViewDesc.Format = DXGI_FORMAT_D32_FLOAT;
	depthStencilViewDesc.ViewDimension = D3D11_DSV_DIMENSION_TEXTURE2D;
	depthStencilViewDesc.Texture2D.MipSlice = 0;

	device->CreateDepthStencilView(	depthStencil,			// Depth stencil texture
									/*&depthStencilViewDesc*/0,	// Depth stencil desc
									&depthStencilView);		// [out] Depth stencil view

	D3D11_SAMPLER_DESC samplerStateDesc;
	ZeroMemory(&samplerStateDesc, sizeof(D3D11_SAMPLER_DESC));
	samplerStateDesc.Filter = D3D11_FILTER_ANISOTROPIC;
	samplerStateDesc.AddressU = D3D11_TEXTURE_ADDRESS_WRAP;
	samplerStateDesc.AddressV = D3D11_TEXTURE_ADDRESS_WRAP;
	samplerStateDesc.AddressW = D3D11_TEXTURE_ADDRESS_WRAP;
	samplerStateDesc.MaxAnisotropy = 16;
	device->CreateSamplerState(&samplerStateDesc, &samplerState);

	D3D11_RASTERIZER_DESC MSAAEnabledStateDesc;
	ZeroMemory(&MSAAEnabledStateDesc, sizeof(D3D11_RASTERIZER_DESC));
	MSAAEnabledStateDesc.FillMode = D3D11_FILL_SOLID;
	MSAAEnabledStateDesc.CullMode = D3D11_CULL_BACK;
	MSAAEnabledStateDesc.MultisampleEnable = true;
	device->CreateRasterizerState(&MSAAEnabledStateDesc, &MSAAEnabled);

	D3D11_RASTERIZER_DESC FrontFaceCullingStateDesc;
	ZeroMemory(&FrontFaceCullingStateDesc, sizeof(D3D11_RASTERIZER_DESC));
	FrontFaceCullingStateDesc.FillMode = D3D11_FILL_SOLID;
	FrontFaceCullingStateDesc.CullMode = D3D11_CULL_FRONT;
	FrontFaceCullingStateDesc.MultisampleEnable = true;
	device->CreateRasterizerState(&FrontFaceCullingStateDesc, &FrontFaceCulling);

	D3D11_BUFFER_DESC cameraSceneShaderBufferDesc;
	ZeroMemory(&cameraSceneShaderBufferDesc, sizeof(D3D11_BUFFER_DESC));
	cameraSceneShaderBufferDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
	cameraSceneShaderBufferDesc.ByteWidth = sizeof(SEND_TO_VRAM_SCENE);
	cameraSceneShaderBufferDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
	cameraSceneShaderBufferDesc.Usage = D3D11_USAGE_DYNAMIC;
	device->CreateBuffer(&cameraSceneShaderBufferDesc, NULL, &cameraSceneShaderBuffer);
	cameraToScene.viewMatrix = viewMatrix;
	cameraToScene.projMatrix = projectionMatrix;

	viewMatrix = XMMatrixIdentity();
	viewMatrix.r[3].m128_f32[0] = 0.0f;
	viewMatrix.r[3].m128_f32[1] = 0.0f;
	viewMatrix.r[3].m128_f32[2] = -7.0f;
	projectionMatrix = XMMatrixIdentity();

	miniViewMatrix = XMMatrixMultiply(XMMatrixRotationX(XMConvertToRadians(90)),XMMatrixIdentity());
	miniViewMatrix.r[3].m128_f32[0] = 0.0f;
	miniViewMatrix.r[3].m128_f32[1] = 14.0f;
	miniViewMatrix.r[3].m128_f32[2] = 0.0f;
	miniProjectionMatrix = XMMatrixIdentity();

	controlsEnabled = false;
	showCursor = true;
	viewAngleX = 0.0f;
	viewAngleY = 0.0f;
	viewX = 0.0f;
	viewY = 0.0f;
	viewZ = 0.0f;

	indexOffset = 0;
	timekeep = 0.0f;
}

//************************************************************
//************ RESIZING THE WINDOW ***************************
//************************************************************

bool DEMO_APP::Resize()
{
	GetWindowRect(window, &windowRect);
	width = (windowRect.right - windowRect.left) - 16;
	height = (windowRect.bottom - windowRect.top) - 39;
	centerX = (int)((float)width * 0.5f + windowRect.left);
	centerY = (int)((float)height * 0.5f + windowRect.top);

	if (renderTargetView != nullptr)
		renderTargetView->Release();
	if (depthStencil != nullptr)
		depthStencil->Release();
	if (depthStencilView != nullptr)
		depthStencilView->Release();
	if (portal1Texture != nullptr)
		portal1Texture->Release();
	if (portal2Texture != nullptr)
		portal2Texture->Release();
	if (portal1RenderTargetView != nullptr)
		portal1RenderTargetView->Release();
	if (portal2RenderTargetView != nullptr)
		portal2RenderTargetView->Release();
	if (portal1ShaderResourceView != nullptr)
		portal1ShaderResourceView->Release();
	if (portal2ShaderResourceView != nullptr)
		portal2ShaderResourceView->Release();

	// back buffer
	swapchain->ResizeBuffers(1, width, height, DXGI_FORMAT_UNKNOWN, DXGI_SWAP_CHAIN_FLAG_ALLOW_MODE_SWITCH);
	ID3D11Resource* resource;
	swapchain->GetBuffer(0, __uuidof(resource), (void**)(&resource));
	device->CreateRenderTargetView(resource, NULL, &renderTargetView);
	resource->Release();

	// viewport
	ZeroMemory(&viewport, sizeof(D3D11_VIEWPORT));
	viewport.MinDepth = 0.0f;
	viewport.MaxDepth = 1.0f;
	viewport.Width = (FLOAT)width;
	viewport.Height = (FLOAT)height;
	viewport.TopLeftX = 0.0f;
	viewport.TopLeftY = 0.0f;

	// mini viewport
	ZeroMemory(&miniViewport, sizeof(D3D11_VIEWPORT));
	miniViewport.MinDepth = 0.0f;
	miniViewport.MaxDepth = 1.0f;
	miniViewport.Width = (FLOAT)width / 4;
	miniViewport.Height = (FLOAT)height / 4;
	miniViewport.TopLeftX = 0.0f;
	miniViewport.TopLeftY = 0.0f;

	// depth buffer
	D3D11_TEXTURE2D_DESC depthStencilDesc;
	ZeroMemory(&depthStencilDesc, sizeof(D3D11_TEXTURE2D_DESC));
	depthStencilDesc.Width = (UINT)width;
	depthStencilDesc.Height = (UINT)height;
	depthStencilDesc.MipLevels = 1;
	depthStencilDesc.ArraySize = 1;
	depthStencilDesc.Format = DXGI_FORMAT_D24_UNORM_S8_UINT;
	depthStencilDesc.SampleDesc.Count = MSAAcount;
	depthStencilDesc.SampleDesc.Quality = 0;
	depthStencilDesc.Usage = D3D11_USAGE_DEFAULT;
	depthStencilDesc.BindFlags = D3D11_BIND_DEPTH_STENCIL;
	//depthStencilDesc.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE | D3D11_CPU_ACCESS_READ;
	depthStencilDesc.MiscFlags = 0;
	device->CreateTexture2D(&depthStencilDesc, NULL, &depthStencil);

	D3D11_DEPTH_STENCIL_VIEW_DESC depthStencilViewDesc;
	ZeroMemory(&depthStencilViewDesc, sizeof(D3D11_DEPTH_STENCIL_VIEW_DESC));
	//depthStencilViewDesc.Format = DXGI_FORMAT_D32_FLOAT;
	depthStencilViewDesc.ViewDimension = D3D11_DSV_DIMENSION_TEXTURE2D;
	depthStencilViewDesc.Texture2D.MipSlice = 0;

	device->CreateDepthStencilView(	depthStencil,			// Depth stencil texture
									/*&depthStencilViewDesc*/0,	// Depth stencil desc
									&depthStencilView);		// [out] Depth stencil view

	// Portal Textures
	D3D11_TEXTURE2D_DESC portalTextureDesc;
	ZeroMemory(&portalTextureDesc, sizeof(D3D11_TEXTURE2D_DESC));
	portalTextureDesc.Width = (UINT)width;
	portalTextureDesc.Height = (UINT)height;
	portalTextureDesc.MipLevels = 1;
	portalTextureDesc.ArraySize = 1;
	portalTextureDesc.Format = DXGI_FORMAT_R32G32B32A32_FLOAT;
	portalTextureDesc.SampleDesc.Count = MSAAcount;
	portalTextureDesc.Usage = D3D11_USAGE_DEFAULT;
	portalTextureDesc.BindFlags = D3D11_BIND_RENDER_TARGET | D3D11_BIND_SHADER_RESOURCE;
	portalTextureDesc.CPUAccessFlags = 0;
	portalTextureDesc.MiscFlags = 0;

	device->CreateTexture2D(&portalTextureDesc, NULL, &portal1Texture);
	device->CreateTexture2D(&portalTextureDesc, NULL, &portal2Texture);

	D3D11_RENDER_TARGET_VIEW_DESC renderTargetViewDesc;
	renderTargetViewDesc.Format = portalTextureDesc.Format;
	renderTargetViewDesc.ViewDimension = D3D11_RTV_DIMENSION_TEXTURE2DMS;
	renderTargetViewDesc.Texture2D.MipSlice = 0;

	device->CreateRenderTargetView(portal1Texture, &renderTargetViewDesc, &portal1RenderTargetView);
	device->CreateRenderTargetView(portal2Texture, &renderTargetViewDesc, &portal2RenderTargetView);

	D3D11_SHADER_RESOURCE_VIEW_DESC shaderResourceViewDesc;
	shaderResourceViewDesc.Format = portalTextureDesc.Format;
	shaderResourceViewDesc.ViewDimension = D3D11_SRV_DIMENSION_TEXTURE2DMS;
	shaderResourceViewDesc.Texture2D.MostDetailedMip = 0;
	shaderResourceViewDesc.Texture2D.MipLevels = 1;

	device->CreateShaderResourceView(portal1Texture, &shaderResourceViewDesc, &portal1ShaderResourceView);
	device->CreateShaderResourceView(portal2Texture, &shaderResourceViewDesc, &portal2ShaderResourceView);

	// projection matrix
	float verticalFOV = 75.0f;
	float aspectRatio = (float)width / (float)height;
	float Znear = 0.1f;
	float Zfar = 100.0f;
	projectionMatrix = XMMatrixPerspectiveFovLH(XMConvertToRadians(verticalFOV), aspectRatio, Znear, Zfar);

	// mini projection matrix
	aspectRatio = (float)(width / 4) / (float)(height / 4);
	miniProjectionMatrix = XMMatrixPerspectiveFovLH(XMConvertToRadians(verticalFOV), aspectRatio, Znear, Zfar);

	return true;
}

//************************************************************
//************ EXECUTION *************************************
//************************************************************

bool DEMO_APP::Run()
{
	timer.Signal();
	timekeep += (float)timer.SmoothDelta();

	GetWindowRect(window, &windowRect);
	int width = (windowRect.right - windowRect.left) - 16;
	int height = (windowRect.bottom - windowRect.top) - 39;
	int centerX = (int)((float)width * 0.5f + windowRect.left);
	int centerY = (int)((float)height * 0.5f + windowRect.top);

	viewX = 0.0f;
	viewY = 0.0f;
	viewZ = 0.0f;
	float multiplier;
	if (GetAsyncKeyState(VK_TAB) & 0x1)
	{
		controlsEnabled = !controlsEnabled;
		if (controlsEnabled)
			SetCursorPos(centerX, centerY);
	}
	if (controlsEnabled)
	{
		if (showCursor)
		{
			showCursor = false;
			ShowCursor(showCursor);
		}
		else
		{
			GetCursorPos(&cursor);
			if (cursor.x != centerX || cursor.y != centerY)
			{
				viewAngleY -= (float)((centerX - cursor.x) * timer.SmoothDelta() * 0.5f);
				viewAngleX -= (float)((centerY - cursor.y) * timer.SmoothDelta() * 0.5f);
				if (viewAngleX > XMConvertToRadians(90))
					viewAngleX = XMConvertToRadians(90);
				if (viewAngleX < XMConvertToRadians(-90))
					viewAngleX = XMConvertToRadians(-90);
				SetCursorPos(centerX, centerY);
			}
		}

		if (GetAsyncKeyState(VK_LSHIFT))
			multiplier = 5.0f;
		else
			multiplier = 2.0f;
		if (GetAsyncKeyState('W'))
			viewZ += (float)timer.Delta() * multiplier;
		if (GetAsyncKeyState('S'))
			viewZ -= (float)timer.Delta() * multiplier;
		if (GetAsyncKeyState('A'))
			viewX -= (float)timer.Delta() * multiplier;
		if (GetAsyncKeyState('D'))
			viewX += (float)timer.Delta() * multiplier;
		if (GetAsyncKeyState(VK_SPACE))
			viewY += (float)timer.Delta() * multiplier;
		if (GetAsyncKeyState('C'))
			viewY -= (float)timer.Delta() * multiplier;
		if (GetAsyncKeyState('I'))
			ToLight[1].pos.z += (float)timer.Delta() * 3.0f;
		if (GetAsyncKeyState('K'))
			ToLight[1].pos.z -= (float)timer.Delta() * 3.0f;
		if (GetAsyncKeyState('J'))
			ToLight[1].pos.x -= (float)timer.Delta() * 3.0f;
		if (GetAsyncKeyState('L'))
			ToLight[1].pos.x += (float)timer.Delta() * 3.0f;

		//if (GetAsyncKeyState('K'))
		//{
		//	skyboxShaderResView->Release();
		//	CreateDDSTextureFromFile(device, L"UnderWater.dds", NULL, &skyboxShaderResView);
		//}
	}
	else
	{
		if (!showCursor)
		{
			showCursor = true;
			ShowCursor(showCursor);
		}
	}

	XMMATRIX viewRotationX = XMMatrixRotationX(viewAngleX);
	XMMATRIX viewRotationY = XMMatrixRotationY(viewAngleY);
	XMMATRIX viewTranslation = XMMatrixTranslation(viewX, viewY, viewZ);

	float X = viewMatrix.r[3].m128_f32[0];
	float Y = viewMatrix.r[3].m128_f32[1];
	float Z = viewMatrix.r[3].m128_f32[2];

	XMMATRIX tempMatrix = XMMatrixIdentity();
	tempMatrix = XMMatrixMultiply(viewRotationX, tempMatrix);
	tempMatrix = XMMatrixMultiply(tempMatrix, viewRotationY);
	tempMatrix.r[3].m128_f32[0] = X;
	tempMatrix.r[3].m128_f32[1] = Y;
	tempMatrix.r[3].m128_f32[2] = Z;

	viewMatrix = XMMatrixMultiply(viewTranslation, tempMatrix);
	X = viewMatrix.r[3].m128_f32[0];
	Y = viewMatrix.r[3].m128_f32[1];
	Z = viewMatrix.r[3].m128_f32[2];

	float verticalFOV = 75.0f;
	float aspectRatio = (float)width / (float)height;
	float Znear = 0.0001f;
	float Zfar = 100.0f;
	projectionMatrix = XMMatrixPerspectiveFovLH(XMConvertToRadians(verticalFOV), aspectRatio, Znear, Zfar);

	aspectRatio = (float)(width / 4) / (float)(height / 4);
	miniProjectionMatrix = XMMatrixPerspectiveFovLH(XMConvertToRadians(verticalFOV), aspectRatio, Znear, Zfar);

	cameraToScene.viewMatrix = XMMatrixInverse(NULL, viewMatrix);
	cameraToScene.projMatrix = projectionMatrix;

	XMMATRIX viewRotationY1 = XMMatrixRotationY(viewAngleY + XMConvertToRadians(180));
	
	float X1 = portal2ToObject.localMatrix.r[3].m128_f32[0] + (portal1ToObject.localMatrix.r[3].m128_f32[0] - viewMatrix.r[3].m128_f32[0]);
	float Y1 = viewMatrix.r[3].m128_f32[1];
	float Z1 = portal2ToObject.localMatrix.r[3].m128_f32[2] + (portal1ToObject.localMatrix.r[3].m128_f32[2] - viewMatrix.r[3].m128_f32[2]);
	tempMatrix = XMMatrixIdentity();
	tempMatrix = XMMatrixMultiply(viewRotationX, tempMatrix);
	tempMatrix = XMMatrixMultiply(tempMatrix, viewRotationY1);
	tempMatrix.r[3].m128_f32[0] = X1;
	tempMatrix.r[3].m128_f32[1] = Y1;
	tempMatrix.r[3].m128_f32[2] = Z1;
	portal1ViewMatrix = tempMatrix;

	float X2 = portal1ToObject.localMatrix.r[3].m128_f32[0] + (portal2ToObject.localMatrix.r[3].m128_f32[0] - viewMatrix.r[3].m128_f32[0]);
	float Y2 = viewMatrix.r[3].m128_f32[1];
	float Z2 = portal1ToObject.localMatrix.r[3].m128_f32[2] + (portal2ToObject.localMatrix.r[3].m128_f32[2] - viewMatrix.r[3].m128_f32[2]);
	tempMatrix = XMMatrixIdentity();
	tempMatrix = XMMatrixMultiply(viewRotationX, tempMatrix);
	tempMatrix = XMMatrixMultiply(tempMatrix, viewRotationY1);
	tempMatrix.r[3].m128_f32[0] = X2;
	tempMatrix.r[3].m128_f32[1] = Y2;
	tempMatrix.r[3].m128_f32[2] = Z2;
	portal2ViewMatrix = tempMatrix;

	//////////////////// v STAR SETUP v ////////////////////
	XMMATRIX rotation = XMMatrixRotationY(1.0f * (float)timer.Delta());
	starToObject.localMatrix = XMMatrixMultiply(rotation, starToObject.localMatrix);
	//////////////////// ^ STAR SETUP ^ ////////////////////

	//////////////////// v FLASHLIGHT SETUP v ///////////////////
	flashlightToObject.localMatrix = viewMatrix;
	flashlightToObject.localMatrix = XMMatrixMultiply(XMMatrixTranslation(0.75f, -0.7f, 0.5f), flashlightToObject.localMatrix);
	flashlightToObject.localMatrix = XMMatrixMultiply(XMMatrixRotationX(0.0f), flashlightToObject.localMatrix);
	flashlightToObject.localMatrix = XMMatrixMultiply(XMMatrixRotationY(1.5f), flashlightToObject.localMatrix);
	flashlightToObject.localMatrix = XMMatrixMultiply(XMMatrixRotationZ(0.0f), flashlightToObject.localMatrix);
	flashlightToObject.localMatrix = XMMatrixMultiply(XMMatrixScaling(0.005f, 0.005f, 0.005f), flashlightToObject.localMatrix);
	//////////////////// ^ FLASHLIGHT SETUP ^ ///////////////////

	ToLight[2].pos.x = viewMatrix.r[3].m128_f32[0];
	ToLight[2].pos.y = viewMatrix.r[3].m128_f32[1];
	ToLight[2].pos.z = viewMatrix.r[3].m128_f32[2];
	ToLight[2].dir.x = viewMatrix.r[2].m128_f32[0];
	ToLight[2].dir.y = viewMatrix.r[2].m128_f32[1];
	ToLight[2].dir.z = viewMatrix.r[2].m128_f32[2];

	//////////////////// v BALL PIT SETUP v ///////////////////
	if (timekeep >= 0.05f)
	{
		timekeep = 0.0f;
		indexOffset++;
		if (indexOffset >= 25)
		{
			indexOffset = 0;
		}
	}
	for (int j = 0; j < 25; j++)
	{
		int i = j + indexOffset;
		if (i >= 25)
			i = i - 25;

		particles[indexOffset].posX = -7.0f;
		particles[indexOffset].posY = -1.4f;
		particles[indexOffset].posZ = 7.0f;
		particles[indexOffset].radius = 0.1f;
		particles[indexOffset].velX = ((1.5f - -1.5f)*((float)rand() / RAND_MAX)) + -1.5f;
		particles[indexOffset].velY = 5.0f;
		particles[indexOffset].velZ = ((1.5f - -1.5f)*((float)rand() / RAND_MAX)) + -1.5f;
		particles[indexOffset].mass = 1.0f;

		particles[i].deltaTime = (float)timer.SmoothDelta();

		deviceContext->Map(structBuffer, NULL, D3D11_MAP_WRITE, NULL, &mapSubresource);
		memcpy_s(mapSubresource.pData, sizeof(particle), &particles[i], sizeof(particle));
		deviceContext->Unmap(structBuffer, NULL);

		if (ballUnorderedAccessView != nullptr)
			ballUnorderedAccessView->Release();
		device->CreateUnorderedAccessView(structBuffer, &ballUnorderedAccessViewDesc, &ballUnorderedAccessView);
		deviceContext->CSSetUnorderedAccessViews(0, 1, &ballUnorderedAccessView, NULL);

		deviceContext->Dispatch(1, 1, 1);

		deviceContext->Map(structBuffer, NULL, D3D11_MAP_WRITE, NULL, &mapSubresource);
		memcpy_s(&particles[i], sizeof(particle), mapSubresource.pData, sizeof(particle));
		deviceContext->Unmap(structBuffer, NULL);
	}
	//////////////////// ^ BALL PIT SETUP ^ ///////////////////

	deviceContext->PSSetSamplers(0, 1, &samplerState);

	deviceContext->RSSetViewports(1, &viewport);

	deviceContext->RSSetState(MSAAEnabled);

	//////////////////// v PORTAL ONE VIEW v ///////////////////
	deviceContext->OMSetRenderTargets(1, &portal1RenderTargetView, depthStencilView);
	deviceContext->ClearRenderTargetView(portal1RenderTargetView, blue);
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH | D3D11_CLEAR_STENCIL, 1.0f, NULL);

	cameraToScene.viewMatrix = XMMatrixInverse(NULL, portal1ViewMatrix);
	deviceContext->Map(cameraSceneShaderBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_SCENE), &cameraToScene, sizeof(SEND_TO_VRAM_SCENE));
	deviceContext->Unmap(cameraSceneShaderBuffer, NULL);
	deviceContext->VSSetConstantBuffers(1, 1, &cameraSceneShaderBuffer);

	// Skybox
	skyboxToObject.localMatrix.r[3].m128_f32[0] = portal1ViewMatrix.r[3].m128_f32[0];
	skyboxToObject.localMatrix.r[3].m128_f32[1] = portal1ViewMatrix.r[3].m128_f32[1];
	skyboxToObject.localMatrix.r[3].m128_f32[2] = portal1ViewMatrix.r[3].m128_f32[2];
	deviceContext->PSSetShaderResources(0, 1, &skyboxShaderResourceView);
	skybox.Draw(deviceContext, skyboxObjectShaderBuffer, skyboxToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH, 1.0f, NULL);
	// Stage
	deviceContext->PSSetShaderResources(0, 1, &stageShaderResourceView);
	stage.Draw(deviceContext, stageObjectShaderBuffer, stageToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 12, 0);
	// Star
	star.Draw(deviceContext, starObjectShaderBuffer, starToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 60, 0);
	// Rock
	deviceContext->PSSetShaderResources(0, 1, &rockShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	rock.Draw(deviceContext, rockObjectShaderBuffer, rockToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	// Ball Pit
	deviceContext->CSSetShader(ComputeShader, NULL, 0);
	for (int j = 0; j < 25; j++)
	{
		int i = j + indexOffset;
		if (i >= 25)
			i = i - 25;

		ballToObject.localMatrix.r[3].m128_f32[0] = particles[i].posX;
		ballToObject.localMatrix.r[3].m128_f32[1] = particles[i].posY;
		ballToObject.localMatrix.r[3].m128_f32[2] = particles[i].posZ;
		ball.Draw(deviceContext, ballObjectShaderBuffer, ballToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	}
	// Flashlight
	deviceContext->PSSetShaderResources(0, 1, &flashlightShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	flashlight.Draw(deviceContext, flashlightObjectShaderBuffer, flashlightToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	//////////////////// ^ PORTAL ONE VIEW ^ ///////////////////

	////////////////// v PORTAL TWO VIEW v ///////////////////
	deviceContext->OMSetRenderTargets(1, &portal2RenderTargetView, depthStencilView);
	deviceContext->ClearRenderTargetView(portal2RenderTargetView, orange);
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH | D3D11_CLEAR_STENCIL, 1.0f, NULL);

	cameraToScene.viewMatrix = XMMatrixInverse(NULL, portal2ViewMatrix);
	deviceContext->Map(cameraSceneShaderBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_SCENE), &cameraToScene, sizeof(SEND_TO_VRAM_SCENE));
	deviceContext->Unmap(cameraSceneShaderBuffer, NULL);
	deviceContext->VSSetConstantBuffers(1, 1, &cameraSceneShaderBuffer);

	// Skybox
	skyboxToObject.localMatrix.r[3].m128_f32[0] = portal2ViewMatrix.r[3].m128_f32[0];
	skyboxToObject.localMatrix.r[3].m128_f32[1] = portal2ViewMatrix.r[3].m128_f32[1];
	skyboxToObject.localMatrix.r[3].m128_f32[2] = portal2ViewMatrix.r[3].m128_f32[2];
	deviceContext->PSSetShaderResources(0, 1, &skyboxShaderResourceView);
	skybox.Draw(deviceContext, skyboxObjectShaderBuffer, skyboxToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH, 1.0f, NULL);
	// Stage
	deviceContext->PSSetShaderResources(0, 1, &stageShaderResourceView);
	stage.Draw(deviceContext, stageObjectShaderBuffer, stageToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 12, 0);
	// Star
	star.Draw(deviceContext, starObjectShaderBuffer, starToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 60, 0);
	// Rock
	deviceContext->PSSetShaderResources(0, 1, &rockShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	rock.Draw(deviceContext, rockObjectShaderBuffer, rockToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	// Ball Pit
	deviceContext->CSSetShader(ComputeShader, NULL, 0);
	for (int j = 0; j < 25; j++)
	{
		int i = j + indexOffset;
		if (i >= 25)
			i = i - 25;

		ballToObject.localMatrix.r[3].m128_f32[0] = particles[i].posX;
		ballToObject.localMatrix.r[3].m128_f32[1] = particles[i].posY;
		ballToObject.localMatrix.r[3].m128_f32[2] = particles[i].posZ;
		ball.Draw(deviceContext, ballObjectShaderBuffer, ballToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	}
	// Flashlight
	deviceContext->PSSetShaderResources(0, 1, &flashlightShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	flashlight.Draw(deviceContext, flashlightObjectShaderBuffer, flashlightToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	////////////////// ^ PORTAL TWO VIEW ^ ///////////////////

	deviceContext->OMSetRenderTargets(1, &renderTargetView, depthStencilView);
	deviceContext->ClearRenderTargetView(renderTargetView, green);
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH | D3D11_CLEAR_STENCIL, 1.0f, NULL);

	cameraToScene.viewMatrix = XMMatrixInverse(NULL, viewMatrix);
	deviceContext->Map(cameraSceneShaderBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_SCENE), &cameraToScene, sizeof(SEND_TO_VRAM_SCENE));
	deviceContext->Unmap(cameraSceneShaderBuffer, NULL);
	deviceContext->VSSetConstantBuffers(1, 1, &cameraSceneShaderBuffer);

	//deviceContext->RSSetState(MSAAEnabled);
	//////////////////// v SKYBOX DRAW v ////////////////////
	skyboxToObject.localMatrix.r[3].m128_f32[0] = X;
	skyboxToObject.localMatrix.r[3].m128_f32[1] = Y;
	skyboxToObject.localMatrix.r[3].m128_f32[2] = Z;

	deviceContext->PSSetShaderResources(0, 1, &skyboxShaderResourceView);
	skybox.Draw(deviceContext, skyboxObjectShaderBuffer, skyboxToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);

	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH, 1.0f, NULL);
	//////////////////// ^ SKYBOX DRAW ^ ////////////////////

	//////////////////// v STAGE DRAW v ///////////////////
	deviceContext->PSSetShaderResources(0, 1, &stageShaderResourceView);
	stage.Draw(deviceContext, stageObjectShaderBuffer, stageToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 12, 0);
	//////////////////// ^ STAGE DRAW ^ ///////////////////

	//////////////////// v STAR DRAW v ////////////////////
	star.Draw(deviceContext, starObjectShaderBuffer, starToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 60, 0);
	//////////////////// ^ STAR DRAW ^ ////////////////////

	//////////////////// v ROCK DRAW v ///////////////////
	deviceContext->PSSetShaderResources(0, 1, &rockShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	rock.Draw(deviceContext, rockObjectShaderBuffer, rockToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	//////////////////// ^ ROCK DRAW ^ ///////////////////

	//////////////////// v BALL PIT DRAW v ///////////////////
	deviceContext->CSSetShader(ComputeShader, NULL, 0);
	for (int j = 0; j < 25; j++)
	{
		int i = j + indexOffset;
		if (i >= 25)
			i = i - 25;

		ballToObject.localMatrix.r[3].m128_f32[0] = particles[i].posX;
		ballToObject.localMatrix.r[3].m128_f32[1] = particles[i].posY;
		ballToObject.localMatrix.r[3].m128_f32[2] = particles[i].posZ;
		ball.Draw(deviceContext, ballObjectShaderBuffer, ballToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	}
	//////////////////// ^ BALL PIT DRAW ^ ///////////////////

	//////////////////// v PORTALS DRAW v ///////////////////
	deviceContext->OMSetDepthStencilState(shapeDepthStencilState, 1);
	deviceContext->PSSetShaderResources(0, 1, &portal1ShaderResourceView);
	portal1.Draw(deviceContext, portal1ObjectShaderBuffer, portal1ToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 150, 0);
	deviceContext->PSSetShaderResources(0, 1, &portal2ShaderResourceView);
	portal2.Draw(deviceContext, portal2ObjectShaderBuffer, portal2ToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 150, 0);
	deviceContext->OMSetDepthStencilState(NULL, 0);
	//////////////////// v PORTALS DRAW v ///////////////////

	//////////////////// v WALL DRAW v ///////////////////
	deviceContext->OMSetDepthStencilState(clipDepthStencilState, 1);
	deviceContext->PSSetShaderResources(0, 1, &wallShaderResourceView);
	wall.Draw(deviceContext, wallObjectShaderBuffer, wallToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	deviceContext->OMSetDepthStencilState(NULL, 0);
	//////////////////// ^ WALL DRAW ^ ///////////////////

	//////////////////// v FLASHLIGHT DRAW v ///////////////////
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH, 1.0f, NULL);
	deviceContext->PSSetShaderResources(0, 1, &flashlightShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	flashlight.Draw(deviceContext, flashlightObjectShaderBuffer, flashlightToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	//////////////////// ^ FLASHLIGHT DRAW ^ ///////////////////

	//////////////////// v MINIMAP v ///////////////////
	deviceContext->RSSetViewports(1, &miniViewport);
	cameraToScene.viewMatrix = XMMatrixInverse(NULL, miniViewMatrix);
	cameraToScene.projMatrix = miniProjectionMatrix;
	deviceContext->Map(cameraSceneShaderBuffer, NULL, D3D11_MAP_WRITE_DISCARD, NULL, &mapSubresource);
	memcpy_s(mapSubresource.pData, sizeof(SEND_TO_VRAM_SCENE), &cameraToScene, sizeof(SEND_TO_VRAM_SCENE));
	deviceContext->Unmap(cameraSceneShaderBuffer, NULL);
	deviceContext->VSSetConstantBuffers(1, 1, &cameraSceneShaderBuffer);

	// Skybox
	skyboxToObject.localMatrix.r[3].m128_f32[0] = miniViewMatrix.r[3].m128_f32[0];
	skyboxToObject.localMatrix.r[3].m128_f32[1] = miniViewMatrix.r[3].m128_f32[1];
	skyboxToObject.localMatrix.r[3].m128_f32[2] = miniViewMatrix.r[3].m128_f32[2];
	deviceContext->PSSetShaderResources(0, 1, &skyboxShaderResourceView);
	skybox.Draw(deviceContext, skyboxObjectShaderBuffer, skyboxToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	deviceContext->ClearDepthStencilView(depthStencilView, D3D11_CLEAR_DEPTH, 1.0f, NULL);
	// Stage
	deviceContext->PSSetShaderResources(0, 1, &stageShaderResourceView);
	stage.Draw(deviceContext, stageObjectShaderBuffer, stageToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 12, 0);
	// Star
	star.Draw(deviceContext, starObjectShaderBuffer, starToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 60, 0);
	// Rock
	deviceContext->PSSetShaderResources(0, 1, &rockShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	rock.Draw(deviceContext, rockObjectShaderBuffer, rockToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	// Ball Pit
	deviceContext->CSSetShader(ComputeShader, NULL, 0);
	for (int j = 0; j < 25; j++)
	{
		int i = j + indexOffset;
		if (i >= 25)
			i = i - 25;

		ballToObject.localMatrix.r[3].m128_f32[0] = particles[i].posX;
		ballToObject.localMatrix.r[3].m128_f32[1] = particles[i].posY;
		ballToObject.localMatrix.r[3].m128_f32[2] = particles[i].posZ;
		ball.Draw(deviceContext, ballObjectShaderBuffer, ballToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	}
	// Portals
	//deviceContext->OMSetDepthStencilState(shapeDepthStencilState, 1);
	//deviceContext->PSSetShaderResources(0, 1, &portal1ShaderResourceView);
	//portal1.Draw(deviceContext, portal1ObjectShaderBuffer, portal1ToObject, ToLightBuffer, ToLight, sizeof(VERTEX), 0, 150, 0);
	//deviceContext->PSSetShaderResources(0, 1, &portal2ShaderResourceView);
	//portal2.Draw(deviceContext, portal2ObjectShaderBuffer, portal2ToObject, ToLightBuffer, ToLight, sizeof(VERTEX), 0, 150, 0);
	// Wall
	//deviceContext->OMSetDepthStencilState(clipDepthStencilState, 1);
	deviceContext->PSSetShaderResources(0, 1, &wallShaderResourceView);
	wall.Draw(deviceContext, wallObjectShaderBuffer, wallToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 36, 0);
	//deviceContext->OMSetDepthStencilState(NULL, 0);
	// Flashlight
	deviceContext->PSSetShaderResources(0, 1, &flashlightShaderResourceView);
	deviceContext->RSSetState(FrontFaceCulling);
	flashlight.Draw(deviceContext, flashlightObjectShaderBuffer, flashlightToObject, ToLightBuffer, ToLight, nullptr, particle(), sizeof(VERTEX), 0, 10000, 0);
	deviceContext->RSSetState(MSAAEnabled);
	//////////////////// ^ MINIMAP ^ ///////////////////

	swapchain->Present(0, 0);

	return true; 
}

//************************************************************
//************ DESTRUCTION ***********************************
//************************************************************

bool DEMO_APP::ShutDown()
{
	if (device != nullptr)
		device->Release();
	if (deviceContext != nullptr)
		deviceContext->Release();
	if (swapchain != nullptr)
		swapchain->Release();
	if (renderTargetView != nullptr)
		renderTargetView->Release();
	if (skyboxObjectShaderBuffer != nullptr)
		skyboxObjectShaderBuffer->Release();
	if (skyboxShaderResourceView != nullptr)
		skyboxShaderResourceView->Release();
	if (stageObjectShaderBuffer != nullptr)
		stageObjectShaderBuffer->Release();
	if (stageTexture != nullptr)
		stageTexture->Release();
	if (stageShaderResourceView != nullptr)
		stageShaderResourceView->Release();
	if (wallObjectShaderBuffer != nullptr)
		wallObjectShaderBuffer->Release();
	if (wallShaderResourceView != nullptr)
		wallShaderResourceView->Release();
	if (portal1ObjectShaderBuffer != nullptr)
		portal1ObjectShaderBuffer->Release();
	if (portal1Texture != nullptr)
		portal1Texture->Release();
	if (portal1RenderTargetView != nullptr)
		portal1RenderTargetView->Release();
	if (portal1ShaderResourceView != nullptr)
		portal1ShaderResourceView->Release();
	if (portal2ObjectShaderBuffer != nullptr)
		portal2ObjectShaderBuffer->Release();
	if (portal2Texture != nullptr)
		portal2Texture->Release();
	if (portal2RenderTargetView != nullptr)
		portal2RenderTargetView->Release();
	if (portal2ShaderResourceView != nullptr)
		portal2ShaderResourceView->Release();
	if (shapeDepthStencilState != nullptr)
		shapeDepthStencilState->Release();
	if (drawDepthStencilState != nullptr)
		drawDepthStencilState->Release();
	if (clipDepthStencilState != nullptr)
		clipDepthStencilState->Release();
	if (starObjectShaderBuffer != nullptr)
		starObjectShaderBuffer->Release();
	if (rockObjectShaderBuffer != nullptr)
		rockObjectShaderBuffer->Release();
	if (rockShaderResourceView != nullptr)
		rockShaderResourceView->Release();
	if (flashlightObjectShaderBuffer != nullptr)
		flashlightObjectShaderBuffer->Release();
	if (flashlightShaderResourceView != nullptr)
		flashlightShaderResourceView->Release();
	if (ToLightBuffer != nullptr)
		ToLightBuffer->Release();
	if (cameraSceneShaderBuffer != nullptr)
		cameraSceneShaderBuffer->Release();
	if (ballObjectShaderBuffer != nullptr)
		ballObjectShaderBuffer->Release();
	if (structBuffer != nullptr)
		structBuffer->Release();
	if (ballShaderResourceView != nullptr)
		ballShaderResourceView->Release();
	if (ballUnorderedAccessView != nullptr)
		ballUnorderedAccessView->Release();
	if (ComputeShader != nullptr)
		ComputeShader->Release();
	if (depthStencil != nullptr)
		depthStencil->Release();
	if (depthStencilView != nullptr)
		depthStencilView->Release();
	if (samplerState != nullptr)
		samplerState->Release();
	if (MSAAEnabled != nullptr)
		MSAAEnabled->Release();
	if (FrontFaceCulling != nullptr)
		FrontFaceCulling->Release();

	UnregisterClass( L"DirectXApplication", application ); 
	return true;
}

//************************************************************
//************ WINDOWS RELATED *******************************
//************************************************************

// ****************** BEGIN WARNING ***********************//

int WINAPI wWinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPTSTR lpCmdLine,	int nCmdShow );						   
LRESULT CALLBACK WndProc(HWND hWnd,	UINT message, WPARAM wparam, LPARAM lparam );		
int WINAPI wWinMain( HINSTANCE hInstance, HINSTANCE, LPTSTR, int )
{
	srand(unsigned int(time(0)));
	DEMO_APP myApp(hInstance,(WNDPROC)WndProc);
	currentApp = &myApp;
    MSG msg; ZeroMemory( &msg, sizeof( msg ) );
    while ( msg.message != WM_QUIT && myApp.Run() )
    {	
	    if ( PeekMessage( &msg, NULL, 0, 0, PM_REMOVE ) )
        { 
            TranslateMessage( &msg );
            DispatchMessage( &msg ); 
        }
    }
	myApp.ShutDown(); 
	return 0; 
}
LRESULT CALLBACK WndProc( HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam )
{
    if(GetAsyncKeyState(VK_ESCAPE))
		message = WM_DESTROY;
    switch ( message )
    {
	case ( WM_DESTROY ):
	{
		PostQuitMessage( 0 );
	}
	break;
	case (WM_SIZE):
	{
		if (currentApp != nullptr)
			currentApp->Resize();
	}
	break;
    }
    return DefWindowProc( hWnd, message, wParam, lParam );
}
//********************* END WARNING ************************//