﻿using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Raymarch (Generic)")]
public class RaymarchCamera : SceneViewFilter
{
    public Transform sunTransform;
    public int maxStep = 80;
    public float drawDistance = 80.0f;
    public float distanceMargin = 0.001f;
    public float bias = 0.0f;
    public float scale = 1.0f;
    public float power = 5.0f;

    private Vector3[] frustumCornersVec = new Vector3[4];
    private Matrix4x4 frustumCornersMat = Matrix4x4.identity;

    [SerializeField]
    private Texture2D _ColorRamp;

    [SerializeField]
    private Shader _EffectShader;

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

        //EffectMaterial.SetMatrix("_TorusInvMatrix", torusTransform ? torusTransform.localToWorldMatrix.inverse : Matrix4x4.identity.inverse);
        //EffectMaterial.SetMatrix("_BoxInvMatrix", boxTransform ? boxTransform.localToWorldMatrix.inverse : Matrix4x4.identity.inverse);
        //EffectMaterial.SetMatrix("_SphereInvMatrix", sphereTransform ? sphereTransform.localToWorldMatrix.inverse : Matrix4x4.identity.inverse);
        //EffectMaterial.SetMatrix("_PlaneInvMatrix", planeTransform ? planeTransform.localToWorldMatrix.inverse : Matrix4x4.identity.inverse);
        //EffectMaterial.SetMatrix("_Plane2InvMatrix", plane2Transform ? plane2Transform.localToWorldMatrix.inverse : Matrix4x4.identity.inverse);

        EffectMaterial.SetInt("_MaxStep", maxStep);
        EffectMaterial.SetFloat("_DrawDistance", drawDistance);
        EffectMaterial.SetFloat("_DistanceMargin", distanceMargin);

        EffectMaterial.SetFloat("_Bias", bias);
        EffectMaterial.SetFloat("_Scale", scale);
        EffectMaterial.SetFloat("_Power", power);

        //Graphics.Blit(source, destination, EffectMaterial, 0); // use given effect shader as image effect
        CustomGraphicsBlit(source, destination, EffectMaterial, 0); // Replace Graphics.Blit with CustomGraphicsBlit
    }

    //private void Update()
    //{
    //    var worldSpaceCorner = CurrentCamera.transform.TransformVector(frustumCornersVec[0]);
    //    Debug.DrawRay(CurrentCamera.transform.position, worldSpaceCorner, Color.red);
    //}
}