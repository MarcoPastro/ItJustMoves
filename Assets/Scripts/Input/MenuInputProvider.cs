using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInputProvider : InputProvider
{
    #region Delegate
    public OnFloatDelegate OnMoveMenu;
    public OnVoidDelegate OnEnterMenu;
    public OnVoidDelegate OnExitMenu;
    #endregion

    [Header("Menu")]
    [SerializeField]
    private InputActionReference _MoveMenu;

    [SerializeField]
    private InputActionReference _EnterMenu;

    [SerializeField]
    private InputActionReference _ExitMenu;
    private void OnEnable()
    {
        _MoveMenu.action.Enable();
        _EnterMenu.action.Enable();
        _ExitMenu.action.Enable();

        _MoveMenu.action.performed += MoveMenuPerfomed;
        _EnterMenu.action.performed += EnterMenuPerfomed;
        _ExitMenu.action.performed += ExitMenuPerfomed;
    }

    private void OnDisable()
    {
        _MoveMenu.action.Disable();
        _EnterMenu.action.Disable();
        _ExitMenu.action.Disable();

        _MoveMenu.action.performed -= MoveMenuPerfomed;
        _EnterMenu.action.performed -= EnterMenuPerfomed;
        _ExitMenu.action.performed -= ExitMenuPerfomed;
    }
    private void MoveMenuPerfomed(InputAction.CallbackContext obj)
    {
        float value = obj.action.ReadValue<float>();
        OnMoveMenu?.Invoke(value);
    }

    private void EnterMenuPerfomed(InputAction.CallbackContext obj)
    {
        OnEnterMenu?.Invoke();
    }
    private void ExitMenuPerfomed(InputAction.CallbackContext obj)
    {
        OnExitMenu?.Invoke();
    }
}
