using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayInputProvider : InputProvider
{
    #region Delegate
    public OnTwoVectorDelegate OnMove;
    public OnVoidDelegate OnJump;
    public OnVoidDelegate OnPause;
    public OnBoolDelegate OnRun;
    #endregion

    [Header("Gameplay")]
    [SerializeField]
    private InputActionReference _Move;

    [SerializeField]
    private InputActionReference _Jump;

    [SerializeField]
    private InputActionReference _Run;

    [SerializeField]
    private InputActionReference _Pause;

    private void OnEnable()
    {
        _Move.action.Enable();
        _Jump.action.Enable();
        _Run.action.Enable();
        _Pause.action.Enable();

        _Move.action.performed += MovePerfomed;
        _Jump.action.performed += JumpPerfomed;
        _Run.action.performed += RunPerfomed;
        _Run.action.canceled += StopRunPerfomed;
        _Pause.action.performed += PausePerfomed;
    }

    private void OnDisable()
    {
        _Move.action.Disable();
        _Jump.action.Disable();
        _Run.action.Disable();
        _Pause.action.Disable();

        _Move.action.performed -= MovePerfomed;
        _Jump.action.performed -= JumpPerfomed;
        _Run.action.performed -= RunPerfomed;
        _Run.action.canceled -= StopRunPerfomed;
        _Pause.action.performed -= PausePerfomed;
    }

    private void MovePerfomed(InputAction.CallbackContext obj)
    {
        Vector2 value = obj.action.ReadValue<Vector2>();
        OnMove?.Invoke(value);
    }

    private void JumpPerfomed(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }
    private void RunPerfomed(InputAction.CallbackContext obj)
    {
        OnRun?.Invoke(true);
    }
    private void StopRunPerfomed(InputAction.CallbackContext obj)
    {
        OnRun?.Invoke(false);
    }
    private void PausePerfomed(InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }
}
