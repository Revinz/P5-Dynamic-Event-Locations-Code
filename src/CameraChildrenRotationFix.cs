using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adjusts the children of the camera right before the rendering is happening
/// to make everything render correctly
/// </summary>
public class CameraChildrenRotationFix : MonoBehaviour
{
    public GameObject children;
    
    void OnPreRender()
    {
        children.transform.rotation = Camera.main.transform.rotation;
    }


}
