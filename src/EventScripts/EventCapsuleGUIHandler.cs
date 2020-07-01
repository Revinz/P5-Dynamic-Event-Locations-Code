using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventCapsuleGUIHandler
{
    private EventCapsule capsule;

    public EventCapsuleGUIHandler(EventCapsule _capsule)
    {
        capsule = _capsule;
    }

    public void DrawGizmos()
    {
        Handles.BeginGUI();

        Handles.Label(capsule.transform.position, capsule.EventName);

        //Shows circle to indicate where the limits of the events are
        Handles.color = Color.red;
        Handles.DrawWireDisc(capsule.transform.position, Vector3.up, capsule.properties.EMPTY_AREA_RADIUS);

        if (capsule.ShowEventAreaIndicators)
        {
            //Help understanding how the event looks like when spawned
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(capsule.transform.position, Vector3.up, capsule.properties.EMPTY_AREA_IN_VIEW_DISTANCE);

            Handles.color = Color.green;
            Handles.DrawWireDisc(capsule.transform.position, Vector3.up, capsule.properties.SPAWN_RADIUS);
        }

        Handles.EndGUI();
    }
}
