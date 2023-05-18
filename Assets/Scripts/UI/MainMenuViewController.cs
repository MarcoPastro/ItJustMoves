using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuViewController : MonoBehaviour
{
    [SerializeField]
    private string _OnHardLoadingToGameplayFSMName = "ON_HARD_LOADING";
    [SerializeField]
    private string _FSMVariable = "SCENE_TO_LOAD";

    [SerializeField]
    private ScriptableId _IdProvider;
    [Header("Option View Prefab")]
    [SerializeField]
    private OptionsViewController _OptionsViewPrefab;
    [Header("Buttons in order top/bottom")]
    [SerializeField]
    private List<Button> MainButtons;
    [Header("Colors of the buttons")]
    [SerializeField]
    private Color _NormalButtonColor;
    [SerializeField]
    private Color _SelectedButtonColor;

    private int _selectedMainIndex;
    private OptionsViewController _optionsViewController;
    private MenuInputProvider _InputProvider;
    private void Awake()
    {
        _InputProvider = PlayerController.Instance.GetInput<MenuInputProvider>(_IdProvider.Id);
    }
    private void OnEnable()
    {
        _InputProvider.OnMoveMenu += MoveMenu;
        _InputProvider.OnEnterMenu += EnterMenu;
        _InputProvider.OnExitMenu += ExitMenu;

        if (MainButtons.Count > 0)
        {
            _selectedMainIndex = 0;
            MainButtons[_selectedMainIndex].image.color = _SelectedButtonColor;
        }
        else
        {
            _selectedMainIndex = -1;
        }
    }
    private void OnDisable()
    {
        _InputProvider.OnMoveMenu -= MoveMenu;
        _InputProvider.OnEnterMenu -= EnterMenu;
        _InputProvider.OnExitMenu -= ExitMenu;
    }
    private void MoveMenu(float movement)
    {
        if (!_optionsViewController && _selectedMainIndex >= 0) 
        {
            if (movement > 0 && _selectedMainIndex > 0)
            {
                MainButtons[_selectedMainIndex].image.color = _NormalButtonColor;
                --_selectedMainIndex;
            }
            if (movement < 0 && _selectedMainIndex < MainButtons.Count-1)
            {
                MainButtons[_selectedMainIndex].image.color = _NormalButtonColor;
                ++_selectedMainIndex;
            }
            MainButtons[_selectedMainIndex].image.color = _SelectedButtonColor;
        }
    }
    private void EnterMenu()
    {
        if (!_optionsViewController && _selectedMainIndex >= 0)
        {
            MainButtons[_selectedMainIndex].onClick?.Invoke();
        }
    }
    private void ExitMenu()
    {
        if (!_optionsViewController)
        {
            QuitGame();
        }
    }
    public void ChangeScene(string nameScene)
    {
        FlowSystem.Instance.SetFSMVariable(_FSMVariable, nameScene);
        FlowSystem.Instance.TriggerFSMEvent(_OnHardLoadingToGameplayFSMName);   
    }
    public void OpenOption()
    {
        if (_optionsViewController) return;
        _optionsViewController=Instantiate(_OptionsViewPrefab);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
