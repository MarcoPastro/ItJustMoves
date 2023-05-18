using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseViewController : MonoBehaviour
{
    [SerializeField]
    private string _OnReturnToMainMenuFSMName = "RETURN_MAIN_MENU";
    [SerializeField]
    private string _FSMVariable = "SCENE_TO_LOAD";

    [SerializeField]
    private string _OnPauseMenuFSMName;

    [SerializeField]
    private ScriptableId _IdProvider;
    [Header("Option View Prefab")]
    [SerializeField]
    private OptionsViewController _OptionsViewPrefab;
    [Header("Buttons in order top/bottom")]
    [SerializeField]
    private List<Button> PauseButtons;
    [Header("Colors of the buttons")]
    [SerializeField]
    private Color _NormalButtonColor;
    [SerializeField]
    private Color _SelectedButtonColor;

    private int _selectedMainIndex;
    private bool _thereAreButtons;
    private OptionsViewController _optionsViewController;
    private MenuInputProvider _InputProvider;
    private void Awake()
    {
        _InputProvider = PlayerController.Instance.GetInput<MenuInputProvider>(_IdProvider.Id);
    }
    private void OnEnable()
    {
        PlayerController.Instance.EnableInputProvider(_IdProvider.Id);
        _InputProvider.OnMoveMenu += MoveMenu;
        _InputProvider.OnEnterMenu += EnterMenu;
        _InputProvider.OnExitMenu += ExitMenu;

        if (PauseButtons.Count > 0)
        {
            _selectedMainIndex = 0;
            PauseButtons[_selectedMainIndex].image.color = _SelectedButtonColor;
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
        if (!_optionsViewController && _selectedMainIndex>=0)
        {
            if (movement > 0 && _selectedMainIndex > 0)
            {
                PauseButtons[_selectedMainIndex].image.color = _NormalButtonColor;
                --_selectedMainIndex;
            }
            if (movement < 0 && _selectedMainIndex < PauseButtons.Count - 1)
            {
                PauseButtons[_selectedMainIndex].image.color = _NormalButtonColor;
                ++_selectedMainIndex;
            }
            PauseButtons[_selectedMainIndex].image.color = _SelectedButtonColor;
        }
    }
    private void EnterMenu()
    {
        if (!_optionsViewController && _selectedMainIndex >= 0)
        {
            PauseButtons[_selectedMainIndex].onClick?.Invoke();
        }
    }
    private void ExitMenu()
    {
        if (!_optionsViewController)
        {
            Resume();
        }
    }
    public void OnPause(bool value)
    {
        if (!value) return;
        Resume();
    }
    public void Resume()
    {
        PauseButtons[_selectedMainIndex].image.color = _NormalButtonColor;
        FlowSystem.Instance.TriggerFSMEvent(_OnPauseMenuFSMName);
    }
    public void OpenOption()
    {
        if (_optionsViewController) return;
        _optionsViewController = Instantiate(_OptionsViewPrefab);
    }
    public void ChangeScene(string nameScene)
    {
        FlowSystem.Instance.SetFSMVariable(_FSMVariable, nameScene);
        FlowSystem.Instance.TriggerFSMEvent(_OnReturnToMainMenuFSMName);
    }
}
