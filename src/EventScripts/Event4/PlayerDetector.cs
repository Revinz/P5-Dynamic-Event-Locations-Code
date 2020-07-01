using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour {

    Animator anim;

    SphereCollider col;

    // Start is called before the first frame update
    void Start() {
        anim = this.gameObject.GetComponent<Animator>();
        col  = this.gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            anim.CrossFadeInFixedTime("RunWindUp", 0.5f);
            col.enabled = false;
            this.gameObject.GetComponent<RunAway>().PlayerDetected();
        }
    }
}
