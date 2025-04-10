using UnityEngine;
using UnityEngine.EventSystems;

public class UnselectableButton : MonoBehaviour
{
    public void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}