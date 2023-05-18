using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelSystem : Singleton<TravelSystem>, ISystem
{
    public delegate void TravelCompleteDelegate();
    public TravelCompleteDelegate OnTravelComplete;
    public bool LoadingIsDone;

    [SerializeField]
    private string _InitialSceneName;
    [SerializeField]
    private string _LoadingSceneName;

    private string _targetSceneName;
    private string _currentSceneName;

    [SerializeField]
    private int _Priority;
    public int Priority { get => _Priority; }

    public void SceneLoad(string name)
    {
        StartCoroutine(Load(name));
    }
    private IEnumerator Load(string name)
    {
        _targetSceneName = name;

        AsyncOperation op_loading = SceneManager.LoadSceneAsync(_LoadingSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return op_loading.isDone; });

        AsyncOperation op_current = SceneManager.UnloadSceneAsync(_currentSceneName);
        yield return new WaitUntil(() => { return op_current.isDone; });

        yield return new WaitUntil(() => { return LoadingIsDone == true; });//wait until loading is done

        AsyncOperation op_target = SceneManager.LoadSceneAsync(_targetSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return op_target.isDone; });

        _currentSceneName = _targetSceneName;

        _targetSceneName = string.Empty;

        op_loading = SceneManager.UnloadSceneAsync(_LoadingSceneName);
        yield return new WaitUntil(() => { return op_loading.isDone; });

        OnTravelComplete?.Invoke();
    }

    public void Setup()
    {
        LoadingIsDone = true;
        _currentSceneName = SceneManager.GetActiveScene().name;
        SystemCoordinator.Instance.FinishSystemSetup(this);
    }
}
