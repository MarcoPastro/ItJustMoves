using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField]
    private ScriptableId _ID;

    [SerializeField] 
    private PoolableObject _PoolableObjectPrefab;
    [SerializeField]
    private int _PoolQuantity;
    [SerializeField]
    private int _ObjectsPerFrame;
    private Queue<PoolableObject> _poolQueue;
    
    public string ID => _ID.Id;
    public void Setup()
    {
        StartCoroutine(AsyncInstantiate());
    }

    private IEnumerator AsyncInstantiate()
    {
        _poolQueue = new Queue<PoolableObject>();
        PoolableObject poolableObject;
        for (int i = 1; i <= _PoolQuantity; ++i)//start at 1 to do the % right
        {
            poolableObject = Instantiate(_PoolableObjectPrefab, gameObject.transform);
            poolableObject.gameObject.SetActive(false);
            _poolQueue.Enqueue(poolableObject);//position of the poolManager
            if(i % _ObjectsPerFrame == 0)//stop every _ObjectsPerFrame
            {
                yield return null;//wait a frame
            }
            
        }
        PoolingSystem.Instance.FinishManagerSetup(ID);
    }

    public T GetPoolableObject<T>() where T : PoolableObject //T has to be PoolableObject
    {
        if (_poolQueue.Count == 0)
        {
            Debug.LogError("Queue ended: " + gameObject.name);
            return null;
        }
        T poolableObject = _poolQueue.Dequeue() as T;
        poolableObject.gameObject.SetActive(true);
        poolableObject.Setup();

        return poolableObject;
    }

    public void ReturnPoolableObject(PoolableObject pobj)
    {
        pobj.Clear();
        pobj.gameObject.SetActive(false);
        _poolQueue.Enqueue(pobj);
    }
}
