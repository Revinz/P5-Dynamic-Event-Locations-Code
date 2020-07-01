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
