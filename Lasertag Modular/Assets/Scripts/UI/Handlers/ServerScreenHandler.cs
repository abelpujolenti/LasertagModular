using System.Collections.Generic;
using Network.Packets;
using TMPro;
using UI.Agent;
using UnityEngine;
using UnityEngine.UI;

public class ServerScreenHandler : MonoBehaviour
{
    [Header("General")]
    public GameObject InitialModeSelection;
    public GameObject NormalModeSettings;
    public GameObject CustomModeSettings;
    public GameObject PlayersMatchSettings;
    public GameObject WaitingForNFC;
    public GameObject MatchWaitRoom;

    [Header ("Normal Game Modes")]
    public MyButton TwoVS;
    public MyButton ThreeVS;
    public MyButton FourVS;
    public MyButton FiveVS;

    [Header("Character Select")]
    [SerializeField] private List<MyButton> _characterButtons;

    [Header("Team Select")]
    public MyToggle TeamSelect;
    public HorizontalLayoutGroup AgentsGroup;

    [Header("Player Info")]
    public TMP_InputField PlayerName;
    public GameObject AgentPrefab;

    [Header("WaitRoom")]
    public List<Agent> TeamAAgents;
    public List<Agent> TeamBAgents;

    [Header("Information")]
    public int NormalModeSelected = 0;
    public Characters CurrentCharacterSelected = Characters.NONE;
    public Characters[] TeamAReceivedList;
    public Characters[] TeamBReceivedList;

    [Header("Switches")]
    public Button CreationSwitch;
    public Button ListSwitch;

    private void Start()
    {
        InitialModeSelection.SetActive(true);
        NormalModeSettings.SetActive(false);
        CustomModeSettings.SetActive(false);
        PlayersMatchSettings.SetActive(false);
        WaitingForNFC.SetActive(false);
        MatchWaitRoom.SetActive(false);

        TwoVS.SetListener(() => OnModeSelected(3));
        ThreeVS.SetListener(() => OnModeSelected(5));
        FourVS.SetListener(() => OnModeSelected(7));
        FiveVS.SetListener(() => OnModeSelected(9));

        for (int i = 0; i < _characterButtons.Count; i++)
        {
            _characterButtons[i].SetListener(() => OnCharacterSelected((Characters)i + 1));
            _characterButtons[i].SetText(((Characters)i + 1).ToString());
        }

        TeamSelect.SetAction(UpdateCharacterButtons);

        CreationSwitch.onClick.AddListener(() => SwitchToList());
        ListSwitch.onClick.AddListener(() => SwitchToCreation());
    }

    public void BlockToggle(bool isTeamB) 
    {
        TeamSelect.SetIsClickable(false, isTeamB);
    }

    private void OnModeSelected(int value)
    {
        NormalModeSelected = value;
    }

    private void OnCharacterSelected(Characters enumValue)
    {
        CurrentCharacterSelected = enumValue;
    }

    public void UpdateCharacterButtons()
    {
        foreach (MyButton button in _characterButtons)
        {
            button.SetIsClickable(true);
        }

        if (TeamSelect.GetIsOn())
        {
            foreach(Characters charBtn in TeamBReceivedList)
            {
                DisableCharactersButton(charBtn);
            }
        }
        else
        {
            foreach (Characters charBtn in TeamAReceivedList)
            {
                DisableCharactersButton(charBtn);
            }
        }
    }

    private void DisableCharactersButton(Characters character)
    {
        for (int i = 0; i < _characterButtons.Count; ++i)
        {
            if ((int)character != i + 1)
            {
                continue;
            }
            _characterButtons[i].SetIsClickable(false);
        }
    }

    public void SwitchToCreation()
    {
        PlayersMatchSettings.SetActive(true);
        MatchWaitRoom.SetActive(false);
    }

    public void SwitchToList()
    {
        PlayersMatchSettings.SetActive(false);
        MatchWaitRoom.SetActive(true);
    }
}