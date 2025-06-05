using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _checker;

    private Action _action;

    private bool _isClickable = true;

    private bool _isOn;

    public void SetAction(Action action)
    { 
        _action = action;
    }

    public void SetIsClickable(bool isClickable, bool isOn)
    { 
        _isOn = isOn;
        _isClickable = isClickable;
        _checker.SetActive(_isOn);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isClickable)
        {
            return;
        }
        _isOn ^= true;
        _checker.SetActive(_isOn);
        _action();
    }

    public bool GetIsOn() 
    {
        return _isOn;
    }
}