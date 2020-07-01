using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scene camera follow the player. Used during test observation. 
/// /// </summary>
public class SceneViewFollow : MonoBehaviour
{

    private UnityEditor.SceneView sceneView;

    private Transform mainCamTrans;

    [SerializeField]
    private Vector3 vecOffset = new Vector3(0, -140, 75);

    // Start is called before the first frame update
    void Start()
    {

        sceneView = UnityEditor.SceneView.lastActiveSceneView;

        mainCamTrans = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Application.isPlaying)
            Follow();
    }

    private void Follow()
    {
        sceneView.camera.transform.position = mainCamTrans.position - vecOffset;
        sceneView.camera.transform.rotation = Quaternion.LookRotation((sceneView.camera.transform.position - mainCamTrans.position) * -1);
        sceneView.AlignViewToObject(sceneView.camera.transform);
    }
}
