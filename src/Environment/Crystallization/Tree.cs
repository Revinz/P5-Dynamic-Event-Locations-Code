using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

[ExecuteInEditMode]
public class Tree : CrystallizableEntity
{
    public bool alreadyCrystallized;
    public BoxCollider Collider;

    private void OnEnable()
    {
        if (Collider == null)
            Collider = GetComponent<BoxCollider>();

        if (Collider != null)
            Collider.enabled = false; //Speeds up the editor after placing the trees
    }

    private void Start()
    {
        SetupTree();

        //Prevent collision and crystallization with/on invisible trees
        if (Application.isPlaying)
        {
            Transform c = transform.Find("branches");
            if (c != null)
            {
                MeshRenderer r = c.GetComponent<MeshRenderer>();
                if (r.enabled == true)
                {
                    Collider.enabled = true;
                }
                else
                {
                    Collider.enabled = false;
                    alreadyCrystallized = true;
                }
            }

        }

    }

    /// <summary>
    /// Disables all the crystal renderers to make a bare tree.
    /// </summary>
    public void SetupTree()
    {

        Collider.enabled = false;
      
        foreach (GameObject o in CrystalLevels)
        {
            MeshRenderer[] renderers = o.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rend in renderers)
            {              
                rend.enabled = false;
            }
        }

    }
}
