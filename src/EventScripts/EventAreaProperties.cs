using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAreaProperties : MonoBehaviour
{
    //Not static because then we can't change it in the inspector window
    public int SPAWN_RADIUS = 50;
    public int EMPTY_AREA_RADIUS = 30;
    public int EMPTY_AREA_IN_VIEW_DISTANCE = 0;
    public int VIEW_DISTANCE;
     
    private void OnValidate()
    {
        if (Camera.main.GetComponent<CameraController>() != null)
        {
            VIEW_DISTANCE = Camera.main.GetComponent<CameraController>().VIEWDISTANCE;
            EMPTY_AREA_IN_VIEW_DISTANCE = EMPTY_AREA_RADIUS + VIEW_DISTANCE;
        }
    }
    
    public static EventAreaProperties GetEventAreaProperties()
    {
        GameObject o = GameObject.Find("EventAreaProperties");
        if (o != null)
            return o.GetComponent<EventAreaProperties>();
        else
            return null;
    }

    public static void GetProperties(ref EventAreaProperties _properties)
    {
        if (_properties != null)
            return;

        //This might give an error when opening Unity which is OK
        _properties = GetEventAreaProperties();
    }

}
