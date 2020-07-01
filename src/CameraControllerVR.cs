using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Took an old camera controller script
/// </summary>
public class CameraControllerVR : MonoBehaviour
{
    public GameObject player;
    float rotX, rotY;

    public float sensitivity = 2; // NOT IMPLIMENTED

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse X") * sensitivity;
        rotY -= Input.GetAxis("Mouse Y") * sensitivity;

        //First rotate the player then rotate the camera because the rotation of the player also induces rotation on the camera
        player.transform.eulerAngles = new Vector3(0, rotX, 0);
        //x = horizontal movement which rotates on the y-axis.
        //y = vertical movement which rotates on the x-axis to move the camera up and down
        transform.eulerAngles = new Vector3(rotY, rotX, 0);
    }
}
