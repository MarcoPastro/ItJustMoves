using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolingSystemData", menuName = "ScriptableObjects/PoolingSystemData")]
public class PoolingSystemData : ScriptableObject
{
    public List<PoolManagerBinding> PoolManagerBindings;
}
