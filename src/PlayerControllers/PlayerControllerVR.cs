using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
/// This is the controller for both VR and non-VR!
/// </summary>
public class PlayerControllerVR : MonoBehaviour {

    [SerializeField]
    private bool logging = false;

    [SerializeField]
    private float speed = 100;

    public float timer = 2.5f;
    public Rigidbody rb;
    private InteractionScript interactor;
    private LineRenderer line;
    private bool arrow = false;

    private float ogSpeed;
    private float count = 0;

    private void Start() {
        line = this.gameObject.GetComponent<LineRenderer>();
        interactor = new InteractionScript();
        ogSpeed = speed;
        if (!File.Exists("Assets/playerPos.txt"))
            File.Create("Assets/playerPos.txt");
    }

    private void Update() {
        if (arrow) {
            line.SetPosition(0, Camera.main.transform.position);
            line.SetPosition(1, Camera.main.transform.position + (Camera.main.transform.forward * 2));
        }
       
        interactor.Update();
    }

    Vector3 direction = Vector3.zero;
    // Update is called once per frame
    void FixedUpdate() {

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

        /*if (Input.GetKey("Oculus_Button_B") || Input.GetKey("Oculus_Button_Y")) {
            speed = ogSpeed * 2;
        } else {
            speed = ogSpeed;
        }*/

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2.ClampMagnitude(input, 1);

        direction = input.x * Camera.main.transform.right + input.y * Camera.main.transform.forward;
        direction.y = 0;
        direction *= speed * Time.fixedDeltaTime;

        rb.AddForce(direction, ForceMode.VelocityChange);

        if ((StaticDemoQueue.statuesFound >= 5 || DynamicQueue.statuesFound >= 5) && !arrow) {
            arrow = true;
            this.gameObject.GetComponent<LineRenderer>().enabled = true;
        }

        if (logging) {
            if (count >= timer) {
                LogPos(System.DateTime.Now.ToString() + " x " + this.gameObject.transform.position.x + " z " + this.gameObject.transform.position.z);
                count = 0;
            }

            count += Time.deltaTime;
        }
    }

    private void OnDrawGizmos() {

        Gizmos.DrawRay(Camera.main.transform.position, direction * 10f);
    }

    private void LogPos(string log) {

        string path = "Assets/playerPos.txt";
        //Debug.Log(log);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(log);
        writer.Close();
    }
}
