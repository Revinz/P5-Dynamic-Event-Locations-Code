using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
    void Pickup();

    void Throw(Vector3 direction);
}
