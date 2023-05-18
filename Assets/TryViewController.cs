using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryViewController : MonoBehaviour
{
    [SerializeField]
    private ScriptableId _IDPoolManager;
    [SerializeField]
    private PoolableObject _PoolableObjectPrefab;
    [SerializeField]
    private int _NumberOfObjects=1000;
    [SerializeField]
    private Vector3 _Position;

    private PoolManager MyPoolManager;

    [SerializeField]
    private List<PoolableObject> _PoolableObjectsPool;
    [SerializeField]
    private List<PoolableObject> _PoolableObjectsNoPool;
    private void Awake()
    {
        _PoolableObjectsPool = new List<PoolableObject>();
        _PoolableObjectsNoPool = new List<PoolableObject>();
        MyPoolManager = PoolingSystem.Instance.ReturnThePoolManager(_IDPoolManager.Id);
    }

    public void ButtonLeft()
    {
        if (_PoolableObjectsPool.Count > 0)
        {
            ReturnObjectsPool();
        }
        else
        {
            SpawnWithPool();
        }
    }
    public void ButtonRight()
    {
        if (_PoolableObjectsNoPool.Count > 0)
        {
            DestroyObjectsNoPool();
        }
        else
        {
            SpawnWithOutPool();
        }
    }
    private void SpawnWithPool()
    {
        PoolableObject p;

        for (int i = 0; i < _NumberOfObjects; ++i)
        {
            p = MyPoolManager.GetPoolableObject<PoolableObject>();
            p.gameObject.transform.position = _Position;
            _PoolableObjectsPool.Add(p);
        }
    }

    // Update is called once per frame
    private void SpawnWithOutPool()
    {
        PoolableObject p;

        for (int i=0; i<_NumberOfObjects;  ++i)
        {
            p = Instantiate(_PoolableObjectPrefab);
            p.gameObject.transform.position = _Position;
            _PoolableObjectsNoPool.Add(p);
        }
    }

    private void ReturnObjectsPool()
    {
        int realObjectsInsidePool = _PoolableObjectsPool.Count;
        for (int i=0; i < realObjectsInsidePool; ++i)
        {
            MyPoolManager.ReturnPoolableObject(_PoolableObjectsPool[i]);
        }
        _PoolableObjectsPool.Clear();
    }
    private void DestroyObjectsNoPool()
    {
        foreach (PoolableObject obj in _PoolableObjectsNoPool)
        {
            Destroy(obj.gameObject);
        }
        _PoolableObjectsNoPool.Clear();
    }
}
