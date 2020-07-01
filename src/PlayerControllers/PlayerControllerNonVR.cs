using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// DO NOT USE! Not working correctly.
/// </summary>
public class PlayerControllerNonVR : MonoBehaviour {

    [SerializeField]
    private bool logging = false;

    private Vector3 direction;
    public Rigidbody rb;
    public float ogSpeed = 50;
    public float timer = 5;
    private float count = 0;
    private float speed;

    private InteractionScript interactionScript;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        interactionScript = new InteractionScript();
    }

    private void Update()
    {
        interactionScript.Update();
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = ogSpeed * 3;
        } else {
            speed = ogSpeed;
        }
            

        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        if (direction.magnitude >= 1)
            direction = direction.normalized;

        direction *= speed * Time.fixedDeltaTime;
        rb.AddRelativeForce(direction, ForceMode.VelocityChange);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
		
		if (logging){
            if (count >= timer) {
                LogPos(System.DateTime.Now.ToString() + " x " + this.gameObject.transform.position.x + " z " + this.gameObject.transform.position.z);
                count = 0;
            }

			count += Time.deltaTime;
		}
    }

	private void LogPos(string log) {

        string path = "Assets/playerPos.txt";
        //Debug.Log(log);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(log);
        writer.Close();
	}
}
