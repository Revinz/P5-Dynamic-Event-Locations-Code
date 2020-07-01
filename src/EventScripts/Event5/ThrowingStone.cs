using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingStone : MonoBehaviour, IThrowable
{
    public LayerMask terrainLayer;
    public BoxCollider PickupCollider;
    public SphereCollider StoneCollider;
    public int throwStrength = 500;
    private bool thrown = false;
    Rigidbody rb;
    public SpriteRenderer outline;

    /// <summary>
    /// Picks up the stone and attaches it in front of the camera
    /// </summary>
    public void Pickup()
    {
        EnableOutline(false);
        transform.position = GameObject.Find("PickupOrigin").transform.position;
        Destroy(GetComponentInChildren<Rigidbody>()); //Required. Otherwise the stone's position won't update since there are 2 rigid bodies on the player
        PickupCollider.enabled = false;
        StoneCollider.enabled = false;
        transform.parent = Camera.main.transform;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180));//prevents the rock going offcenter
    }   

    /// <summary>
    /// Throws the stone in the specified direction
    /// </summary>
    public void Throw(Vector3 direction)
    {
        EnableOutline(true);
        this.gameObject.transform.parent = null;
        rb = transform.gameObject.AddComponent<Rigidbody>();
        StoneCollider.enabled = true;
        PickupCollider.enabled = true;
        rb.AddForce(direction * throwStrength, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        PickupCollider.transform.position = StoneCollider.transform.position;
    }

    private void EnableOutline(bool value)
    {
        outline.enabled = value;
    }

}