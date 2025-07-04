using System.Collections.Generic;
using TMPro;
using UI.Agent;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientScreenHandler : MonoBehaviour
{
    [Header("General")]
    public GameObject WaitingForNFC;
    public GameObject PlayersSetting;
    public GameObject InGameUI;

    private GameObject _currentActiveUI;

    [Header("PlayerSetting")]
    public TMP_Text MatchPreparationValue;
    public Agent playerPanel;

    [Header("MatchValues")]
    public int ConfirmedPlayers;
    public int AllPlayers;

    [Header("InGame")]
    public List<GameObject> hearts;
    public TMP_Text PlayerName;
    public TMP_Text AScore;
    public TMP_Text BScore;
    public GameObject Skill01;
    public GameObject Skill02;
    public GameObject Skill03;

    private void Start()
    {
        PlayersSetting.SetActive(false);
        InGameUI.SetActive(false);
        _currentActiveUI = WaitingForNFC;
        _currentActiveUI.SetActive(true);
    }

    public void UpdateMatchState()
    {
        MatchPreparationValue.text = ConfirmedPlayers + "/" + AllPlayers;
    }

    public void SetPlayerName(string newName)
    {
        PlayerName.text = newName;
    }

    public void BackToStartScreenScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void BlockSkillButtons(string skillName)
    {
        if (Skill01.GetComponentInChildren<TMP_Text>().text == skillName)
        {
            Skill01.GetComponentInChildren<Animator>().SetTrigger("Disabled");
            Skill01.GetComponentInChildren<Button>().interactable = false;
        }
        else if (Skill02.GetComponentInChildren<TMP_Text>().text == skillName)
        {
            Skill02.GetComponentInChildren<Animator>().SetTrigger("Disabled");
            Skill02.GetComponentInChildren<Button>().interactable = false;
        }
        else if(Skill03.GetComponentInChildren<TMP_Text>().text == skillName)
        {
            Skill03.GetComponentInChildren<Animator>().SetTrigger("Disabled");
            Skill03.GetComponentInChildren<Button>().interactable = false;
        }
    }

    public void EnableNFC()
    {
        ChangeCurrentUI(WaitingForNFC);
    }

    public void EnablePlayerSetting()
    {
        ChangeCurrentUI(PlayersSetting);
    }

    public void EnableInGameUI()
    {
        ChangeCurrentUI(InGameUI);
    }

    private void ChangeCurrentUI(GameObject ui)
    {
        _currentActiveUI.SetActive(false);
        ui.SetActive(true);
        _currentActiveUI = ui;
    }

    public void FromPreparationToMatchScreen()
    {
        PlayersSetting.SetActive(false);
        InGameUI.SetActive(true);
    }

    public void SetSkills(/*Skill skill01, Skill skill02, Skill skill03*/)
    {
        /*if (skill01 != null)
        {
            Skill01.GetComponent<Button>().onClick.AddListener(() => skill01.TryUseAbility());
            Skill01.GetComponentInChildren<TMP_Text>().SetText(skill01._name);
        }
        else
        {
            Skill01.SetActive(false);
        }
        if (skill02 != null)
        {
            Skill02.GetComponent<Button>().onClick.AddListener(() => skill02.TryUseAbility());
            Skill02.GetComponentInChildren<TMP_Text>().SetText(skill02._name);
        }
        else 
        { 
            Skill02.SetActive(false);
        }
        if (skill03 != null)
        {
            Skill03.GetComponent<Button>().onClick.AddListener(() => skill03.TryUseAbility());
            Skill03.GetComponentInChildren<TMP_Text>().SetText(skill03._name);
        }
        else
        {
            Skill03.SetActive(false);
        }*/
    }

    public void UpdateScore(int ANewScore, int BNewScore)
    {
        AScore.text = ANewScore.ToString();
        BScore.text = BNewScore.ToString();
    }

    public void UpdateHealth(int health)
    {
        int heartCounter = 1;
        foreach (var heart in hearts)
        {
            heart.SetActive(heartCounter <= health);
            heartCounter++;
        }
    }
}