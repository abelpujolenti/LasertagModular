using TMPro;
using UnityEngine;

public class ClientScreenHandler : MonoBehaviour
{
    [Header("General")]
    public GameObject WaitingForNFC;
    public GameObject PlayersSetting;
    public GameObject InGameUI;

    [Header("PlayerSetting")]
    public TMP_Text MatchPreparationValue;
    public Agent playerPanel;

    [Header("MatchValues")]
    public int ConfirmedPlayers;
    public int AllPlayers;

    private void Start()
    {
        WaitingForNFC.SetActive(true);
        PlayersSetting.SetActive(false);
        InGameUI.SetActive(false);
    }

    public void UpdateMatchState()
    {
        MatchPreparationValue.text = ConfirmedPlayers + "/" + AllPlayers;
    }
}