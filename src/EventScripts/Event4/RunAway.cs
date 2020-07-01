using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunAway : MonoBehaviour {

    [SerializeField]
    private bool leftPerson = false;

    [SerializeField]
    private float speed = 75;

    [SerializeField]
    private float maxSpeed = 350;

    private bool dynamicScene = false;
    private bool turned = false;
    private bool playerDetected = false;

    private GameObject player;
    private Rigidbody rb;

    private Vector3 event5Position;
    private Vector3 directionToEvent;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = this.gameObject.GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().name.ToLower().Contains("dynamic"))
            dynamicScene = true;

        if (!dynamicScene)
            event5Position = GameObject.Find("Event5").transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        
        if (playerDetected && !dynamicScene) {
            directionToEvent = this.gameObject.transform.position - event5Position;
            directionToEvent.Normalize();
            directionToEvent *= speed * Time.fixedDeltaTime;
            transform.LookAt(event5Position, Vector3.up);
            rb.AddRelativeForce(directionToEvent, ForceMode.VelocityChange);
        }

        if (playerDetected && dynamicScene && leftPerson) {
            if (!turned) {
                this.gameObject.transform.Rotate(Vector3.up, -90f, Space.Self);
                turned = true;
            }
            
            if (speed < maxSpeed)
                rb.AddRelativeForce(Vector3.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            if (speed >= maxSpeed)
                rb.AddRelativeForce(Vector3.forward * maxSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if (playerDetected && dynamicScene && !leftPerson) {
            if (!turned) {
                this.gameObject.transform.Rotate(Vector3.up, 90f, Space.Self);
                turned = true;
            }

            if (speed < maxSpeed)
                rb.AddRelativeForce(Vector3.forward * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

            if (speed >= maxSpeed)
                rb.AddRelativeForce(Vector3.forward * maxSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        if (playerDetected && speed < maxSpeed)
            speed += 2f;

        if (Vector3.Distance(this.gameObject.transform.position, player.transform.position) >= 30 && playerDetected)
            Destroy(this.gameObject);
    }

    public void PlayerDetected() {
        playerDetected = true;
    }
}
