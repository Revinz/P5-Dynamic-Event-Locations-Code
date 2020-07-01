using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class ScriptAnim : MonoBehaviour
{
    [Tooltip("Move a random object to show the animation")]
    public bool ShowInEditor = false;
    public bool Disabled = false;
    
    // Update is called once per frame
    void Update()
    {
        if (Disabled)
            return;

        if (ShowInEditor)
            Animate();
        else if (Application.isPlaying)
            Animate();
    }

    public abstract void Animate();
}
