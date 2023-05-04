using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsViewController : MonoBehaviour
{
    [SerializeField]
    private ScriptableId _IdProvider;

    private MenuInputProvider _InputProvider;

    private void Awake()
    {
        _InputProvider = PlayerController.Instance.GetInput<MenuInputProvider>(_IdProvider.Id);
    }
    private void OnEnable()
    {
        _InputProvider.OnExitMenu += ExitMenu;
    }
    private void OnDisable()
    {
        _InputProvider.OnExitMenu -= ExitMenu;
    }
    private void ExitMenu()
    {
        QuitOptions();
    }
    public void QuitOptions()
    {
        Destroy(gameObject);
    }
}
