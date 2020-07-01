using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Leap.Unity;
using Leap;

public class VRPlayerController : MonoBehaviour {

    [SerializeField]
    private GameObject cam = null;

    [SerializeField]
    private LeapXRServiceProvider leap = null;

    [SerializeField]
    private bool leftHanded = false;

    [SerializeField]
    private bool logging = false;

    private float speed = 4;
    private float timer = 5;
    private float count = 0;

    private Vector3 direction;
    private Vector3 indexTipCentre;
    private Vector3 thumbTipCentre;

    private Frame curFrame;

    private Hand nDomHand;
    private Hand domHand;
    public SphereCollider domHandInteractionCollider;

    private Bone indexTip;
    private Bone thumbTip;

    private bool pinching = false;

    private bool moving = false;

    private InteractionScript interactor;

    void Awake() {
        if (cam == null)
            cam = this.transform.GetChild(0).gameObject;

        if (leap == null)
            leap = this.transform.GetChild(0).GetComponent<LeapXRServiceProvider>();
    }

    // Start is called before the first frame update
    void Start() {
        interactor = new InteractionScript();
        //Making it so the cursor is not visible in the play area
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        //Getting the current frame from the leap motion
        curFrame = leap.CurrentFrame;

        //Releasing the mouse cursor so it can be seen in the play area
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

        //For loop to get each hand
        for (int i = 0; i < curFrame.Hands.Count; i++) {
            if (i >= 2) {
                Debug.Log("More than two hands detected");
                break;
            }

            //Setting dominant hand based on the leftHanded variable
            if (leftHanded) {
                if (curFrame.Hands[i].IsLeft) {
                    domHand = curFrame.Hands[i];
                } else {
                    nDomHand = curFrame.Hands[i];
                }
            } else {
                if (curFrame.Hands[i].IsLeft) {
                    nDomHand = curFrame.Hands[i];
                } else {
                    domHand = curFrame.Hands[i];
                }
            }
        }

        //Finding the bone for the tip of the index and thumb finger
        indexTip = domHand.Fingers[1].bones[3];
        thumbTip = domHand.Fingers[0].bones[3];

        //Doing this cause Leap.Vector can not be converted to Unity Vector
        indexTipCentre = VectorConvert(indexTip);
        thumbTipCentre = VectorConvert(thumbTip);

        if (curFrame.Hands.Contains(domHand))
        {
            //Interaction when pinching
            if (domHand.IsPinching() && !pinching)
            {
                Debug.Log("Interact");
                pinching = true;
                InteractPinch();
            }

            //Throw the object when you stop pinching
            else if (!domHand.IsPinching() && pinching)
            {
                Debug.Log("Throw");
                pinching = false;
                if (interactor.throwableObject != null)
                    interactor.Throw();
            }
        }

        //Leap Fingers List goes from thumb(0) to pinky(4) and checking if index is extended
        if (!nDomHand.Fingers[0].IsExtended &&
             nDomHand.Fingers[1].IsExtended &&
            !nDomHand.Fingers[2].IsExtended &&
            !nDomHand.Fingers[3].IsExtended &&
            !nDomHand.Fingers[4].IsExtended) {

            //Getting the x and z to move the player
            if (!moving) {
                direction.x = cam.transform.forward.x;
                direction.z = cam.transform.forward.z;

                direction.Normalize();
                moving = true;
            }
        }

        //Checking if hand is closed
        if (!nDomHand.Fingers[0].IsExtended &&
            !nDomHand.Fingers[1].IsExtended &&
            !nDomHand.Fingers[2].IsExtended &&
            !nDomHand.Fingers[3].IsExtended &&
            !nDomHand.Fingers[4].IsExtended) {

            moving = false;
        }

        //Moving the player, multiplying by Time.deltaTime so it moves independently of frame rate
        if (moving)
            this.gameObject.transform.Translate(direction * speed * Time.deltaTime, Space.Self);

        if (logging) {
            if (count >= timer) {
                LogPos(System.DateTime.Now.ToString() + " x " + this.gameObject.transform.position.x + " z " + this.gameObject.transform.position.z);
                count = 0;
            }

            count += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f);
    }

    private void InteractPinch() {
        interactor.EmitInteractionRay();
    }

    private Vector3 VectorConvert(Leap.Bone leapBone) {
        Vector3 vec3;

        vec3.x = leapBone.Center.x;
        vec3.y = leapBone.Center.y;
        vec3.z = leapBone.Center.z;

        return vec3;
    }

    private void LogPos(string log) {
        string path = "Assets/playerPos.txt";
        Debug.Log(" " + log);
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(log);
        writer.Close();
    }
}
