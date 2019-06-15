using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FluidSim : MonoBehaviour
{
    /// <summary>
	/// Particle data structure used by the shader and the compute shader.
	/// </summary>
	private struct Particle
    {
        public float density;
        public float temperature;
        public Vector3 position;
        public Vector3 velocity;
    }

    private struct Voxel
    {
        public int numParticles;
        public Vector3 positiveBound;
        public Vector3 negativeBound;
    }

    public ComputeShader ComputeShader
    {
        get
        {
            if (!_ComputeShader && m_ComputeShader)
            {
                _ComputeShader = Resources.Load<ComputeShader>("Compute Shaders/FluidSim");
                _ComputeShader.hideFlags = HideFlags.HideAndDontSave;
            }

            return _ComputeShader;
        }
    }
    private ComputeShader _ComputeShader;

    public bool showBounds = true;
    public bool showVoxels = true;
    public int particleCount = 1000;
    public int numVoxelsX = 20;
    public int numVoxelsY = 20;
    public int numVoxelsZ = 20;
    public float voxelRes = 0.1f;
    public Transform boundOrigin = null;

    [SerializeField]
    private ComputeShader m_ComputeShader;

    /// <summary>
	/// Number of particle per warp.
	/// </summary>
	private const int WARP_SIZE = 1;
    /// <summary>
	/// Number of warp needed.
	/// </summary>
	private int mWarpCount;
    private int mComputeShaderKernelID;
    private int numVoxels = 0;
    private ComputeBuffer particleBuffer;
    private ComputeBuffer voxelBuffer;
    Particle[] particleArray;
    Voxel[,,] voxelArray;

    private void Awake()
    {
        // Calculate the number of warp needed to handle all the particles
        if (particleCount <= 0)
        {
            particleCount = 1;
        }
        mWarpCount = Mathf.CeilToInt((float)particleCount / WARP_SIZE);

        // Initialize the array of particles
        particleArray = new Particle[particleCount];
        for (int i = 0; i < particleCount; ++i)
        {
            particleArray[i].position.x = boundOrigin.position.x + (Random.value * 2.0f - 1.0f) * (numVoxelsX / 2 * voxelRes);
            particleArray[i].position.y = boundOrigin.position.y + (Random.value * 2.0f - 1.0f) * (numVoxelsY / 2 * voxelRes);
            particleArray[i].position.z = boundOrigin.position.z + (Random.value * 2.0f - 1.0f) * (numVoxelsZ / 2 * voxelRes);

            particleArray[i].velocity.x = 0;
            particleArray[i].velocity.y = 0;
            particleArray[i].velocity.z = 0;
        }

        // Create the ComputeBuffer holding the Particles
        particleBuffer = new ComputeBuffer(particleCount, /*sizeof(Particle)*/ 32);
        particleBuffer.SetData(particleArray);

        // Find the id of the kernel
        mComputeShaderKernelID = ComputeShader.FindKernel("FluidSim");

        // Bind the ComputeBuffer to the shader and the compute shader
        ComputeShader.SetBuffer(mComputeShaderKernelID, "particleBuffer", particleBuffer);

        numVoxels = numVoxelsX * numVoxelsY * numVoxelsZ;
        voxelArray = new Voxel[numVoxelsX, numVoxelsY, numVoxelsZ];

        // Initialize the array of voxels
        Vector3 initPosBound, currPosBound;
        initPosBound.x = currPosBound.x = boundOrigin.position.x + ((numVoxelsX / 2) * voxelRes) + ((numVoxelsX % 2 == 0) ? 0 : voxelRes * 0.5f);
        initPosBound.y = currPosBound.y = boundOrigin.position.y + ((numVoxelsY / 2) * voxelRes) + ((numVoxelsY % 2 == 0) ? 0 : voxelRes * 0.5f);
        initPosBound.z = currPosBound.z = boundOrigin.position.z + ((numVoxelsZ / 2) * voxelRes) + ((numVoxelsZ % 2 == 0) ? 0 : voxelRes * 0.5f);
        Vector3 initNegBound, currNegBound;
        initNegBound.x = currNegBound.x = initPosBound.x - voxelRes;
        initNegBound.y = currNegBound.y = initPosBound.y - voxelRes;
        initNegBound.z = currNegBound.z = initPosBound.z - voxelRes;
        for (int i = 0; i < numVoxelsZ; i++)
        {
            for (int j = 0; j < numVoxelsY; j++)
            {
                for (int k = 0; k < numVoxelsX; k++)
                {
                    voxelArray[k, j, i].positiveBound = currPosBound;
                    voxelArray[k, j, i].negativeBound = currNegBound;
                    //voxelArray[k, j, i].particles = new Vector3[168];
                    //voxelArray[k, j, i].particles = new Texture2D(1, particleCount, TextureFormat.RGBAFloat, false);
                    voxelArray[k, j, i].numParticles = 0;
        
                    currPosBound.x -= voxelRes;
                    currNegBound.x -= voxelRes;
                }
                currPosBound.y -= voxelRes;
                currNegBound.y -= voxelRes;
                currPosBound.x = initPosBound.x;
                currNegBound.x = initPosBound.x - voxelRes;
            }
            currPosBound.z -= voxelRes;
            currNegBound.z -= voxelRes;
            currPosBound.y = initPosBound.y;
            currNegBound.y = initPosBound.y - voxelRes;
        }

        //// Sort particles into the voxels by position
        //int indexX, indexY, indexZ;
        //float diffX, diffY, diffZ;
        //for (int i = 0; i < particleCount; ++i)
        //{
        //    diffX = Mathf.Abs(particleArray[i].position.x - initPosBound.x);
        //    indexX = (int)(diffX / voxelRes);
        //    diffY = Mathf.Abs(particleArray[i].position.y - initPosBound.y);
        //    indexY = (int)(diffY / voxelRes);
        //    diffZ = Mathf.Abs(particleArray[i].position.z - initPosBound.z);
        //    indexZ = (int)(diffZ / voxelRes);
        //
        //    voxelArray[indexX, indexY, indexZ].numParticles++;
        //}
        
        voxelBuffer = new ComputeBuffer(numVoxels, /*sizeof(Voxel)*/ (28 /*+ (12 * 168)*/));
        voxelBuffer.SetData(voxelArray);

        ComputeShader.SetInt("numVoxels", numVoxels);
        ComputeShader.SetInt("numVoxelsX", numVoxelsX);
        ComputeShader.SetInt("numVoxelsY", numVoxelsY);
        ComputeShader.SetInt("numVoxelsZ", numVoxelsZ);
        ComputeShader.SetFloat("voxelSize", voxelRes);
        ComputeShader.SetFloats("positiveBound", new float[] { initPosBound.x, initPosBound.y, initPosBound.z });
        ComputeShader.SetBuffer(mComputeShaderKernelID, "voxelBuffer", voxelBuffer);
    }

    private void Update()
    {
        for (int i = 0; i < numVoxelsZ; i++)
        {
            for (int j = 0; j < numVoxelsY; j++)
            {
                for (int k = 0; k < numVoxelsX; k++)
                {
                    voxelArray[k, j, i].numParticles = 0;
                }
            }
        }
        voxelBuffer.SetData(voxelArray);
        //ComputeShader.SetBuffer(mComputeShaderKernelID, "voxelBuffer", voxelBuffer);

        // Send data to the compute shader
        ComputeShader.SetFloat("deltaTime", Time.deltaTime);
        ComputeShader.SetFloats("mousePosition", new float[] { boundOrigin.position.x, boundOrigin.position.y, boundOrigin.position.z });

        // Update the Particles
        ComputeShader.Dispatch(mComputeShaderKernelID, mWarpCount, 1, 1);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            particleBuffer.GetData(particleArray);
            for (int i = 0; i < particleCount; i++)
            {
                Gizmos.DrawSphere(particleArray[i].position, 0.02f);
            }
        }

        if (showVoxels && Application.isPlaying) // Draw the voxels
        {
            voxelBuffer.GetData(voxelArray);
            for (int i = 0; i < numVoxelsZ; i++)
            {
                for (int j = 0; j < numVoxelsY; j++)
                {
                    for (int k = 0; k < numVoxelsX; k++)
                    {
                        if (voxelArray[k, j, i].numParticles > 0)
                        {
                            Gizmos.DrawWireCube(new Vector3(voxelArray[k, j, i].positiveBound.x - voxelRes / 2,
                                voxelArray[k, j, i].positiveBound.y - voxelRes / 2,
                                voxelArray[k, j, i].positiveBound.z - voxelRes / 2),
                                new Vector3(voxelRes, voxelRes, voxelRes));
                        }
                    }
                }
            }
        }

        if (showBounds) // Draw the bounding box
            Gizmos.DrawWireCube(boundOrigin.position, new Vector3(numVoxelsX * voxelRes, numVoxelsY * voxelRes, numVoxelsZ * voxelRes));
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

        //if (null != voxelArray)
        //{
        //    for (int i = 0; i < numVoxels; i++)
        //    {
        //        Vector3 currPositive = voxelArray[i].positiveBound;
        //        Vector3 currNegative = voxelArray[i].negativeBound;
        //
        //        Debug.DrawLine(currPositive, new Vector3(currNegative.x, currPositive.y, currPositive.z), Color.blue);
        //        Debug.DrawLine(currPositive, new Vector3(currPositive.x, currNegative.y, currPositive.z), Color.blue);
        //        Debug.DrawLine(currPositive, new Vector3(currPositive.x, currPositive.y, currNegative.z), Color.blue);
        //        Debug.DrawLine(currNegative, new Vector3(currPositive.x, currNegative.y, currNegative.z), Color.blue);
        //        Debug.DrawLine(currNegative, new Vector3(currNegative.x, currPositive.y, currNegative.z), Color.blue);
        //        Debug.DrawLine(currNegative, new Vector3(currNegative.x, currNegative.y, currPositive.z), Color.blue);
        //        Debug.DrawLine(new Vector3(currPositive.x, currNegative.y, currPositive.z),
        //            new Vector3(currNegative.x, currNegative.y, currPositive.z),
        //            Color.blue);
        //        Debug.DrawLine(new Vector3(currPositive.x, currNegative.y, currPositive.z),
        //            new Vector3(currPositive.x, currNegative.y, currNegative.z),
        //            Color.blue);
        //        Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currNegative.z),
        //            new Vector3(currPositive.x, currPositive.y, currNegative.z),
        //            Color.blue);
        //        Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currNegative.z),
        //            new Vector3(currNegative.x, currPositive.y, currPositive.z),
        //            Color.blue);
        //        Debug.DrawLine(new Vector3(currNegative.x, currPositive.y, currPositive.z),
        //            new Vector3(currNegative.x, currNegative.y, currPositive.z),
        //            Color.blue);
        //        Debug.DrawLine(new Vector3(currPositive.x, currPositive.y, currNegative.z),
        //            new Vector3(currPositive.x, currNegative.y, currNegative.z),
        //            Color.blue);
        //    }
        //}
    }
}