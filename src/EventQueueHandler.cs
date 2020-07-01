using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventQueueHandler : MonoBehaviour
{
    private GameObject player;
    
    public AudioClip startEvent;

    public ObstacleTypes[] obstacles;
    public List<ObstacleSpawned> obstaclesSpawned = new List<ObstacleSpawned>();

    [SerializeField] int numberOfInstances;

    void Awake()
    {
        for (int x = 0; x < numberOfInstances; x++)
        {
            ObstacleSpawned obsItem = new ObstacleSpawned();
            for (int y = 0; y < obstacles.Length; y++)
            {
                GameObject obstacle = Instantiate(obstacles[y].prefab, transform) as GameObject;
                obstacle.name = obstacles[y].name;
                obstacle.SetActive(false);
                //obsItem.spawnedObstacles.Add (obstacle);
                print(obsItem.spawnedObstacles);
            }
            obstaclesSpawned.Add(obsItem);
        }
    }


    private void Start()
    {
        startEvent = GetComponent<AudioClip>();
        player = GameObject.Find("Player");
        
        
    }



    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 40)
        {
            SpawnNextEvent();

        }
        else if (Vector3.Distance(player.transform.position, transform.position) > 40)
        {
            despawnEvent();

        }
    }




    public void SpawnNextEvent()
    {
        

    }




    public void despawnEvent()
    {

    }

    private void OnDrawGizmos()
    {
        //Draw the view distance
        Handles.BeginGUI();

        Handles.color = Color.white;
        Handles.Label(transform.position, "Event View Radius");
        Handles.DrawWireDisc(transform.position, Vector3.up, 40);

        Handles.EndGUI();
    }

}

[Serializable]
public class ObstacleTypes
{
    public GameObject prefab;
    public string name;
}

[Serializable]
public class ObstacleSpawned
{
    public List<GameObject> spawnedObstacles = new List<GameObject>();
}

