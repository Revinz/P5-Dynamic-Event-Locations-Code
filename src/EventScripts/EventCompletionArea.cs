using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Completes the event simply by walking into the specific area
/// </summary>
public class EventCompletionArea : MonoBehaviour {

    private bool dynamicScene;
    private ArrowGuide arrow;
    private StaticDemoQueue queue;
    private DynamicQueue dQueue;

    public LayerMask playerLayer;

    private void Start() {
        if (SceneManager.GetActiveScene().name.ToLower().Contains("dynamic"))
            dynamicScene = true;

        if (!dynamicScene) {
            queue = GameObject.Find("QueueObject").GetComponent<StaticDemoQueue>();
            arrow = GameObject.Find("ArrowPivot").GetComponent<ArrowGuide>();
        } else {
            dQueue = GameObject.Find("EventQueue").GetComponent<DynamicQueue>();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (1 << other.gameObject.layer == playerLayer)
            CompleteEvent();
    }

    private void CompleteEvent() {
        
        if (!dynamicScene) {
            StaticDemoQueue.statuesFound++;
            queue.NextStatue().SetActive(true);
            arrow.Destination = queue.NextStatue();
        } else {
            DynamicQueue.statuesFound++;
            dQueue.NextEvent();
        }

        Destroy(this);
    }
}
