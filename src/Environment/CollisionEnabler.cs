using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// Enables the colliders for trees
/// This is done to increase performance, since we have 5k+ trees in the world at all times.
/// 
/// Retrospect: Occlusion culling would have been better than doing this.
/// </summary>
public class CollisionEnabler : MonoBehaviour
{
    public int maxRange = 45;//Needs to be minimum the same size of the crystallization to prevent crystals from popping in!
    List<Collider> prevColliders = new List<Collider>();

    private void FixedUpdate()
    {
        Profiler.BeginSample("CollisionEnabler");
        //Disable the previously found colliders
        foreach (Collider col in prevColliders)
        {
            col.enabled = false;
        }

        //Find the colliders in the new area
        Tree[] allTrees = FindObjectsOfType<Tree>();

        foreach (Tree t in allTrees)
        {
            float distance = Vector3.Distance(t.transform.position, this.transform.position);
            if ((distance < maxRange && distance > 30) || distance < 10)
                t.Collider.enabled = true;
            else
                t.Collider.enabled = false;
        }
        Profiler.EndSample();
    }
}
