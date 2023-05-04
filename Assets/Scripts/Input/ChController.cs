using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _Rb;
    [SerializeField]
    private Animator _Animator;
    [SerializeField]
    private float _Speed = 6;
    [SerializeField]
    private float _SpeedAddedWhileRunning = 6;
    [SerializeField]
    private float _JumpPower = 15;
    [SerializeField]
    private ScriptableId _IdProvider;
    [SerializeField]
    private FeetTrigger _Feet;

    private Vector3 _direction;
    private GameplayInputProvider _InputProvider;
    private bool _walking;
    private bool _running;
    private bool _falling;
    private bool _hipOnce;//to apply the jump force a single time
    private Vector3 _baseVector;
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
        _Feet.OnFalling += GroundedCharacter;
        _falling = false;
        _running = false;
        _walking = false;
        _hipOnce= true;
    }
    private void OnDisable()
    {
        _InputProvider.OnMove -= MoveCharacter;
        _InputProvider.OnJump -= JumpCharacter;
        _InputProvider.OnRun -= RunCharacter;
        _Feet.OnFalling -= GroundedCharacter;
    }

    private void JumpCharacter()
    {
        if (!_falling) 
        {
            _Animator.SetBool("Jumping", true);
            AudioController.Instance.PlaySound(_JumpClip);
            StartCoroutine(JumpDoneCharacter());
        }    
    }
    IEnumerator JumpDoneCharacter()
    {
        yield return new WaitUntil(() => _Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_Animator.IsInTransition(0));
        if (_hipOnce)
        {
            _Rb.AddForce(Vector3.up * _JumpPower, ForceMode.VelocityChange);
            _Animator.SetBool("Jumping", false);
            _hipOnce = false;
        }
    }
    private void RunCharacter(bool isRunning)
    {
        _running=isRunning;
    }

    private void MoveCharacter(Vector2 value)
    {
        _direction = new Vector3(value.x, 0f, value.y);
        _walking = true;

    }
    private void FixedUpdate()
    {
        Move();
        ChangeAnimator();
    }
    private void ChangeAnimator()
    {
        _Animator.SetBool("Falling", _falling);
        _Animator.SetBool("Running", _running);
        _Animator.SetBool("Walking", _walking);
    }
    private void Move()
    {
        if (!_direction.Equals(_baseVector))
        {
            AudioClip audio=_WalkClip;
            float speed = _Speed;
            if (_running)
            {
                speed += _SpeedAddedWhileRunning;
                audio = _RunClip;
            }
            //rotation
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle + 180f, 0f);

            _Rb.MovePosition(transform.position + _direction * (speed * Time.deltaTime));
            if(!_falling)   AudioController.Instance.PlaySound(audio);
        }
        else
        {
            _walking=false;
            _running=false;
        }
    }

    private void GroundedCharacter(bool isOnTheGround)
    {
        _falling = !isOnTheGround;
        _hipOnce = isOnTheGround;
    }
    
}
