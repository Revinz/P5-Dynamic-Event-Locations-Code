using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class EventCapsule : MonoBehaviour
{
    public enum EventState {
        NOT_SPAWNED,
        SPAWNED,
        SEEN,
        COMPLETED
    }
    
    public LayerMask playerLayer;
    public string EventName = "";
    public bool ShowColliders = false;
    public bool ShowEventAreaIndicators = false;

    public EventState state = EventState.NOT_SPAWNED;

    [SerializeField]
    public EventAreaProperties properties;
    public SphereCollider InVisionCollider;
    private EventCapsuleGUIHandler GUIHandler;
    [SerializeField]
    private Terrain terrain;

    public int IN_VIEW_DISTANCE_OFFSET = -7;

    private void Start() {
        if (Application.isPlaying)
            HideTerrain(true);
    }
    private void OnValidate()
    {
        CreateGUIHandler();
        
        if (InVisionCollider != null && properties != null) //just prevents an error log.
            InVisionCollider.radius = properties.EMPTY_AREA_IN_VIEW_DISTANCE + IN_VIEW_DISTANCE_OFFSET;

        if (!Application.isPlaying) //Resource hog. This prevents it from running when playing
            RestrictInspectorChanges();

    }

    private void Awake()
    {
        CreateGUIHandler();
    }

    private void CreateGUIHandler()
    {
        GUIHandler = new EventCapsuleGUIHandler(this);
        EventAreaProperties.GetProperties(ref properties);
    }

    private void OnDrawGizmos()
    {
        GUIHandler.DrawGizmos();
    }
    
    private void RestrictInspectorChanges()
    {
        //Holds the position at 0 on the y-axis, to prevent moving it up
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (properties == null)
            return;
        //Keep the terrain size up to date with the newest property sizes.
        terrain.terrainData.size = new Vector3(properties.EMPTY_AREA_RADIUS * 2, 100, properties.EMPTY_AREA_RADIUS * 2);
        terrain.transform.localPosition = new Vector3(-properties.EMPTY_AREA_RADIUS, 0, -properties.EMPTY_AREA_RADIUS);
    }

    /// Since the events has a terrain of their own, we need to hide it
    public void HideTerrain(bool value)
    {
        terrain.enabled = false;
    }

    //Triggers when the player is inside view range
    public void OnTriggerEnter(Collider other)
    {
        if (1 << other.gameObject.layer == playerLayer)
        {
            Debug.Log("Seen");
            state = EventState.SEEN;
        }
    }
}
