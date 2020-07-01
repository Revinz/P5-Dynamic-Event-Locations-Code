using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockShieldSmash : MonoBehaviour
{
    public FlyingRockEvent rockEvent;
    private bool smashed = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "throwing_rock" && !smashed)
        {
            RockSmashed();
        }
            
    }

    private void RockSmashed()
    {
        gameObject.transform.parent = null;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.centerOfMass = new Vector3(0.0f, -0.3f, 0.0f); //To make the rockshield fall over
        rockEvent.rockSmashedCount++;
        smashed = true;
    }
}
