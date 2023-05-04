using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseViewController : MonoBehaviour
{
    public delegate void OnActivePauseDelegate(bool value);
    public OnActivePauseDelegate OnPauseActive;

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
        OnPauseActive?.Invoke(true);
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
        OnPauseActive?.Invoke(false);
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
            QuitGame();
        }
    }
    public void Resume()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OpenOption()
    {
        if (_optionsViewController) return;
        _optionsViewController = Instantiate(_OptionsViewPrefab);
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        AudioController.Instance.StopMusic();
        TravelSystem.Instance.SceneLoad("MainMenu");
    }
}
