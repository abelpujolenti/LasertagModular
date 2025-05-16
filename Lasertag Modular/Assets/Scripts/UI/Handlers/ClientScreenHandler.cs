using Network.Packets;
using System;
using System.Security.Cryptography;
using TMPro;
using UI.Agent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClientScreenHandler : MonoBehaviour
{
    [Header("General")]
    public GameObject ConnectionScreen;
    public GameObject FailScreen;
    public GameObject WaitingForNFC;
    public GameObject PlayersSetting;
    public GameObject InGameUI;

    [Header("PlayerSetting")]
    public GameObject AgentPrefab;
    public TMP_Text MatchPreparationValue;
    public MyToggle ReadySelect;
    public GameObject AgentSpace;

    private void Start()
    {
        ConnectionScreen.SetActive(true);
        FailScreen.SetActive(false);
        WaitingForNFC.SetActive(false);
        PlayersSetting.SetActive(false);
        InGameUI.SetActive(false);
    }
}