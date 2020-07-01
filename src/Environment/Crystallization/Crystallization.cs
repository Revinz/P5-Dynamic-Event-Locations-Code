using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystallization : MonoBehaviour
{

    public int level = 1;

    public void IncreaseLevel()
    {
        level++;
    }

    /// <summary>
    /// Adds crystallization to trees around the object
    /// </summary>
    /// <param name="other">Collider from a tree</param>
    private void OnTriggerEnter(Collider other)
    {
        
        Tree t = other.gameObject.GetComponent<Tree>();
        if (t != null)
        {
            if (level == 0)
                t.alreadyCrystallized = true;           

            if (t.alreadyCrystallized == false)
            {
                t.ShowCrystalLevels(level);
                t.alreadyCrystallized = true;
            }
        }

    }

}
