using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelSystem : Singleton<TravelSystem>
{
    public delegate void TravelCompleteDelegate();
    public TravelCompleteDelegate OnTravelComplete;

    [SerializeField]
    private string _InitialSceneName;
    [SerializeField]
    private string _LoadingSceneName;

    private string _targetSceneName;
    private string _currentSceneName;

    private void Start()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;
        SceneLoad(_InitialSceneName);
    }
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

        AsyncOperation op_target = SceneManager.LoadSceneAsync(_targetSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => { return op_target.isDone; });

        _currentSceneName = _targetSceneName;

        _targetSceneName = string.Empty;

        op_loading = SceneManager.UnloadSceneAsync(_LoadingSceneName);
        yield return new WaitUntil(() => { return op_loading.isDone; });

        OnTravelComplete?.Invoke();
    }
}
