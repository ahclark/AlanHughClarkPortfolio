using UnityEngine;
using System.Collections;

public class PostToonShading : MonoBehaviour {

    // Use this for initialization
    public float intensity;
    private Material material;
    public Shader shad;
	void Awake ()
    {
        material = new Material(Shader.Find("Toon/Lit"));
	}

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}
