using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChController1 : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _Rb;
    [SerializeField]
    private Animator _Animator;

    [SerializeField] 
    private PauseViewController _PauseMenu;
    [SerializeField]
    private float _JumpPower = 5f;
    [SerializeField]
    private float _StepPower = 2f;
    [SerializeField]
    private float _StepPowerRunning = 3.5f;
    [SerializeField]
    private float _MaxSpeed = 15f;
    [SerializeField]
    private ScriptableId _IdProvider;
    [SerializeField]
    private List<FootTrigger> _Feets;
    [SerializeField]
    private FeetTrigger _FeetTrigger;


    private Vector3 _direction;
    private Vector3 _normalizedDirection;
    private Vector3 _normalizedVelocity;
    private GameplayInputProvider _InputProvider;
    private bool _walking;
    private bool _running;
    private bool _falling;
    private bool _hipOnce;//to apply the jump force a single time


    private Vector3 _baseVector;
    private Vector3 _velocityXZ;
    private Vector3 _ClampedVelocityXZ;
    [SerializeField]
    private AudioClip _WalkClip;
    [SerializeField]
    private AudioClip _RunClip;
    [SerializeField]
    private AudioClip _JumpClip;
    private void Awake()
    {
        _baseVector = new Vector3(0, 0, 0);
        _direction = _baseVector;
        _InputProvider = PlayerController.Instance.GetInput<GameplayInputProvider>(_IdProvider.Id);
    }
    private void OnEnable()
    {
        _InputProvider.OnMove += MoveCharacter;
        _InputProvider.OnJump += JumpCharacter;
        _InputProvider.OnRun += RunCharacter;
        _InputProvider.OnPause += PauseCharacter;
        _FeetTrigger.OnFalling += FallingCharacter;

        _PauseMenu.OnPauseActive += PauseActiveCharacter;

        _FeetTrigger.OnFalling += FallingCharacter;
        foreach (FootTrigger f in _Feets)
        {
            f.OnGrounded += StepCharacter;
        }
        
        _falling = false;
        _running = false;
        _walking = false;
        _hipOnce = true;
    }
    private void OnDisable()
    {
        _InputProvider.OnMove -= MoveCharacter;
        _InputProvider.OnJump -= JumpCharacter;
        _InputProvider.OnRun -= RunCharacter;
        _FeetTrigger.OnFalling -= FallingCharacter;
        _InputProvider.OnPause -= PauseCharacter;

        _PauseMenu.OnPauseActive -= PauseActiveCharacter;

        foreach (FootTrigger f in _Feets)
        {
            f.OnGrounded -= StepCharacter;
        }
    }

    private void JumpCharacter()
    {
        if (!_falling) 
        {
            _Animator.SetTrigger("Jumping");
            AudioController.Instance.PlaySound(_JumpClip);
            _Rb.AddForce(Vector3.up * _JumpPower, ForceMode.VelocityChange);
            StartCoroutine(JumpDoneCharacter());
        }    
    }
    IEnumerator JumpDoneCharacter()
    {
        yield return new WaitUntil(() => _Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_Animator.IsInTransition(0));
        if (_hipOnce)
        {
            _Rb.AddForce(Vector3.up * _JumpPower, ForceMode.VelocityChange);
            _hipOnce = false;
        }
    }
    private void RunCharacter(bool isRunning)
    {
        _running=isRunning;
        _Animator.SetBool("Running", _running);
    }

    private void MoveCharacter(Vector2 value)
    {
        _direction = new Vector3(value.x, 0f, value.y);
        _walking = true;
        _Animator.SetBool("Walking", _walking);

    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (!_direction.Equals(_baseVector))
        {
            //rotation
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle + 180f, 0f);
        }
        else
        {
            _walking =false;
            _running=false;
            _Animator.SetBool("Running", _running);
            _Animator.SetBool("Walking", _walking);
        }
        _velocityXZ=new Vector3(_Rb.velocity.x,0f, _Rb.velocity.z);
        if (_velocityXZ.magnitude > _MaxSpeed) 
        {
            _ClampedVelocityXZ = Vector3.ClampMagnitude(_velocityXZ, _MaxSpeed);
            _Rb.velocity = new Vector3(_ClampedVelocityXZ.x, _Rb.velocity.y, _ClampedVelocityXZ.z);
        }
        if (!_falling)
        {
            _normalizedDirection = _direction.normalized;
            _normalizedVelocity = _Rb.velocity.normalized;
            _Rb.velocity = new Vector3(_normalizedDirection.x, _normalizedVelocity.y, _normalizedDirection.z) * _Rb.velocity.magnitude;
        }
    }
    private void StepCharacter(bool isOnTheGround)
    {
        if (!isOnTheGround && _walking)
        {
            if (_walking)
            {
                if (_running) _Rb.AddForce(new Vector3(_direction.x, 0.3f, _direction.z) * _StepPowerRunning, ForceMode.VelocityChange);
                else _Rb.AddForce(new Vector3(_direction.x, 0.2f, _direction.z) * _StepPower, ForceMode.VelocityChange);
            }
        }
        else
        {
            AudioController.Instance.PlaySound(_WalkClip);
        }
    }
    private void FallingCharacter(bool isFalling)
    {
        _falling =isFalling;
        _Animator.SetBool("Falling", _falling);
    }
    private void PauseCharacter()
    {
        _PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    private void PauseActiveCharacter(bool active)
    {
        ReciveInputMove(!active);
    }
    private void ReciveInputMove(bool active)
    {
        if (!active)
        {
            _InputProvider.OnMove -= MoveCharacter;
            _InputProvider.OnJump -= JumpCharacter;
            _InputProvider.OnRun -= RunCharacter;
            _InputProvider.OnPause -= PauseCharacter;
            _FeetTrigger.OnFalling -= FallingCharacter;
            foreach (FootTrigger f in _Feets)
            {
                f.OnGrounded -= StepCharacter;
            }
        }
        else
        {
            _InputProvider.OnMove += MoveCharacter;
            _InputProvider.OnJump += JumpCharacter;
            _InputProvider.OnRun += RunCharacter;
            _InputProvider.OnPause += PauseCharacter;
            _FeetTrigger.OnFalling += FallingCharacter;
            foreach (FootTrigger f in _Feets)
            {
                f.OnGrounded += StepCharacter;
            }
        }
        
    }
}
