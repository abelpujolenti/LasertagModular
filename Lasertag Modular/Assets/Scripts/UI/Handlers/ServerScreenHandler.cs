using System.Collections.Generic;
using Network.Packets;
using TMPro;
using UI.Agent;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerScreenHandler : MonoBehaviour
{
    [Header("General")]
    public GameObject ServerConnectionSetting;
    public GameObject InitialModeSelection;
    public GameObject NormalModeSettings;
    public GameObject CustomModeSettings;
    public GameObject PlayersMatchSettings;
    public GameObject WaitingForNFC;
    public GameObject MatchWaitRoom;

    [Header("Server Connection Setting")]
    public TMP_InputField SSID;
    public TMP_InputField Password;

    [Header ("Normal Game Modes")]
    public MyButton TwoVS;
    public MyButton ThreeVS;
    public MyButton FourVS;
    public MyButton FiveVS;
    public Animator SelectButtonAnimator;

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
        ServerConnectionSetting.SetActive(true);
        InitialModeSelection.SetActive(false);
        NormalModeSettings.SetActive(false);
        CustomModeSettings.SetActive(false);
        PlayersMatchSettings.SetActive(false);
        WaitingForNFC.SetActive(false);
        MatchWaitRoom.SetActive(false);

        TwoVS.SetListener(() => OnModeSelected(3));
        ThreeVS.SetListener(() => OnModeSelected(5));
        FourVS.SetListener(() => OnModeSelected(7));
        FiveVS.SetListener(() => OnModeSelected(9));

        for (byte i = 0; i < _characterButtons.Count; i++)
        {
            byte fuckCSharp = i;
            _characterButtons[i].SetListener(() => OnCharacterSelected((Characters)(fuckCSharp + 1)));
            _characterButtons[i].SetText(((Characters)(i + 1)).ToString());
        }

        TeamSelect.SetAction(UpdateCharacterButtons);

        CreationSwitch.onClick.AddListener(() => SwitchToList());
        ListSwitch.onClick.AddListener(() => SwitchToCreation());
    }

    public void BackToInitialSelection()
    {
        InitialModeSelection.SetActive(true);
        NormalModeSettings.SetActive(false);
    }

    public void BackToServerConnection()
    {
        ServerConnectionSetting.SetActive(true);
        InitialModeSelection.SetActive(false);
    }

    public void BackToStartScreenScreen()
    {
        SceneManager.LoadScene("StartScreen");
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
        OnCharacterSelected(Characters.NONE);
        
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
        foreach(MyButton btn in _characterButtons)
        {
            if (btn.GetText() != character.ToString())
            {
                continue;
            }
            btn.SetIsClickable(false);
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