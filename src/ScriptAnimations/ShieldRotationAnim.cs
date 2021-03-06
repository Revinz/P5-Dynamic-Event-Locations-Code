﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Makes the crystal's shield rotate
/// </summary>

public class ShieldRotationAnim : ScriptAnim
{
    public float rotationSpeed = 5;

    // Update is called once per frame
    public override void Animate()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
