using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventAreaGUIHandler : Editor
{
    private EventArea area;
    // Start is called before the first frame update

    public EventAreaGUIHandler(EventArea _area)
    {
        area = _area;
    }

   
    public void OnValidate()
    {
        //Update the colliders radii
        if (area == null)
            return;

        if (area.spawnCollider != null && area.properties != null) //This gives an error on start - this prevents the error from appearing
            area.spawnCollider.radius = area.properties.SPAWN_RADIUS;

        if (area.spawnCollider == null)
            return;
        if (area.emptyAreaCollider == null)
            return;

        //Show/Hide colliders
        if (!area.ShowColliders)
        {            
            area.spawnCollider.hideFlags = HideFlags.HideInHierarchy;
            area.emptyAreaCollider.hideFlags = HideFlags.HideInHierarchy;
        }
        else
        {
            area.spawnCollider.hideFlags = HideFlags.None;
            area.emptyAreaCollider.hideFlags = HideFlags.None;
        }

    }

    public void OnDrawGizmos()
    {
        Handles.BeginGUI();
        Handles.Label(area.transform.position, "Event Area"); //Without this the circle won't show. It seems like a bug in Unity  

        //Empty area circle
        Handles.color = Color.red;
        Handles.DrawWireDisc(area.transform.position, Vector3.up, area.properties.EMPTY_AREA_RADIUS);

        //Spawn circle
        Handles.color = Color.green;
        Handles.DrawWireDisc(area.transform.position, Vector3.up, area.properties.SPAWN_RADIUS);

        //Indicator of when the empty area is in view
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(area.transform.position, Vector3.up, area.properties.EMPTY_AREA_IN_VIEW_DISTANCE);

        Handles.EndGUI();
    }

}

/// <summary>
/// Adds a warning tool-tip to the event area
/// </summary>
[CustomEditor(typeof(EventArea))]
[CanEditMultipleObjects]
public class EventAreaEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("Remember to re-calculate the area by hitting the 'Update Area' checkbox", MessageType.Warning);
    }
}