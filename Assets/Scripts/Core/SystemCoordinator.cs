using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SystemCoordinator : Singleton<SystemCoordinator>
{
    [SerializeField]
    private string _OnAllSystemReadyFSMName;
    [SerializeField]
    private string _OnFinishHardLoadFSMName;

    private List<ISystem> _iSystemList;
    private Dictionary<ISystem,bool> _dictonarySystem;
    private int _countSystemReady = 0;

    private void Start()
    {
        StartSystemSetup();
    }
    public void StartSystemSetup()
    {
        _iSystemList=new List<ISystem>();
        _dictonarySystem = new Dictionary<ISystem, bool>();
        //_iSystemList = FindObjectsOfType<GameObject>().OfType<ISystem>().ToList().OrderByDescending(P => P.Priority).ToList();
        foreach (GameObject obj in FindObjectsOfType<GameObject>().ToList()) 
        {

            ISystem isys=obj.GetComponent<ISystem>();
            if (isys != null)
            {
                _iSystemList.Add(isys);
            }
        }
        _iSystemList.OrderByDescending(P => P.Priority).ToList();

        foreach (ISystem sys in _iSystemList)
        {
            _dictonarySystem.Add(sys, false);
            sys.Setup();
        }

    }

    public void FinishSystemSetup(ISystem isystem)
    {
        if(!_dictonarySystem.ContainsKey(isystem)) 
        {
            Debug.LogError("UNLOADED");
            return;
        }

        _dictonarySystem[isystem] = true;
        ++_countSystemReady;

        CheckAllSystemReady();
    }
    private void CheckAllSystemReady()
    {
        if(_countSystemReady == _iSystemList.Count) 
        {
            FlowSystem.Instance.TriggerFSMEvent(_OnAllSystemReadyFSMName);
        }
    }
    public void WaitOnHardLoading()
    {
        TravelSystem.Instance.LoadingIsDone = false;
    }
    public void FinishWaitingOnHardLoading()
    {
        TravelSystem.Instance.LoadingIsDone = true;
        FlowSystem.Instance.TriggerFSMEvent(_OnFinishHardLoadFSMName);
    }
}
