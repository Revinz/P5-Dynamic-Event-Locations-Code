using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSpawner : MonoBehaviour
{
    public GameObject spawnedEvent = null;
    EventCapsule capsule = null;
    void OnTriggerEnter(Collider other)
    {
        DynamicQueue queue = GameObject.Find("EventQueue").GetComponent<DynamicQueue>();

        Debug.Log(queue.nextEvent.GetComponent<EventCapsule>().state.ToString());
        if (capsule == null)
        {
            Debug.Log("Spawned");
            spawnedEvent = Instantiate(queue.nextEvent, transform.position, Quaternion.identity);
            capsule = spawnedEvent.GetComponent<EventCapsule>();
            capsule.HideTerrain(true);
            capsule.state = EventCapsule.EventState.SPAWNED;
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (capsule != null)
        {
            if (capsule.state == EventCapsule.EventState.SEEN || capsule.state == EventCapsule.EventState.COMPLETED)
            {
                return;
            } else
            {
                Destroy(spawnedEvent);
                capsule.state = EventCapsule.EventState.NOT_SPAWNED;
            }
            
        }

    }
}
