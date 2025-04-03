using UnityEngine;
using TMPro;

public class ScrollingInputField : MonoBehaviour
{
    public TMP_InputField inputField;

    private void Start()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();

        inputField.onValueChanged.AddListener(OnTextChanged);
    }

    private void OnTextChanged(string text)
    {
        inputField.stringPosition = text.Length;
    }
}