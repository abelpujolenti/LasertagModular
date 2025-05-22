using Network.Packets;
using System;
using System.Security.Cryptography;
using TMPro;
using UI.Agent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ServerScreenHandler : MonoBehaviour
{
    [Header("General")]
    public GameObject InitialModeSelection;
    public GameObject NormalModeSettings;
    public GameObject CustomModeSettings;
    public GameObject PlayersMatchSettings;
    public GameObject MatchWaitRoom;

    [Header ("Normal Game Modes")]
    public Button TwoVS;
    public Button ThreeVS;
    public Button FourVS;
    public Button FiveVS;

    [Header("Character Select")]
    public GameObject Char1;
    public GameObject Char2;
    public GameObject Char3;
    public GameObject Char4;
    public GameObject Char5;
    public GameObject Char6;
    public GameObject Char7;
    public GameObject Char8;

    [Header("Team Select")]
    public Toggle TeamSelect;
    public HorizontalLayoutGroup AgentsGroup;

    [Header("Player Info")]
    public TMP_InputField PlayerName;
    public GameObject AgentPrefab;

    [Header("Information")]
    public int NormalModeSelected = 0;
    public Characters CurrentCharacterSelected = Characters.NONE;

    private void Start()
    {
        InitialModeSelection.SetActive(true);
        NormalModeSettings.SetActive(false);
        CustomModeSettings.SetActive(false);
        PlayersMatchSettings.SetActive(false);
        MatchWaitRoom.SetActive(false);

        TwoVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(3));
        ThreeVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(5));
        FourVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(7));
        FiveVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(9));

        Char1.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.ENGINEER));
        Char1.GetComponentInChildren<TMP_Text>().SetText(Characters.ENGINEER.ToString());
        Char2.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.SCOUT));
        Char2.GetComponentInChildren<TMP_Text>().SetText(Characters.SCOUT.ToString());
        Char3.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.DEFENDER));
        Char3.GetComponentInChildren<TMP_Text>().SetText(Characters.DEFENDER.ToString());
        Char4.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.DEMOLISHER));
        Char4.GetComponentInChildren<TMP_Text>().SetText(Characters.DEMOLISHER.ToString());
        Char5.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.REFLECTOR));
        Char5.GetComponentInChildren<TMP_Text>().SetText(Characters.REFLECTOR.ToString());
        Char6.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.NINJA));
        Char6.GetComponentInChildren<TMP_Text>().SetText(Characters.NINJA.ToString());
        Char7.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.HEALER));
        Char7.GetComponentInChildren<TMP_Text>().SetText(Characters.HEALER.ToString());
        Char8.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.HACKER));
        Char8.GetComponentInChildren<TMP_Text>().SetText(Characters.HACKER.ToString());
    }

    private void OnModeSelected(int value)
    {
        NormalModeSelected = value;
    }

    private void OnCharacterSelected(Characters enumValue)
    {
        CurrentCharacterSelected = enumValue;
    }
}