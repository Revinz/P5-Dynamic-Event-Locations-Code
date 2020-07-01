using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartbeatAnim : ScriptAnim
{
    public float speed;
    public float sizeMultiplier;
    private Vector3 orignalScale;
    private float progress = 0;
    // Update is called once per frame

    private void Start()
    {
        orignalScale = transform.localScale;
    }
    public override void Animate()
    {
        float scaling = (Mathf.Abs(Mathf.Sin(progress)) * sizeMultiplier);
        transform.localScale = orignalScale + new Vector3(scaling, scaling, scaling);
        progress += speed * Time.deltaTime;
    }
}
