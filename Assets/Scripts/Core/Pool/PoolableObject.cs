using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    //Setup the object with possible variation
    public virtual void Setup() {}

    //Clear has to return in the initial state
    public virtual void Clear() {}
}
