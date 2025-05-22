using UnityEngine;

public class ClientActionInput : MonoBehaviour
{
    ClientScreenHandler clientScreenHandler;

    [Header("PlayerSetting")]
    public MyToggle ReadySelect;

    private void Start()
    {
        ReadySelect.SetAction(ReadyStateSwaped);
    }

    public void ReadyStateSwaped()
    {
        bool isReady = ReadySelect.enabled;
        //TODO: call changed player ready state
    }
}