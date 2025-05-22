using UnityEngine;
using System.Collections.Generic;

public class ButtonGroup : MonoBehaviour
{
    [SerializeField] private List<MyButton> _buttons;

    private void Start()
    {
        foreach (MyButton button in _buttons)
        {
            button.SetOnPressedAction(OnButtonPressed);
        }
    }

    private void OnButtonPressed(int instanceId)
    {
        foreach (MyButton button in _buttons)
        {
            if (button.GetInstanceID() != instanceId)
            {
                button.Unselect();
                continue;
            }

            button.Select();
        }
    }
}
