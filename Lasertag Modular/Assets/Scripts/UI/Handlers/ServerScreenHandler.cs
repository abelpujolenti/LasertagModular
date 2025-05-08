using Network.Packets;
using TMPro;
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
    public Button Char1;
    public Button Char2;
    public Button Char3;
    public Button Char4;
    public Button Char5;
    public Button Char6;
    public Button Char7;
    public Button Char8;

    [Header("Team Select")]
    public Toggle TeamSelect; 

    [Header("Player Info")]
    public TMP_InputField PlayerName;

    int NormalModeSelected = 0;
    int playerCounter = 1;

    Characters CurrentCharacterSelected = Characters.NONE;
    string CurrentTeamSelected;
    string CurrentPlayerName;

    private void Start()
    {
        InitialModeSelection.SetActive(true);
        NormalModeSettings.SetActive(false);
        CustomModeSettings.SetActive(false);
        PlayersMatchSettings.SetActive(false);
        MatchWaitRoom.SetActive(false);

        TwoVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(4));
        ThreeVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(6));
        FourVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(8));
        FiveVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(10));

        Char1.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.ENGINEER));
        Char2.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.SCOUT));
        Char3.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.DEFENDER));
        Char4.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.DEMOLISHER));
        Char5.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.REFLECTOR));
        Char6.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.NINJA));
        Char7.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.HEALER));
        Char8.GetComponent<Button>().onClick.AddListener(() => OnCharacterSelected(Characters.HACKER));
    }

    private void OnModeSelected(int value)
    {
        NormalModeSelected = value;
        print(value);
    }

    private void OnCharacterSelected(Characters enumValue)
    {
        CurrentCharacterSelected = enumValue;
        print(enumValue);
    }

    public void TryCreatePlayer()
    {
        if(NormalModeSelected == playerCounter)
        {
            PlayersMatchSettings.SetActive(false);
            MatchWaitRoom.SetActive(true);
        }
        else
        {
            if (PlayerName.text != "" && CurrentCharacterSelected != Characters.NONE)
            {
                CurrentPlayerName = PlayerName.text;
                PlayerName.text = "";
                CurrentCharacterSelected = Characters.NONE;
                EventSystem.current.SetSelectedGameObject(null);
                CurrentTeamSelected = TeamSelect.IsActive() ? "B" : "A";

                //TODO: IF HALF OF PLAYERS ARE ONE TEAM MAKE THE OTHERS BE FROM THE OTHER
                //TODO: GET ALL THIS INFO AND ADD IT TO THE SERVER
                playerCounter++;
            }
        }
    }

    public void TryApplyGameMode()
    {
        if (NormalModeSelected != 0)
        {
            NormalModeSettings.SetActive(false);
            PlayersMatchSettings.SetActive(true);
        }
    }
}