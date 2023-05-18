using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolManagerBinding
{
    public string SceneName;
    public List<PoolManager> PoolManagerList;
}

public class PoolingSystem : Singleton<PoolingSystem>, ISystem
{
    [SerializeField]
    private PoolingSystemData _PoolingSystemData;
    [SerializeField]
    private int _Priority;
    public int Priority { get => _Priority; }
    [SerializeField]
    public List<PoolManagerBinding> PoolManagerBinding;

    //scene , list of pools
    private Dictionary<string,List<PoolManager>> _poolManagersDictionary;

    //idPoolManager , PoolManager
    private Dictionary<string, PoolManager> _currentManagersDictonary;
    private int _countManagerReady = 0;

    

    public void Setup()
    {
        _poolManagersDictionary = new Dictionary<string,List<PoolManager>>();
        _currentManagersDictonary = new Dictionary<string, PoolManager>();
        foreach (PoolManagerBinding binding in _PoolingSystemData.PoolManagerBindings)
        {
            _poolManagersDictionary.Add(binding.SceneName, new List<PoolManager>(binding.PoolManagerList));//DO NEW OR List is a reference so the scriptable can be change
        }

        SystemCoordinator.Instance.FinishSystemSetup(this);
    }

    public void SetupSceneManagers(string sceneName)//called by the flowsystem
    {

        if (!_poolManagersDictionary.ContainsKey(sceneName))
        {
            SystemCoordinator.Instance.FinishWaitingOnHardLoading();
            return;
        }
        if (_currentManagersDictonary.Count > 0) DestroyAllManagers();//destroy the managers if they already exists
        
        SystemCoordinator.Instance.WaitOnHardLoading();

        List<PoolManager> list = _poolManagersDictionary[sceneName];
        foreach(PoolManager manager in list)
        {
            PoolManager currentManager = Instantiate(manager, gameObject.transform);
            currentManager.Setup();
            _currentManagersDictonary.Add(currentManager.ID, currentManager);
        }
    }
    public void FinishManagerSetup(string manager)
    {
        if (!_currentManagersDictonary.ContainsKey(manager))
        {
            Debug.LogError("UNLOADED");
            return;
        }
        ++_countManagerReady;

        CheckAllManagerReady();
    }
    private void CheckAllManagerReady()
    {
        if (_countManagerReady == _currentManagersDictonary.Count)
        {
            SystemCoordinator.Instance.FinishWaitingOnHardLoading();
        }
    }
    public void DestroyAllManagers()
    {
        foreach(string id in _currentManagersDictonary.Keys)
        {
            Destroy(_currentManagersDictonary[id].gameObject);
        }
        _countManagerReady = 0;
    }
    public void ClearManagersDictonary()
    {
        DestroyAllManagers();
        _currentManagersDictonary.Clear();
    }
    public PoolManager ReturnThePoolManager(string manager)
    {
        if (!_currentManagersDictonary.ContainsKey(manager))
        {
            Debug.LogError("UNLOADED");
            return null;
        }
        return _currentManagersDictonary[manager];
    }
}
