using Network.NetEntities;
using TMPro;
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
        _client.SendPlayerReadyToPlay();
    }
}