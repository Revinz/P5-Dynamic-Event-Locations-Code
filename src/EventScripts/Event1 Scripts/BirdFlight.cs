using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the bird fly away in a given direction or random direction
/// </summary>
public class BirdFlight : MonoBehaviour {
    bool hasTakenFlight = false;
    public Animator anim;
    public float AccelerationAmount = 1;
    private float speed = 5f;
    public float turnSpeed = 5f;
    private float turnAngle = 0;
    private float flyAngle = 0.05f;
    public int maxSpeed = 50;

    private float flightTime = 0;

    //Please allow for both RANDOM and a specific direction
    public Vector3 flyDirection;
    public bool randomFlyDirection = false;
    public LayerMask playerlayer;
    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>(); //Enables communication with animator
    }

    private void Update() {
        if (hasTakenFlight)
            Fly();
    }

    private void OnTriggerEnter(Collider other) {
        //Check for player entering the trigger collider based on the layer.
        if (1 << other.gameObject.layer == playerlayer) {
            Debug.Log("Fly bird! Fly!"); //Debug to show the player has actually triggered the trigger
            anim.Play("takeFlight"); //Play flapping animation (Works now, Remember to wrie flight with capital F)
            hasTakenFlight = true; //Set bool for 'Update' that moves the bird
        }
    }

    /// <summary>
    /// Makes the bird fly
    /// </summary>
    private void Fly() {
        transform.Translate(0, 0, -speed * Time.deltaTime); //Changes location of the bird in z direction

        Vector3 flyAngleVector = Vector3.zero;
        if (transform.localRotation.eulerAngles.x < 60 && transform.localRotation.eulerAngles.x > -5)
            flyAngleVector = new Vector3(flyAngle, 0, 0);
        //this.gameObject.transform.Rotate(Vector3.up, flyAngle, Space.Self);

        //Rotate the bird on the y-axis
        Vector3 yAxisRotation = new Vector3(0, Mathf.Sin(turnAngle) * 1.5f, 0);
        turnAngle += turnSpeed * Time.deltaTime;

        this.gameObject.transform.Rotate(yAxisRotation + flyAngleVector);


        if (speed < maxSpeed)
            speed += AccelerationAmount * Time.deltaTime;


        if (flyAngle < 0)
            flyAngle += 0.1f * Time.deltaTime;

        flightTime += Time.deltaTime;
        if (flightTime > 20)
            Destroy(this.gameObject);

    }
}
