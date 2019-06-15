using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Raymarch (Generic)")]
public class RaymarchSmoke : SceneViewFilter
{
    public Transform sunTransform;
    public Transform originTransform;
    public int maxStep = 80;
    public float drawDistance = 80.0f;
    public float distanceMargin = 0.001f;
    public float fixedStep = 0.05f;
    public int particleCount = 1000;
    public Transform _TopFrontRightBound = null;
    public Transform _BottomBackLeftBound = null;
    public int numVoxelsX = 2;
    public int numVoxelsY = 2;
    public int numVoxelsZ = 2;
    public bool useVoxels = true;

    [SerializeField]
    private Texture2D _ColorRamp;
    [SerializeField]
    private Shader _EffectShader;
    [SerializeField]
    public ComputeShader m_ComputeShader;

    private Vector3[] frustumCornersVec = new Vector3[4];
    private Matrix4x4 frustumCornersMat = Matrix4x4.identity;
    private int mComputeShaderKernelID;
    /// <summary>
	/// Number of particle per warp.
	/// </summary>
	private const int WARP_SIZE = 256;
    /// <summary>
	/// Number of warp needed.
	/// </summary>
	private int mWarpCount;
    private ComputeBuffer particleBuffer;
    private ComputeBuffer voxelBuffer;
    Particle[] particleArray;
    Voxel[] voxelArray;
    RenderTexture voxelTextures;
    RenderTexture shaderVoxelTextures;
    private int numVoxels = 0;

    public Material EffectMaterial
    {
        get
        {
            if (!_EffectMaterial && _EffectShader)
            {
                _EffectMaterial = new Material(_EffectShader);
                _EffectMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _EffectMaterial;
        }
    }
    private Material _EffectMaterial;

    public ComputeShader ComputeShader
    {
        get
        {
            if (!_ComputeShader && m_ComputeShader)
            {
                _ComputeShader = Resources.Load<ComputeShader>("Compute Shaders/Particles");
                _ComputeShader.hideFlags = HideFlags.HideAndDontSave;
            }

            return _ComputeShader;
        }
    }
    private ComputeShader _ComputeShader;

    public Camera CurrentCamera
    {
        get
        {
            if (!_CurrentCamera)
                _CurrentCamera = GetComponent<Camera>();
            return _CurrentCamera;
        }
    }
    private Camera _CurrentCamera;

    /// <summary>
	/// Particle data structure used by the shader and the compute shader.
	/// </summary>
	private struct Particle
    {
        public Vector3 position;
        public Vector3 velocity;
    }

    private struct Voxel
    {
        public int numParticles;
        public Vector3 positiveBound;
        public Vector3 negativeBound;
    }

    private void Start()
    {
        // Calculate the number of warp needed to handle all the particles
        if (particleCount <= 0)
        {
            particleCount = 1;
        }
        mWarpCount = Mathf.CeilToInt((float)particleCount / WARP_SIZE);

        // Initialize the Particles on Awake
        particleArray = new Particle[particleCount];
        for (int i = 0; i < particleCount; ++i)
        {
            particleArray[i].position.x = originTransform.position.x + (Random.value * 2 - 1.0f);
            particleArray[i].position.y = originTransform.position.y + (Random.value * 2 - 1.0f);
            particleArray[i].position.z = originTransform.position.z + (Random.value * 2 - 1.0f);

            particleArray[i].velocity.x = 0;
            particleArray[i].velocity.y = 0;
            particleArray[i].velocity.z = 0;
        }

        // Create the ComputeBuffer holding the Particles
        particleBuffer = new ComputeBuffer(particleCount, /*sizeof(Particle)*/ 24);
        particleBuffer.SetData(particleArray);

        // Find the id of the kernel
        mComputeShaderKernelID = ComputeShader.FindKernel("Particles");

        // Bind the ComputeBuffer to the shader and the compute shader
        ComputeShader.SetBuffer(mComputeShaderKernelID, "particleBuffer", particleBuffer);
        //material.SetBuffer("particleBuffer", particleBuffer);
        EffectMaterial.SetBuffer("particleBuffer", particleBuffer);

        numVoxels = numVoxelsX * numVoxelsY * numVoxelsZ;
        voxelArray = new Voxel[numVoxels];

        float stepX = (_TopFrontRightBound.position.x - _BottomBackLeftBound.position.x) / numVoxelsX;
        float stepY = (_TopFrontRightBound.position.y - _BottomBackLeftBound.position.y) / numVoxelsY;
        float stepZ = (_TopFrontRightBound.position.z - _BottomBackLeftBound.position.z) / numVoxelsZ;
        Vector3 currPositive = _TopFrontRightBound.position;
        Vector3 currNegative = new Vector3(_TopFrontRightBound.position.x - stepX, _TopFrontRightBound.position.y - stepY, _TopFrontRightBound.position.z - stepZ);
        int currIndex = 0;
        for (int i = 0; i < numVoxelsZ; i++)
        {
            for (int j = 0; j < numVoxelsY; j++)
            {
                for (int k = 0; k < numVoxelsX; k++)
                {
                    voxelArray[currIndex].positiveBound = currPositive;
                    voxelArray[currIndex].negativeBound = currNegative;
                    //voxelArray[currIndex].particles = new Vector3[168];
                    //voxelArray[currIndex].particles = new Texture2D(1, particleCount, TextureFormat.RGBAFloat, false);
                    voxelArray[currIndex].numParticles = 0;

                    currPositive.x -= stepX;
                    currNegative.x -= stepX;

                    currIndex++;
                }
                currPositive.y -= stepY;
                currNegative.y -= stepY;
                currPositive.x = _TopFrontRightBound.position.x;
                currNegative.x = _TopFrontRightBound.position.x - stepX;
            }
            currPositive.z -= stepZ;
            currNegative.z -= stepZ;
            currPositive.y = _TopFrontRightBound.position.y;
            currNegative.y = _TopFrontRightBound.position.y - stepY;
        }

        voxelBuffer = new ComputeBuffer(numVoxels, /*sizeof(Voxel)*/ (28 /*+ (12 * 168)*/));
        voxelBuffer.SetData(voxelArray);

        ComputeShader.SetBuffer(mComputeShaderKernelID, "voxelBuffer", voxelBuffer);
        EffectMaterial.SetBuffer("voxelBuffer", voxelBuffer);

        ComputeShader.SetInt("numVoxles", numVoxels);
        EffectMaterial.SetInt("_NumVoxels", numVoxels);

        //voxelTextures = new Texture2DArray(1, particleCount, numVoxels, TextureFormat.RGBAFloat, false);
        //voxelTextures = new Texture2DArray(1, particleCount, numVoxels, GraphicsFormat.R32G32B32_SFloat, TextureCreationFlags.None);
        voxelTextures = new RenderTexture(1, particleCount, 32, RenderTextureFormat.ARGBFloat);
        voxelTextures.dimension = TextureDimension.Tex2DArray;
        voxelTextures.enableRandomWrite = true;
        voxelTextures.volumeDepth = numVoxels;
        voxelTextures.Create();
        ComputeShader.SetTexture(mComputeShaderKernelID, "voxelTextures", voxelTextures);
        EffectMaterial.SetTexture("_VoxelTextures", voxelTextures);

        EndOfFrame();
    }

    private void OnDestroy()
    {
        if (null != particleBuffer)
        {
            particleBuffer.Release();
        }
        if (null != voxelBuffer)
        {
            voxelBuffer.Release();
        }
    }

    private void OnDisable()
    {
        if (null != particleBuffer)
        {
            particleBuffer.Release();
        }
        if (null != voxelBuffer)
        {
            voxelBuffer.Release();
        }
    }

    private void Update()
    {
        // Send datas to the compute shader
        ComputeShader.SetFloat("deltaTime", Time.deltaTime);
        ComputeShader.SetFloats("mousePosition", new float[] { originTransform.position.x, originTransform.position.y, originTransform.position.z });
        
        // Update the Particles
        ComputeShader.Dispatch(mComputeShaderKernelID, mWarpCount, 1, 1);
    }

    IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < numVoxels; i++)
        {
            voxelArray[i].numParticles = 0;
        }
        voxelBuffer.SetData(voxelArray);
        ComputeShader.SetBuffer(mComputeShaderKernelID, "voxelBuffer", voxelBuffer);

        //EffectMaterial.SetTexture("_VoxelTextures", voxelTextures);
    }

    /// \brief Stores the normalized rays representing the camera frustum in a 4x4 matrix.  Each row is a vector.
    /// 
    /// The following rays are stored in each row (in eyespace, not worldspace):
    /// Top Left corner:     row=0
    /// Top Right corner:    row=1
    /// Bottom Right corner: row=2
    /// Bottom Left corner:  row=3
    private Matrix4x4 GetFrustumCorners(Camera cam)
    {
        float camFov = cam.fieldOfView;
        float camAspect = cam.aspect;

        Matrix4x4 frustumCorners = Matrix4x4.identity;

        float fovWHalf = camFov * 0.5f;

        float tan_fov = Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 toRight = Vector3.right * tan_fov * camAspect;
        Vector3 toTop = Vector3.up * tan_fov;

        Vector3 topLeft = (-Vector3.forward - toRight + toTop);
        Vector3 topRight = (-Vector3.forward + toRight + toTop);
        Vector3 bottomRight = (-Vector3.forward + toRight - toTop);
        Vector3 bottomLeft = (-Vector3.forward - toRight - toTop);

        frustumCorners.SetRow(0, topLeft);
        frustumCorners.SetRow(1, topRight);
        frustumCorners.SetRow(2, bottomRight);
        frustumCorners.SetRow(3, bottomLeft);

        return frustumCorners;
    }

    /// \brief Custom version of Graphics.Blit that encodes frustum corner indices into the input vertices.
    /// 
    /// In a shader you can expect the following frustum cornder index information to get passed to the z coordinate:
    /// Top Left vertex:     z=0, u=0, v=0
    /// Top Right vertex:    z=1, u=1, v=0
    /// Bottom Right vertex: z=2, u=1, v=1
    /// Bottom Left vertex:  z=3, u=1, v=0
    /// 
    /// \warning You may need to account for flipped UVs on DirectX machines due to differing UV semantics
    ///          between OpenGL and DirectX.  Use the shader define UNITY_UV_STARTS_AT_TOP to account for this.
    static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
    {
        RenderTexture.active = dest;

        fxMaterial.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho(); // Note: z value of vertices don't make a difference because we are using ortho projection

        fxMaterial.SetPass(passNr);

        GL.Begin(GL.QUADS);

        // Here, GL.MultitexCoord2(0, x, y) assigns the value (x, y) to the TEXCOORD0 slot in the shader.
        // GL.Vertex3(x,y,z) queues up a vertex at position (x, y, z) to be drawn.  Note that we are storing
        // our own custom frustum information in the z coordinate.
        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

        GL.End();
        GL.PopMatrix();
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!EffectMaterial)
        {
            Graphics.Blit(source, destination); // do nothing
            return;
        }

        // pass frustum rays to shader
        if (CurrentCamera.stereoActiveEye != Camera.MonoOrStereoscopicEye.Mono)
        {
            Matrix4x4 invViewMat = CurrentCamera.GetStereoViewMatrix((Camera.StereoscopicEye)CurrentCamera.stereoActiveEye).inverse;

            frustumCornersVec = new Vector3[4];
            frustumCornersMat = Matrix4x4.identity;
            CurrentCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), CurrentCamera.farClipPlane, CurrentCamera.stereoActiveEye, frustumCornersVec);
            for (int i = 0; i < 4; i++)
            {
                frustumCornersVec[i].z *= -1;
            }
            frustumCornersMat.SetRow(0, frustumCornersVec[1]);
            frustumCornersMat.SetRow(1, frustumCornersVec[2]);
            frustumCornersMat.SetRow(2, frustumCornersVec[3]);
            frustumCornersMat.SetRow(3, frustumCornersVec[0]);

            EffectMaterial.SetMatrix("_FrustumCornersES", frustumCornersMat);
            EffectMaterial.SetMatrix("_CameraInvViewMatrix", invViewMat);
            EffectMaterial.SetVector("_CameraWS", new Vector4(invViewMat.m03, invViewMat.m13, invViewMat.m23, 1));
        }
        else
        {
            EffectMaterial.SetMatrix("_FrustumCornersES", GetFrustumCorners(CurrentCamera));
            EffectMaterial.SetMatrix("_CameraInvViewMatrix", CurrentCamera.cameraToWorldMatrix); // world matrix is the inverse of view matrix
            EffectMaterial.SetVector("_CameraWS", CurrentCamera.transform.position);
        }

        EffectMaterial.SetVector("_LightDir", sunTransform ? sunTransform.forward : Vector3.down);

        EffectMaterial.SetTexture("_ColorRamp", _ColorRamp);

        // Update the Particles
        //ComputeShader.Dispatch(mComputeShaderKernelID, mWarpCount, 1, 1);
        EffectMaterial.SetMatrix("_OriginInvMatrix", originTransform ? originTransform.localToWorldMatrix.inverse : Matrix4x4.identity.inverse);
        //EffectMaterial.SetBuffer("particleBuffer", particleBuffer);

        EffectMaterial.SetInt("_MaxStep", maxStep);
        EffectMaterial.SetFloat("_DrawDistance", drawDistance);
        EffectMaterial.SetFloat("_DistanceMargin", distanceMargin);
        EffectMaterial.SetFloat("_FixedStep", fixedStep);
        EffectMaterial.SetInt("_ParticleCount", particleCount);
        EffectMaterial.SetInt("_UseVoxels", (useVoxels) ? 1 : -1);

        //Graphics.Blit(source, destination, EffectMaterial, 0); // use given effect shader as image effect
        CustomGraphicsBlit(source, destination, EffectMaterial, 0); // Replace Graphics.Blit with CustomGraphicsBlit
    }

    private void OnRenderObject()
    {
        //Graphics.DrawProcedural(MeshTopology.Points, 1, particleCount);
        //Graphics.DrawProcedural(MeshTopology.Lines,)

        //float stepX = (_TopFrontRightBound.position.x - _BottomBackRightBound.position.x) / numVoxelsX;
        //float stepY = (_TopFrontRightBound.position.y - _BottomBackRightBound.position.y) / numVoxelsY;
        //float stepZ = (_TopFrontRightBound.position.z - _BottomBackRightBound.position.z) / numVoxelsZ;
        //
        //Vector3 currPositive = _TopFrontRightBound.position;
        //Vector3 currNegative = new Vector3(_TopFrontRightBound.position.x - stepX, _TopFrontRightBound.position.y - stepY, _TopFrontRightBound.position.z - stepZ);
        //
        //for (int i = 0; i < numVoxelsZ; i++)
        //{
        //    for (int j = 0; j < numVoxelsY; j++)
        //    {
        //        for (int k = 0; k < numVoxelsX; k++)
        //        {
        //            Debug.DrawLine(currPositive, new Vector3(currNegative.x, currPositive.y, currPositive.z), Color.blue);
        //            Debug.DrawLine(currPositive, new Vector3(currPositive.x, currNegative.y, currPositive.z), Color.blue);
        //            Debug.DrawLine(currPositive, new Vector3(currPositive.x, currPositive.y, currNegative.z), Color.blue);
        //            Debug.DrawLine(currNegative, new Vector3(currPositive.x, currNegative.y, currNegative.z), Color.blue);
        //            Debug.DrawLine(currNegative, new Vector3(currNegative.x, currPositive.y, currNegative.z), Color.blue);
        //            Debug.DrawLine(currNegative, new Vector3(currNegative.x, currNegative.y, currPositive.z), Color.blue);
        //            Debug.DrawLine(new Vector3(currPositive.x, currNegative.y, currPositive.z),
        //                new Vector3(currNegative.x, currNegative.y, currPositive.z),
        //                Color.blue);
        //            Debug.DrawLine(new Vector3(currPositive.x, currNegative.y, currPositive.z),
        //                new Vector3(currPositive.x, currNegative.y, currNegative.z),
        //                Color.blue);
        //            Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currNegative.z),
        //                new Vector3(currPositive.x, currPositive.y, currNegative.z),
        //                Color.blue);
        //            Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currNegative.z),
        //                new Vector3(currNegative.x, currPositive.y, currPositive.z),
        //                Color.blue);
        //            Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currPositive.z),
        //                new Vector3(currNegative.x, currNegative.y, currPositive.z),
        //                Color.blue);
        //            Debug.DrawLine(new Vector3(currPositive.x, currPositive.y, currNegative.z),
        //                new Vector3(currPositive.x, currNegative.y, currNegative.z),
        //                Color.blue);
        //
        //            currPositive.x -= stepX;
        //            currNegative.x -= stepX;
        //        }
        //        currPositive.y -= stepY;
        //        currNegative.y -= stepY;
        //        currPositive.x = _TopFrontRightBound.position.x;
        //        currNegative.x = _TopFrontRightBound.position.x - stepX;
        //    }
        //    currPositive.z -= stepZ;
        //    currNegative.z -= stepZ;
        //    currPositive.y = _TopFrontRightBound.position.y;
        //    currNegative.y = _TopFrontRightBound.position.y - stepY;
        //}

        if (null != voxelArray)
        {
            for (int i = 0; i < numVoxels; i++)
            {
                Vector3 currPositive = voxelArray[i].positiveBound;
                Vector3 currNegative = voxelArray[i].negativeBound;

                Debug.DrawLine(currPositive, new Vector3(currNegative.x, currPositive.y, currPositive.z), Color.blue);
                Debug.DrawLine(currPositive, new Vector3(currPositive.x, currNegative.y, currPositive.z), Color.blue);
                Debug.DrawLine(currPositive, new Vector3(currPositive.x, currPositive.y, currNegative.z), Color.blue);
                Debug.DrawLine(currNegative, new Vector3(currPositive.x, currNegative.y, currNegative.z), Color.blue);
                Debug.DrawLine(currNegative, new Vector3(currNegative.x, currPositive.y, currNegative.z), Color.blue);
                Debug.DrawLine(currNegative, new Vector3(currNegative.x, currNegative.y, currPositive.z), Color.blue);
                Debug.DrawLine(new Vector3(currPositive.x, currNegative.y, currPositive.z),
                    new Vector3(currNegative.x, currNegative.y, currPositive.z),
                    Color.blue);
                Debug.DrawLine(new Vector3(currPositive.x, currNegative.y, currPositive.z),
                    new Vector3(currPositive.x, currNegative.y, currNegative.z),
                    Color.blue);
                Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currNegative.z),
                    new Vector3(currPositive.x, currPositive.y, currNegative.z),
                    Color.blue);
                Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currNegative.z),
                    new Vector3(currNegative.x, currPositive.y, currPositive.z),
                    Color.blue);
                Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currPositive.z),
                    new Vector3(currNegative.x, currNegative.y, currPositive.z),
                    Color.blue);
                Debug.DrawLine(new Vector3(currPositive.x, currPositive.y, currNegative.z),
                    new Vector3(currPositive.x, currNegative.y, currNegative.z),
                    Color.blue);
            }
        }
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    //private void Update()
    //{
    //    var worldSpaceCorner = CurrentCamera.transform.TransformVector(frustumCornersVec[0]);
    //    Debug.DrawRay(CurrentCamera.transform.position, worldSpaceCorner, Color.red);
    //}
}