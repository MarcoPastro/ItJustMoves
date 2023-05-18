using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPoolingSysistem : MonoBehaviour
{
    [SerializeField]
    private string _SceneName;

    [ContextMenu("InstantiatePool")]
    public void InstantiatePool()
    {
        PoolingSystem.Instance.SetupSceneManagers(_SceneName);
    }
}
