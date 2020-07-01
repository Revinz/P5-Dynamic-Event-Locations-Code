using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnim : ScriptAnim
{
    public float bobbingHeight = 1;
    public float bobbingSpeed = 1;
    public float animProgress = 0;
    Vector3 originalPosition;

    public void OnValidate()
    {
        originalPosition = gameObject.transform.localPosition;
    }

    public override void Animate()
    {
        animProgress += bobbingSpeed * Time.deltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x,
                                              originalPosition.y + Mathf.Sin(animProgress) * bobbingHeight,
                                              transform.localPosition.z);

    }

}
