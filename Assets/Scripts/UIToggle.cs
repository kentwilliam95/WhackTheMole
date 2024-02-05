using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public abstract class UIToggle<T> : MonoBehaviour
{
    public Toggle _toggle;
    public T _value;
    public System.Action<T> onValueChanged;

    private void OnValidate()
    {
        if(_toggle == null)
            _toggle = GetComponent<Toggle>();
        
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(OnValueChanged);
    }

    private void Start()
    {
        _toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        if(!value)
            return;
        
        onValueChanged?.Invoke(_value);
    }
}