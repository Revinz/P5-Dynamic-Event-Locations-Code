using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Spawns the next event when the player is entering the EventArea
/// </summary>
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

    /// <summary>
    /// Destroys the event when leaving the EventArea if the player did not see/complete the event
    /// </summary>
    /// <param name="other">Player's Collider</param>
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
