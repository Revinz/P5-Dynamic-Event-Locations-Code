using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderScript : MonoBehaviour
{
    [SerializeField]
    private Material fogMaterial;

    void Awake()
    {
        //Enable depth texture on the camera
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, fogMaterial);
    }
}
