using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[AddComponentMenu("Path Tracing/PathTracingManager")]
public class PathTracingManager : MonoBehaviour
{
    public Light DirectionalLight;
    public int SphereSeed;
    public Vector2 SphereRadius = new Vector2(3.0f, 8.0f);
    public uint SpheresMax = 100;
    public float SpherePlacementRadius = 100.0f;

    private int m_kernelID;
    private uint m_currentSample = 0;
    private Camera m_camera;
    private RenderTexture m_target;
    private RenderTexture m_converged;
    private ComputeBuffer m_sphereBuffer;

    [SerializeField]
    // TODO: Is this redundant? (Two ComputeShader variables)
    private ComputeShader m_computeShader;
    public ComputeShader ComputeShader
    {
        get
        {
            if (null == _ComputeShader && null != m_computeShader)
            {
                _ComputeShader = Resources.Load<ComputeShader>("Compute Shaders/PathTracing");
                //_ComputeShader.hideFlags = HideFlags.HideAndDontSave;
            }

            return _ComputeShader;
        }
    }
    private ComputeShader _ComputeShader;

    [SerializeField]
    private Shader m_postProcessingShader;
    public Material PostProcessingMaterial
    {
        get
        {
            if (null == _PostProcessingMaterial && null != m_postProcessingShader)
            {
                _PostProcessingMaterial = new Material(m_postProcessingShader);
                //_PostProcessingMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _PostProcessingMaterial;
        }
    }
    private Material _PostProcessingMaterial;

    struct Sphere
    {
        public float radius;
        public float smoothness;
        public Vector3 position;
        public Vector3 albedo;
        public Vector3 specular;
        public Vector3 emission;
    };

    private void OnEnable()
    {
        m_currentSample = 0;
        SetUpScene();
    }

    private void OnDisable()
    {
        if (m_sphereBuffer != null)
            m_sphereBuffer.Release();
    }

    private void Awake()
    {
        // Save a reference to the camera
        m_camera = GetComponent<Camera>();

        // Find the id of our compute shader's kernel
        m_kernelID = ComputeShader.FindKernel("PathTracing");
    }

    private void Update()
    {
        if (true == transform.hasChanged)
        {
            transform.hasChanged = false;

            // Reset post processing sample
            m_currentSample = 0;
        }
        if (true == DirectionalLight.transform.hasChanged)

        {
            DirectionalLight.transform.hasChanged = false;

            // Reset post processing sample
            m_currentSample = 0;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Render(destination);
    }

    private void Render(RenderTexture destination)
    {
        // Initialize our target render texture
        InitRenderTexture();

        // Set compute shader data and dispatch
        SetComputeShaderParameters();
        ComputeShader.SetTexture(m_kernelID, "Result", m_target);
        int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
        ComputeShader.Dispatch(m_kernelID, threadGroupsX, threadGroupsY, 1);

        // Apply post processing
        PostProcessingMaterial.SetFloat("_Sample", m_currentSample);

        // Blit the result texture to the screen
        Graphics.Blit(m_target, m_converged, PostProcessingMaterial);
        Graphics.Blit(m_converged, destination);

        // Update post processing sample
        m_currentSample++;
    }

    private void InitRenderTexture()
    {
        // If a target render texture hasn't been set
        // OR If a converged render texture hasn't been set
        // OR If the screen's width does not match the target render texture's width
        // OR If the screen's height does not match the target render texture's height
        if (null == m_target || null == m_converged || Screen.width != m_target.width || Screen.height != m_target.height)
        {
            // If we already have a target render texture, but the width or height doesn't match
            if (null != m_target)
            {
                // Release the current target render texture
                m_target.Release();
            }
            // Create a new target render texture
            m_target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            m_target.enableRandomWrite = true;
            m_target.Create();

            // If we already have a converged render texture, but the width or height doesn't match
            if (null != m_converged)
            {
                // Release the current converged render texture
                m_converged.Release();
            }
            // Create a new converged render texture
            m_converged = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            m_converged.enableRandomWrite = true;
            m_converged.Create();

            // Reset post processing sample
            m_currentSample = 0;
        }
    }

    private void SetComputeShaderParameters()
    {
        ComputeShader.SetMatrix("_CameraToWorld", m_camera.cameraToWorldMatrix);
        ComputeShader.SetMatrix("_CameraInverseProjection", m_camera.projectionMatrix.inverse);

        // Pass in the skybox texture
        if (null != RenderSettings.skybox.mainTexture)
            ComputeShader.SetTexture(m_kernelID, "_SkyboxTexture", RenderSettings.skybox.mainTexture);

        ComputeShader.SetVector("_PixelOffset", new Vector2(Random.value, Random.value));

        Vector3 l = DirectionalLight.transform.forward;
        ComputeShader.SetVector("_DirectionalLight", new Vector4(l.x, l.y, l.z, DirectionalLight.intensity));

        ComputeShader.SetBuffer(m_kernelID, "_Spheres", m_sphereBuffer);

        ComputeShader.SetFloat("_Seed", Random.value);
    }

    private void SetUpScene()
    {
        Random.InitState(SphereSeed);

        List<Sphere> spheres = new List<Sphere>();
        // Add a number of random spheres
        for (int i = 0; i < SpheresMax; i++)
        {
            Sphere sphere = new Sphere();
            // Radius and radius
            sphere.radius = SphereRadius.x + Random.value * (SphereRadius.y - SphereRadius.x);
            Vector2 randomPos = Random.insideUnitCircle * SpherePlacementRadius;
            sphere.position = new Vector3(randomPos.x, sphere.radius, randomPos.y);
            // Reject spheres that are intersecting others
            foreach (Sphere other in spheres)
            {
                float minDist = sphere.radius + other.radius;
                if (Vector3.SqrMagnitude(sphere.position - other.position) < minDist * minDist)
                    goto SkipSphere;
            }

            // Albedo and specular color
            Color color = Random.ColorHSV();
            float chance = Random.value;
            if (chance < 0.8f)
            {
                bool metal = chance < 0.4f;
                sphere.albedo = metal ? Vector3.zero : new Vector3(color.r, color.g, color.b);
                sphere.specular = metal ? new Vector3(color.r, color.g, color.b) : Vector3.one * 0.04f;
                sphere.smoothness = Random.value;
            }
            else
            {
                Color emission = Random.ColorHSV(0.0f, 1.0f, 0.0f, 1.0f, 3.0f, 8.0f);
                sphere.albedo = Vector3.one;
                sphere.emission = new Vector3(emission.r, emission.g, emission.b);
            }

            // Add the sphere to the list
            spheres.Add(sphere);
            SkipSphere:
            continue;
        }
        // Assign to compute buffer
        m_sphereBuffer = new ComputeBuffer(spheres.Count, 56);
        m_sphereBuffer.SetData(spheres);
    }
}