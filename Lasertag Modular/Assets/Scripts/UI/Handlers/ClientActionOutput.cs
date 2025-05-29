using Factory;
using Interface.Agent;
using Network.Packets;
using UI.Agent;
using UnityEngine;

public class ClientActionOutput : MonoBehaviour
{
    ClientScreenHandler clientScreenHandler;

    public IBaseAgent PlayerConfirmed(Characters character, string name, bool isteamB)
    {
        clientScreenHandler.WaitingForNFC.SetActive(false);
        clientScreenHandler.PlayersSetting.SetActive(true);

        IBaseAgent baseAgent = null;
        Agent agent = clientScreenHandler.playerPanel;
        agent.DecorateAgentPanel(name, character.ToString(), isteamB);
        clientScreenHandler.SetPlayerName(name);
        baseAgent = CharacterFactory.Instance.CreateAgent(character, agent);
        return baseAgent;
    }

    public void SkillUsedInUI(string skillName)
    {
        clientScreenHandler.BlockSkillButtons(skillName);
    }

    public void UpdateClientScores(byte AScore, byte BScore)
    {
        clientScreenHandler.UpdateScore((int)AScore, (int)BScore);
    }

    public void UpdateClientHealth(byte newHealth)
    {
        clientScreenHandler.UpdateHealth((int)newHealth);
    }

    public void StartMatchInterface()
    {
        clientScreenHandler.FromPreparationToMatchScreen();
    }

    public void SetClientSkills(/*Skill skill01, Skill skill02, Skill skill03*/)
    {
        clientScreenHandler.SetSkills(/*Skill skill01, Skill skill02, Skill skill03*/);
    }
    
    public void UpdateConfirmedPlayers(byte confirmedPlayers)
    {
        clientScreenHandler.ConfirmedPlayers = (int)confirmedPlayers;
        clientScreenHandler.UpdateMatchState();
    }

    public void UpdateAllPlayers(byte allPlayers)
    {
        clientScreenHandler.AllPlayers = (int)allPlayers;
        clientScreenHandler.UpdateMatchState();
    }
}