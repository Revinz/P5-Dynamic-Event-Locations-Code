using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wisp : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject DestinationEvent;
    public Vector3 originalPosition;
    public float flySpeed = 0.05f;
    bool flying = false;
    bool returning = false;
    float DistanceBetweenStartAndEvent;

    public bool isDynamic = false;

    // Start is called before the first frame update
    void Start()
    {
        //Check if dynamic scene
        if (SceneManager.GetActiveScene().name.Contains("Dynamic"))
            isDynamic = true;

        originalPosition = transform.position;
        DestinationEvent = FindClosestEvent();

        //Prevent it from going into the ground
        Vector3 EventNoYAxis = new Vector3(DestinationEvent.transform.position.x, originalPosition.y, DestinationEvent.transform.position.y);

        DistanceBetweenStartAndEvent = Mathf.Abs(Vector3.Distance(DestinationEvent.transform.position, originalPosition));


    }

    // Update is called once per frame
    void Update()
    {
        //When close to the event, return to original position;
        float DistanceToEvent = Mathf.Abs(Vector3.Distance(transform.position, DestinationEvent.transform.position));
        if (DistanceToEvent < 20)
        {
            returning = true;
            flying = false;
        }

        if (flying)
            MoveToEvent();

        else if (returning)
            ReturnToStart();

        if (flying)
            Debug.Log(DistanceToEvent);
            
    }

    private GameObject FindClosestEvent()
    {

        //Only find the closest event if the wisp doesn't have an assigned event already
        if (DestinationEvent != null)
            return DestinationEvent;

        if (isDynamic)
        {
           return FindClosestEventArea();
        }
        else
            return FindActiveEvent();

    }

    private GameObject FindClosestEventArea()
    {
        //Find the closest event
        EventArea[] areas = GameObject.FindObjectsOfType<EventArea>();
        GameObject ClosestEvent = areas[0].gameObject;
        float currentShortestDistance = 999999;
        foreach (EventArea area in areas)
        {
            float DistanceToEvent = Mathf.Abs(Vector3.Distance(area.gameObject.transform.position, originalPosition));
            if (DistanceToEvent <= currentShortestDistance)
            {
                ClosestEvent = area.gameObject;
                currentShortestDistance = DistanceToEvent;
                Debug.Log(currentShortestDistance);
            }
        }   

        return ClosestEvent;
    }

    private GameObject FindActiveEvent()
    {
        //Find the closest event
        EventCapsule[] areas = GameObject.FindObjectsOfType<EventCapsule>();
        GameObject ClosestEvent = areas[0].gameObject;
        float currentShortestDistance = 999999;
        foreach (EventCapsule area in areas)
        {
            float DistanceToEvent = Mathf.Abs(Vector3.Distance(area.gameObject.transform.position, originalPosition));
            if (DistanceToEvent <= currentShortestDistance)
            {
                ClosestEvent = area.gameObject;
                currentShortestDistance = DistanceToEvent;
            }
        }

        return ClosestEvent;
    }

    private void MoveToEvent()
    {
        transform.position = Vector3.MoveTowards(transform.position, DestinationEvent.transform.position, DistanceBetweenStartAndEvent * flySpeed * Time.deltaTime);
    }

    private void ReturnToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, originalPosition, DistanceBetweenStartAndEvent * flySpeed/2 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == playerLayer)
        {
            flying = true;
            returning = false;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (flying)
            return;

        if (1 << other.gameObject.layer == playerLayer)
        {
            flying = true;
            returning = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (1 << other.gameObject.layer == playerLayer)
        {
            flying = false;
            returning = true;
        }
    }

}
