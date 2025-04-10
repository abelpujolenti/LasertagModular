using UnityEngine;
using UnityEngine.UI;

public class AutoWrapInputField : MonoBehaviour
{
    public InputField inputField;
    public ScrollRect scrollRect;

    /*private void Start()
    {
        if (inputField != null)
        {
            inputField.lineType = InputField.LineType.MultiLineNewline;
            inputField.textComponent.enableWordWrapping = true;
        }

        if (scrollRect != null && inputField != null)
        {
            scrollRect.horizontal = false;
        }
    }

    public void OnTextChanged(string text)
    {
        if (inputField != null)
        {
            inputField.textComponent.text = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(inputField.textComponent.rectTransform);
        }
    }*/
}