using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InputProvider;

public class FeetTrigger : MonoBehaviour
{
    public delegate void OnFallingDelegate(bool value);
    public OnFallingDelegate OnFalling;
    
    private void OnTriggerEnter(Collider other)
    {
        OnFalling?.Invoke(false);
    }
    private void OnTriggerExit(Collider other)
    {
        OnFalling?.Invoke(true);
    }
}
