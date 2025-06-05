using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _text;

    [SerializeField] private Animator _animator;

    bool _isClickable = true;

    private Action<int> _onPressed;
    private Action _listener;
    private Action _notSelected = () => { };

    bool _isPressed;

    private void Start()
    {
        _notSelected = () => _animator.SetTrigger("Normal");
    }

    public void SetOnPressedAction(Action<int> onPressed)
    { 
        _onPressed = onPressed;
    }

    public void SetListener(Action listener) 
    {
        _listener = listener;    
    }

    public void SetText(string text)
    { 
        _text.text = text;
    }

    public string GetText()
    {
        return _text.text;
    }

    public void Select() 
    {
        _animator.SetTrigger("Selected");
    }

    public void Unselect() 
    {
        _notSelected();
    }

    public void SetIsClickable(bool isClickable)
    { 
        _isClickable = isClickable;

        if (_isClickable) 
        {
            ChangeOffState("Normal");
            Unselect();
            return;
        }

        ChangeOffState("Disabled");
        Unselect();
    }

    public void ChangeOffState(string state)
    {
        _notSelected = () => _animator.SetTrigger(state);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isClickable)
        {
            return;
        }

        _onPressed(GetInstanceID());
        _listener();
    }
}
