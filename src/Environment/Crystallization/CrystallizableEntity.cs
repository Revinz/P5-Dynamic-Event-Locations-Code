using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystallizableEntity : MonoBehaviour
{
    public GameObject[] CrystalLevels;

    public void ShowCrystalLevels(int level)
    {
        if (level == 0)
            return;

        if (level >= 1)
        {
            EnableAllRenderers(CrystalLevels[0]);
            //CrystalLevels[0].SetActive(true);

            if (level >= 2)
            {
                EnableAllRenderers(CrystalLevels[1]);
                //CrystalLevels[1].SetActive(true);

                if (level >= 3)
                {
                    EnableAllRenderers(CrystalLevels[2]);
                    //CrystalLevels[2].SetActive(true);

                }
            }
        } 
    }

    private void EnableAllRenderers(GameObject o)
    {
        foreach (MeshRenderer renderer in o.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = true;
        }
    }

}
