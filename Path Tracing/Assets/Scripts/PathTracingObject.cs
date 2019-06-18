using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("Path Tracing/PathTracingObject")]
public class PathTracingObject : MonoBehaviour
{
    public Color Albedo = Color.white;
    public Vector3 Specular = new Vector3(0.65f, 0.65f, 0.65f);
    public float Smoothness = 0.99f;
    public Vector3 Emission = new Vector3(0.0f, 0.0f, 0.0f);

    private void OnEnable()
    {
        PathTracingManager.RegisterObject(this);
    }

    private void OnDisable()
    {
        PathTracingManager.UnregisterObject(this);
    }

    private void Update()
    {
        if (true == transform.hasChanged)
        {
            transform.hasChanged = false;

            PathTracingManager.RebuildObjects();
        }
    }
}