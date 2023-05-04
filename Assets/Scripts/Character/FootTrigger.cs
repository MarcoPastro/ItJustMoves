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
}
