using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputProvider : MonoBehaviour 
{
    public delegate void OnTwoVectorDelegate(Vector2 value);
    public delegate void OnFloatDelegate(float value);
    public delegate void OnBoolDelegate(bool value);
    public delegate void OnVoidDelegate();
    [Header("Input Provider")]
    [SerializeField]
    private ScriptableId _id;
    public ScriptableId Id => _id;
}
