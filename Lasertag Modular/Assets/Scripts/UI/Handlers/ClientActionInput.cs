using Network.NetEntities;
using UnityEngine;

public class ClientActionInput : MonoBehaviour
{
    [SerializeField] private Client _client;

    [Header("PlayerSetting")]
    public MyToggle ReadySelect;

    private void Start()
    {
        ReadySelect.SetAction(ReadyStateSwaped);
    }

    public void ReadyStateSwaped()
    {
        bool isReady = ReadySelect.enabled;
        ReadySelect.SetAction(_client.SendPlayerReadyToPlay);
    }
}