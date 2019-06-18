using System.Collections.Generic;
using System.Linq;
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
    public bool PassesStartLocked = false;
    // Value of 0 means unlimited
    public uint MaxPasses = 1;

    private bool m_passLocked = false;
    private int m_kernelID;
    private uint m_numPasses = 0;
    private uint m_currentSample = 0;
    private float m_renderTime = 0.0f;
    private string m_passLockText;
    private Rect m_buttonNumPassRect;
    private Rect m_buttonPassLockRect;
    private Rect m_labelMaxPassRect;
    private Rect m_fieldMaxPassRect;
    private Rect m_labelTimerRect;
    private Camera m_camera;
    private RenderTexture m_target;
    private RenderTexture m_converged;
    private ComputeBuffer m_sphereBuffer;
    private ComputeBuffer m_meshObjectBuffer;
    private ComputeBuffer m_vertexBuffer;
    private ComputeBuffer m_indexBuffer;
    private ComputeBuffer m_normalBuffer;

    private static bool m_meshObjectsNeedRebuilding = false;
    private static List<PathTracingObject> m_pathTracingObjects = new List<PathTracingObject>();
    private static List<MeshObject> m_meshObjects = new List<MeshObject>();
    private static List<Vector3> m_vertices = new List<Vector3>();
    private static List<int> m_indices = new List<int>();
    private static List<Vector3> m_normals = new List<Vector3>();

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

    struct MeshObject
    {
        public Matrix4x4 localToWorldMatrix;
        public int indices_offset;
        public int indices_count;
        public float smoothness;
        public Vector3 albedo;
        public Vector3 specular;
        public Vector3 emission;
    }

    private void Awake()
    {
        // Save a reference to the camera
        m_camera = GetComponent<Camera>();

        // Find the id of our compute shader's kernel
        m_kernelID = ComputeShader.FindKernel("PathTracing");

        float buffer = 5.0f;
        m_buttonPassLockRect = new Rect(10, 10, 150, 25);
        m_buttonNumPassRect = new Rect(m_buttonPassLockRect.x, m_buttonPassLockRect.yMax + buffer, m_buttonPassLockRect.width, m_buttonPassLockRect.height);
        m_labelMaxPassRect = new Rect(m_buttonPassLockRect.x, m_buttonNumPassRect.yMax + buffer, 77, m_buttonPassLockRect.height);
        m_fieldMaxPassRect = new Rect(m_labelMaxPassRect.xMax + buffer, m_labelMaxPassRect.y,
            m_buttonPassLockRect.width - m_labelMaxPassRect.width - buffer, m_buttonPassLockRect.height);
        m_labelTimerRect = new Rect(m_buttonPassLockRect.x, m_fieldMaxPassRect.yMax + buffer, m_buttonPassLockRect.width, m_buttonPassLockRect.height);
    }

    private void OnEnable()
    {
        m_currentSample = 0;
        m_numPasses = 0;
        m_renderTime = 0.0f;
        m_meshObjectsNeedRebuilding = true;
        SetUpPassLock(PassesStartLocked);
        SetUpScene();
    }

    private void OnDisable()
    {
        if (m_sphereBuffer != null)
            m_sphereBuffer.Release();
        if (m_meshObjectBuffer != null)
            m_meshObjectBuffer.Release();
        if (m_vertexBuffer != null)
            m_vertexBuffer.Release();
        if (m_indexBuffer != null)
            m_indexBuffer.Release();
        if (m_normalBuffer != null)
            m_normalBuffer.Release();
    }

    private void Update()
    {
        if (true == transform.hasChanged)
        {
            transform.hasChanged = false;

            // Reset post processing sample
            m_currentSample = 0;

            // Reset number of passes
            m_numPasses = 0;

            // Reset render timer
            m_renderTime = 0.0f;
        }
        if (true == DirectionalLight.transform.hasChanged)
        {
            DirectionalLight.transform.hasChanged = false;

            // Reset post processing sample
            m_currentSample = 0;

            // Reset number of passes
            m_numPasses = 0;

            // Reset render timer
            m_renderTime = 0.0f;
        }
        if (true == m_meshObjectsNeedRebuilding)
        {
            // Reset post processing sample
            m_currentSample = 0;

            // Reset number of passes
            m_numPasses = 0;

            // Reset render timer
            m_renderTime = 0.0f;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_numPasses < MaxPasses || false == m_passLocked)
        {
            RebuildMeshObjectBuffers();

            Render(destination);
        }
        else
            Graphics.Blit(m_converged, destination);
    }

    private void OnGUI()
    {
        if (GUI.Button(m_buttonPassLockRect, m_passLockText))
        {
            TogglePassLock();
        }

        // Enable the next GUI elements only if passes are locked and the current number of passes has reached the defined maximum
        GUI.enabled = m_passLocked && m_numPasses == MaxPasses;

        if (GUI.Button(m_buttonNumPassRect, "Passes: " + m_numPasses) && true == m_passLocked)
        {
            MaxPasses++;
        }

        // Enable all GUI elements again after previous check
        GUI.enabled = true;

        GUI.Label(m_labelMaxPassRect, "Max Passes: ");
        MaxPasses = uint.Parse(GUI.TextField(m_fieldMaxPassRect, MaxPasses.ToString()));

        GUI.Label(m_labelTimerRect, "Time: " + (int)(m_renderTime * 1000) + "ms (" + ((int)(1/m_renderTime)) + " fps)");
    }

    public static void RegisterObject(PathTracingObject obj)
    {
        m_pathTracingObjects.Add(obj);
        m_meshObjectsNeedRebuilding = true;
    }

    public static void UnregisterObject(PathTracingObject obj)
    {
        m_pathTracingObjects.Remove(obj);
        m_meshObjectsNeedRebuilding = true;
    }

    public static void RebuildObjects()
    {
        m_meshObjectsNeedRebuilding = true;
    }

    private void SetUpPassLock(bool _startLocked)
    {
        m_passLocked = _startLocked;

        if (true == _startLocked)
        {
            m_passLockText = "Unlock";
        }
        else
        {
            m_passLockText = "Lock";
        }
    }

    private void TogglePassLock()
    {
        m_passLocked = !m_passLocked;

        if (true == m_passLocked)
        {
            m_passLockText = "Unlock";
        }
        else
        {
            m_passLockText = "Lock";
        }
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

        // Update number of passes
        m_numPasses++;

        // Update render timer
        m_renderTime += Time.deltaTime;
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

        SetComputeBuffer("_Spheres", m_sphereBuffer);
        SetComputeBuffer("_MeshObjects", m_meshObjectBuffer);
        SetComputeBuffer("_Vertices", m_vertexBuffer);
        SetComputeBuffer("_Indices", m_indexBuffer);
        SetComputeBuffer("_Normals", m_normalBuffer);
    }

    private void RebuildMeshObjectBuffers()
    {
        if (!m_meshObjectsNeedRebuilding)
        {
            return;
        }

        m_meshObjectsNeedRebuilding = false;
        m_currentSample = 0;

        // Clear all lists
        m_meshObjects.Clear();
        m_vertices.Clear();
        m_indices.Clear();
        m_normals.Clear();

        // Loop over all objects and gather their data
        foreach (PathTracingObject obj in m_pathTracingObjects)
        {
            Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;

            // Add vertex data
            int firstVertex = m_vertices.Count;
            m_vertices.AddRange(mesh.vertices);

            // Add index data - if the vertex buffer wasn't empty before, the
            // indices need to be offset
            int firstIndex = m_indices.Count;
            var indices = mesh.GetIndices(0);
            m_indices.AddRange(indices.Select(index => index + firstVertex));

            int firstNormal = m_normals.Count;
            m_normals.AddRange(mesh.normals);

            // Add the object itself
            m_meshObjects.Add(new MeshObject()
            {
                localToWorldMatrix = obj.transform.localToWorldMatrix,
                indices_offset = firstIndex,
                indices_count = indices.Length,
                smoothness = obj.Smoothness,
                albedo = new Vector3(obj.Albedo.r, obj.Albedo.g, obj.Albedo.b),
                specular = obj.Specular,
                emission = obj.Emission
            });
        }

        CreateComputeBuffer(ref m_meshObjectBuffer, m_meshObjects, 112);
        CreateComputeBuffer(ref m_vertexBuffer, m_vertices, 12);
        CreateComputeBuffer(ref m_indexBuffer, m_indices, 4);
        CreateComputeBuffer(ref m_normalBuffer, m_normals, 12);
    }

    private static void CreateComputeBuffer<T>(ref ComputeBuffer buffer, List<T> data, int stride)
    where T : struct
    {
        // Do we already have a compute buffer?
        if (buffer != null)
        {
            // If no data or buffer doesn't match the given criteria, release it
            if (data.Count == 0 || buffer.count != data.Count || buffer.stride != stride)
            {
                buffer.Release();
                buffer = null;
            }
        }

        if (data.Count != 0)
        {
            // If the buffer has been released or wasn't there to
            // begin with, create it
            if (buffer == null)
            {
                buffer = new ComputeBuffer(data.Count, stride);
            }

            // Set data on the buffer
            buffer.SetData(data);
        }
    }

    private void SetComputeBuffer(string name, ComputeBuffer buffer)
    {
        if (buffer != null)
        {
            ComputeShader.SetBuffer(m_kernelID, name, buffer);
        }
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