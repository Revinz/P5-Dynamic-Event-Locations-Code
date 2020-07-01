using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRockEvent : MonoBehaviour, IInteractable
{
    public int rockSmashedCount = 0;
    public GameObject centerRock;
    public Rigidbody centerRockRb;
    public ScriptAnim animation;
    public GameObject[] SmallRocks = new GameObject[4];
    private CrystalColors colors = new CrystalColors();

    public bool FallenDown = false;
    public bool Dying = false;
    public float DyingColorTransitionSpeed = 0.05f;

    // Update is called once per frame
    void Update()
    {
        //When all 4 rock shields are destroyed, make the crystal fall down
        if (rockSmashedCount >= 4)
            FallDown();

        //Animate the crystal color transition
        if (Dying)
        {
            CrystalColorAnimation();
        }            
    }

    private void CrystalColorAnimation()
    {
        //Change the crystal material's color to be "dead"
        MeshRenderer rend = centerRock.GetComponent<MeshRenderer>();
        Material mat = rend.sharedMaterial;
        mat.color = Color.Lerp(mat.color, colors.crystalDead, DyingColorTransitionSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Makes the big crystal fall down and the small crystals
    /// </summary>
    private void FallDown()
    {
        animation.Disabled = true;
        centerRockRb.useGravity = true;
        centerRockRb.centerOfMass = new Vector3(0.0f, -0.3f, 0.0f); //To make the rock fall over
        SmallRocksFallDown();
        FallenDown = true;
    }

    /// <summary>
    /// Makes the small crystals around the big crystal fall down
    /// </summary>
    private void SmallRocksFallDown()
    {
        foreach (GameObject o in SmallRocks)
        {
            o.GetComponent<ScriptAnim>().Disabled = true;
            o.GetComponent<CapsuleCollider>().enabled = true;
            o.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void Interact()
    {
        if (rockSmashedCount >= 4 && Dying == false)
            SmashRock();
    }

    public void SmashRock()
    {
        GetComponent<ParticleSystem>().Stop();
        GetComponent<HeartbeatAnim>().Disabled = true;
        Dying = true;
        Texture2D path = this.gameObject.GetComponent<PlayerPath>().DrawPath();
        Texture2D map = this.gameObject.GetComponent<MapOutput>().DrawMap();
        ImageMerger.MergeImages(map, path);
        GameObject.Find("EventQueue").GetComponent<DynamicQueue>().NextEvent();
    }
}
