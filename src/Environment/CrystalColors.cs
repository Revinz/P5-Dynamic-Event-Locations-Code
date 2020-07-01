using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the color for the crystal on start and on quit
/// </summary>
public class CrystalColors : MonoBehaviour
{
    public Material crystalMat;
    public Color crystalAlive;
    public Color crystalDead;
 
    void Start()
    {
        crystalMat.color = crystalAlive;
    }
    private void OnApplicationQuit()
    {
        crystalMat.color = crystalAlive;
    }

}
