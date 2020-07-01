using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// EventArea determines the area an event can spawn in. 
/// This hides/disables all objects inside the event area to avoid
/// conflict between the premade events and the terrain objects that would be inside the EventArea
/// </summary>
[ExecuteInEditMode]
public class EventArea : MonoBehaviour
{   
    [SerializeField] //Used for showing in the inspector window 
    public EventAreaProperties properties;
    [SerializeField]
    public LayerMask EnvironmentObjectsLayer;
    [SerializeField]
    public bool ShowColliders = false;
    [SerializeField]
    public SphereCollider spawnCollider;
    [SerializeField]
    public SphereCollider emptyAreaCollider;

    private EventAreaGUIHandler GUI;

    public bool UpdateArea = false;

    //Used for updating the area while dragging the object around
    [SerializeField]
    public CollidersData collidersData;

    private void OnEnable()
    {
        GUI = new EventAreaGUIHandler(this);
        collidersData = new CollidersData();       
    }

    public void Start()
    {

        if (!Application.isPlaying)
        {
            UnHideAllEnvObjectsInWorld();
            UpdateEventArea();
        }

    }

    void OnDrawGizmos()
    {
        GUI.OnDrawGizmos();
    }


    private void OnValidate()
    {
        GUI = new EventAreaGUIHandler(this);
        EventAreaProperties.GetProperties(ref properties);
        GUI.OnValidate();
        
        UnhideObjects();
        EditorUtility.SetDirty(transform.gameObject);
        SceneView.RepaintAll();

        if (UpdateArea)
        {
            UpdateEventArea();
        }
    }

    private void UpdateEventArea()
    {
        UnhideObjects();
        FindObjectsInArea();
        UpdateArea = false;
    }
    
    void LateUpdate()
    {
        if (Application.isPlaying)
            return;

        HideObjectsInArea();
    }

    /// <summary>
    /// Finds all objects in the area and adds it to an array that stores all the colliders
    /// </summary>
    void FindObjectsInArea()
    {
        //Prevents the area to be moved up/down on the y-axis, which distorts the size of the area depending on the angle
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);


        //Enable all colliders to make it possible to find all the colliders in the area
        Tree[] allTreesInScene = FindObjectsOfType<Tree>();
        Tree[] originalSettings = (Tree[])allTreesInScene.Clone();
        for (int i = 0; i < allTreesInScene.Length; i++)
        {
            allTreesInScene[i].Collider.enabled = true;
        }
        //Add all colliders in the area to an array
        collidersData.collidersInArea = Physics.OverlapSphere(transform.position, properties.EMPTY_AREA_RADIUS);

        //Revert the settings back to the original settings
        for (int i = 0; i < allTreesInScene.Length; i++)
        {
            allTreesInScene[i].Collider.enabled = originalSettings[i];
        }

    }

    /// <summary>
    /// Hides all the objects inside the EventArea.
    /// 
    /// Note: Must be done in after all EventAreas have called FindObjectsInArea()
    /// </summary>
    private void HideObjectsInArea()
    {
        if (collidersData.collidersInArea == null)
            return;

        //After all event areas have found the colliders in their area,
        //it is then possible to hide the objects, to avoid bug where the objects won't unhide again
        foreach (Collider col in collidersData.collidersInArea)
        {
            if (1 << col.gameObject.layer == EnvironmentObjectsLayer)
            {
                hideObject(col.gameObject);
            }
        }

        collidersData.oldColliders = collidersData.collidersInArea;
    }

    private void UnhideObjects()
    {
        //Unhide the old objects 
        if (collidersData != null)
        {
            if (collidersData.oldColliders != null)
            {
                foreach (Collider col in collidersData.oldColliders)
                {
                    //Prevents showing an acceptable error when entering/leaving playmode
                    if (col == null)
                        continue;

                    if (1 << col.gameObject.layer == EnvironmentObjectsLayer)
                    {
                        UnhideObject(col.gameObject);
                        foreach (Transform c in col.transform)
                        {
                            UnhideObject(col.gameObject);
                        }
                    }
                }
            }
        }
    }

    //Fixes a bug where the objects would permanently stay hidden when entering/leaving playmode
    //Because the ColliderData would get reset
    private void UnHideAllEnvObjectsInWorld()
    {

        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject o in objs)
        {
            if (1 << o.layer == EnvironmentObjectsLayer)
            {
                UnhideObject(o); 
            }
        }
    }

    private void UnhideObject(GameObject o)
    {
        foreach (MeshRenderer rend in o.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = true;
        }
        foreach (BoxCollider coll in o.GetComponentsInChildren<BoxCollider>())
        {
            coll.enabled = true;
        }
    }

    private void hideObject(GameObject o)
    {
        foreach (MeshRenderer rend in o.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
        foreach (BoxCollider coll in o.GetComponentsInChildren<BoxCollider>())
        {
            coll.enabled = false;
        }

    }

}

/// <summary>
/// Stores the colliders in the area.
/// Used to store data between frames in the Editor, and when starting
/// </summary>
public class CollidersData
{
    [SerializeField]
    public Collider[] oldColliders;
    [SerializeField]
    public Collider[] collidersInArea;
}