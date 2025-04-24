using UnityEngine;
using UnityEngine.UI;

public class ServerScreenHandler : MonoBehaviour
{
    //GENERAL:
    [Header("General")]
    public GameObject InitialModeSelection;
    public GameObject NormalModeSettings;
    public GameObject CustomModeSettings;
    public GameObject PlayersMatchSettings;
    public GameObject MatchWaitRoom;

    //NORMAL GAME MODES:
    [Header ("Normal Game Modes")]
    public Button TwoVS;
    public Button ThreeVS;
    public Button FourVS;
    public Button FiveVS;

    //ENUMS:
    enum NormalMode { VS2, VS3, VS4, VS5 };

    //SAVED DATA:
    NormalMode NormalModeSelected;

    private void Start()
    {
        InitialModeSelection.SetActive(true);
        NormalModeSettings.SetActive(false);
        CustomModeSettings.SetActive(false);
        PlayersMatchSettings.SetActive(false);
        MatchWaitRoom.SetActive(false);

        TwoVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(NormalMode.VS2));
        ThreeVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(NormalMode.VS3));
        FourVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(NormalMode.VS4));
        FiveVS.GetComponent<Button>().onClick.AddListener(() => OnModeSelected(NormalMode.VS5));
    }

    private void OnModeSelected(NormalMode enumValue)
    {
        NormalModeSelected = enumValue;
        print(enumValue);
    }
}