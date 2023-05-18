using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    public delegate void OnGroundDelegate(bool value);
    public OnGroundDelegate OnGrounded;

    private void OnTriggerEnter(Collider other)
    {
        OnGrounded?.Invoke(true);
    }
    private void OnTriggerExit(Collider other)
    {
        OnGrounded?.Invoke(false);
    }

    /*[SerializeField]
    private float _RaycastLength = 0.1f;

    private bool _wasGrounded=false;
    private RaycastHit _hit;
    private void Update()
    {
        if (!_wasGrounded && Physics.Raycast(transform.position, Vector3.down, out _hit, _RaycastLength))
        {
            if (_hit.distance <= _RaycastLength)
            {
                OnGrounded?.Invoke(true);
                _wasGrounded = true;
            }
            else
            {
                OnGrounded?.Invoke(false);
            }
        }
        else if(_wasGrounded)
        {
            _wasGrounded = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _RaycastLength);
    }*/
}
