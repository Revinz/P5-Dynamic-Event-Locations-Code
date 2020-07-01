using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour {

    public int VIEWDISTANCE = 30;
    public int ExtraCrystallizationDistance = 5;
    //private float sensitivity = 2;
    //private float yaw;
    //private float pitch;
    SphereCollider crystallizationCollider;

    private GameObject character;

    private Vector2 clampInDegrees = new Vector2(360, 180);
    private Vector2 sensitivity = new Vector2(2, 2);
    private Vector2 smoothing = new Vector2(3, 3);
    private Vector2 targetDirection;
    private Vector2 targetCharacterDirection;
    private Vector2 smoothMouse;
    private Vector2 mouseAbsolute;

    // Start is called before the first frame update
    void Start() {
        character = this.transform.parent.gameObject;

        targetDirection = transform.localRotation.eulerAngles;
        targetCharacterDirection = character.transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update() {

        Quaternion targetOrientation = Quaternion.Euler(targetDirection);
        Quaternion targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        smoothMouse.x = Mathf.Lerp(smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        smoothMouse.y = Mathf.Lerp(smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        mouseAbsolute += smoothMouse;
        mouseAbsolute += smoothMouse;

        if (clampInDegrees.x < 360)
            mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        if (clampInDegrees.y < 360)
            mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

        Quaternion yRotation = Quaternion.AngleAxis(mouseAbsolute.x, Vector3.up);
        //character.transform.localRotation = yRotation * targetCharacterOrientation;

        /*yaw += sensitivity * Input.GetAxis("Mouse X");
        pitch -= sensitivity * Input.GetAxis("Mouse Y");
        this.gameObject.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        character.transform.localRotation = Quaternion.AngleAxis(yaw, character.transform.up);
        */
    }

    private void OnDrawGizmos()
    {
        //Draw the view distance
        Handles.BeginGUI();

            Handles.color = Color.white;
            Handles.Label(transform.position, "Player View Radius"); //Without this the circle won't show. It seems like a bug in Unity           
            Handles.DrawWireDisc(transform.position, Vector3.up, VIEWDISTANCE);

        Handles.EndGUI();
    }

    void OnValidate()
    {
        //Update the camera's view distance from the script instead of the camera
        if (Camera.main != null)
            Camera.main.farClipPlane = VIEWDISTANCE;

        //Update the crystallization collider's radius to be a bit further out
        //to prevent the player from noticing the crystals popping
        crystallizationCollider = GameObject.Find("CrystallizationAreaCollider").GetComponent<SphereCollider>();
        crystallizationCollider.radius = VIEWDISTANCE + ExtraCrystallizationDistance;
    }
}
