using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

[Obsolete("No longer used. Use SceneViewFollowInstead.")]
public class LookAtPlayer : MonoBehaviour
{

    private GameObject player;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    /// <summary>
    /// Makes the camera look at the player
    /// </summary>
    void Update()
    {
        targetPosition = new Vector3(player.transform.position.x,
                                        this.transform.position.y,
                                        player.transform.position.z);

        this.transform.LookAt(targetPosition);
    }
}
