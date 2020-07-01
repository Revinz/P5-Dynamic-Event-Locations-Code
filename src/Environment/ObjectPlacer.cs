using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
*
*   Terrain Editing Tool
*   Allows for placing prefabs on the terrain with original colliders
*
*   Created by Patrick Staalbo.
*
*/

[ExecuteInEditMode]
public class ObjectPlacer : EditorWindow
{

    public static Terrain terrain;
    public GameObject[] ObjectsToPlace = null;
    public GameObject ParentGameObject = null;
    static ObjectPlacer window;
    bool Enabled = false;

    //Brush settings
    int brushRadius = 5;
    int density = 50;
    int objectZoneRadius = 10;
    bool randomSize = true;
    bool randomRotation = true;
    bool removeObjects = false;
    bool SquarePlacement = false;
    float sizeVariance = 0.2f;
    private int anglingDegrees = 13;


    [MenuItem("TerrainEditor/PlacePrefabs")]
    static void Init()
    {
        window = (ObjectPlacer)EditorWindow.GetWindow(typeof(ObjectPlacer));

        window.Show();
    }


    void OnEnable()
    {
        //Lets us get the scene events and draw in the scene
        SceneView.duringSceneGui -= SceneGUI;
        SceneView.duringSceneGui += SceneGUI;

    }


    void SceneGUI(SceneView sceneView)
    {

        // This will have scene events including mouse down on scenes objects
        Event cur = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(cur.mousePosition);
        RaycastHit hit = new RaycastHit();
        LayerMask TerrainLayer = 1 << 9;

        if (cur.type == EventType.MouseDown && cur.button == 0 && Enabled)
        {
            if (Physics.Raycast(ray, out hit, 1000.0f, TerrainLayer))
            {
                //Debug.Log(Event.current.mousePosition);

                if (ParentGameObject == null && !removeObjects)
                {
                    EditorUtility.DisplayDialog("Missing 'Parent' GameObject", "Please set 'Parent' GameObject.", "OK");
                    return;
                }

                if (!removeObjects)
                    InstantiateObjectAtLoc(hit);
                else
                    RemoveObjects(hit.point);

            }

        }

        //Show brush size only when enabled
        if (!Enabled)
            return;

        Handles.BeginGUI();

        //Show radius
        if (Physics.Raycast(ray, out hit, 1000.0f, TerrainLayer))
        {
            //Debug.Log(hit.point);
            Handles.color = Color.red;

            //Circle
            Vector3 NormalHit = Vector3.Scale(hit.normal, new Vector3(360f, 360f, 360f));
            Handles.Label(hit.point, ""); //Without this the circle won't show. It seems like a bug in Unity
            Handles.DrawWireDisc(hit.point, NormalHit, brushRadius);
            //Debug.Log("Draw");


        }

        Handles.EndGUI();


    }

    /*
        Created by Jannik Pedersen.
    */
    /// <summary>
    /// Removes the object in the user selected brush radius around the specified hit point
    /// </summary>
    /// <param name="hitPoint">The point to remove objects around</param>
    private void RemoveObjects(Vector3 hitPoint)
    {
        Collider[] hitColliders = Physics.OverlapSphere(hitPoint, brushRadius);
        //Debug.Log("Colliders hit : " + hitColliders.Length);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //BUG: This will delete ANYTHING without "Terrain" in it's name, incl. the player and camera etc.
            //Use the 'layer' "EnvironmentObjects" instead. -- Patrick Staalbo
            if (!hitColliders[i].gameObject.name.Contains("Terrain") && !hitColliders[i].gameObject.name.Contains("player") && !hitColliders[i].gameObject.name.Contains("camera"))
            {
                DestroyImmediate(hitColliders[i].gameObject);
            }
        }
    }

    /// <summary>
    /// Spawns an object with random scale and rotation at the hit.point
    /// </summary>
    /// <param name="hit">The raycast hit</param>
    private void InstantiateObjectAtLoc(RaycastHit hit)
    {

        //Find spawn locations inside area
        List<Vector3> spawnLocations = new List<Vector3>();
        if (brushRadius == 1)
            spawnLocations.Add(hit.point);
        else
            spawnLocations = FindPossibleSpawnLocations(hit.point);

        foreach (Vector3 pos in spawnLocations)
        {
            //Get random object from list
            int randomIndex = Random.Range(0, ObjectsToPlace.Length);
            GameObject obj = ObjectsToPlace[randomIndex];

            //Rotate it
            int randomYRotationAmount = randomRotation ? Random.Range(0, 360) : 0; //Just an If statement syntax: boolean ? true : false
            Vector3 normalRotation = Vector3.Scale(hit.normal, new Vector3(360f, 360f, 360f));
            //Debug.Log("Normal: " + normalRotation);       
            Quaternion QRot = Quaternion.FromToRotation(Vector3.up, normalRotation);
            QRot *= Quaternion.Euler(0, randomYRotationAmount, 0);


            //Instantiate it
            GameObject o = Instantiate(obj, pos, QRot);

            //Scale it afterwards (to prevent the prefab also being scaled)
            float randomSizeVariance = Random.Range(1 - sizeVariance / 2, 1 + sizeVariance / 2);
            o.transform.localScale = Vector3.Scale(o.transform.localScale, new Vector3(randomSizeVariance, randomSizeVariance, randomSizeVariance));

            //Add some random y-axis offset down into the ground + 
            //some random angling on the z and x axes for some more variance
            Vector3 randomXZRotationAmount = new Vector3(Random.Range(-anglingDegrees, anglingDegrees), 360f, Random.Range(-anglingDegrees, anglingDegrees));
            Vector3 yAxisPosOffset = new Vector3(0, Random.Range(0, -2f), 0);
            o.transform.position += yAxisPosOffset;
            o.transform.Rotate(randomXZRotationAmount);

            //Parent the gameobject to the gameobject 'folder' for the respective thing
            if (ParentGameObject != null)
                o.transform.SetParent(ParentGameObject.transform);
        }

    }

    /// <summary>
    /// Finds possible spawn positions in a circle or square for the selected object(s) (respective to the current settings)
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <returns>Returns a List<Vector3> of valid spawn positions</returns>
    private List<Vector3> FindPossibleSpawnLocations(Vector3 hitPoint)
    {
        List<Vector3> spawnLocations = new List<Vector3>();

        int colsRows = brushRadius / 10;
        int _correctedDensity = 100 - density;

        for (int x = 0; x < (colsRows + 1); x++) //+1 because it also needs to add the very last row/column
        {
            for (int z = 0; z < (colsRows + 1); z++)
            {
                //Needs 2 different random numbers otherwise the bias can easily be seen
                float posVarianceX = Random.Range(-_correctedDensity, _correctedDensity);
                float posVarianceZ = Random.Range(-_correctedDensity, _correctedDensity);
                Vector3 newPos = new Vector3(hitPoint.x - (colsRows / 2 * _correctedDensity) + _correctedDensity * x + posVarianceX,
                                            0,
                                            hitPoint.z - (colsRows / 2 * _correctedDensity) + _correctedDensity * z + posVarianceZ);

                //Prevents overlapping
                bool overlapping = false;
                foreach (Vector3 otherPos in spawnLocations)
                {
                    if (Vector3.Distance(otherPos, newPos) < objectZoneRadius)
                    {
                        overlapping = true;
                        break;
                    }

                }
                if (overlapping)
                    continue;

                //If not overlapping, add the point
                if (!SquarePlacement)
                {
                    if (Vector3.Distance(hitPoint, newPos) < brushRadius)
                        spawnLocations.Add(newPos);
                }
                else
                {
                    if (hitPoint.x - brushRadius < newPos.x && hitPoint.x + brushRadius > newPos.x &&
                        hitPoint.z - brushRadius < newPos.z && hitPoint.z + brushRadius > newPos.z)
                    {
                        spawnLocations.Add(newPos);
                    }
                }

            }
        }


        return spawnLocations;
    }

    void OnGUI()
    {
        //To add the list of objects
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty objectsProperty = so.FindProperty("ObjectsToPlace");

        EditorGUILayout.PropertyField(objectsProperty, true); // True means show children

        SerializedProperty parentProperty = so.FindProperty("ParentGameObject");

        EditorGUILayout.PropertyField(parentProperty, false); // True means show children
        so.ApplyModifiedProperties(); // Remember to apply modified properties
        EditorGUILayout.Space();

        //Settings
        EditorGUILayout.LabelField("Brush size");
        brushRadius = EditorGUILayout.IntSlider(brushRadius, 1, 1000);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Density");
        density = EditorGUILayout.IntSlider(density, 1, 150);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Object's Radius? (prevent's overlap)");
        objectZoneRadius = EditorGUILayout.IntSlider(objectZoneRadius, 1, 100);

        EditorGUILayout.LabelField("Square Placement?");
        SquarePlacement = EditorGUILayout.Toggle(SquarePlacement);

        EditorGUILayout.LabelField("Random size?");
        randomSize = EditorGUILayout.Toggle(randomSize);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Size Variance (only works if random size is enabled)");
        sizeVariance = EditorGUILayout.FloatField(sizeVariance);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Random Rotation?");
        randomRotation = EditorGUILayout.Toggle(randomRotation);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Remove Stuff?");
        removeObjects = EditorGUILayout.Toggle(removeObjects);

        EditorGUILayout.Space();
        string enabledText = Enabled ? "Enabled" : "Enable Editing";
        Enabled = GUILayout.Toggle(Enabled, enabledText, "Button");

    }

}
