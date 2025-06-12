using System.Linq;
using Factory;
using Interface.Agent;
using Network.Packets;
using UI.Agent;
using UnityEngine;

public class ServerActionOutput : MonoBehaviour
{
    public ServerScreenHandler ScreenHandler;
    int teamASlot = 0;
    int teamBSlot = 0;

    public void ConnectionSettuped()
    {
        ScreenHandler.ServerConnectionSetting.SetActive(false);
        ScreenHandler.InitialModeSelection.SetActive(true);
    }

    public void MatchSetupped()
    {
        ScreenHandler.NormalModeSettings.SetActive(false);
        ScreenHandler.PlayersMatchSettings.SetActive(true);
    }

    [SerializeField] private int _sh;

    private void Update()
    {
        if (_sh == 0)
        {
            return;
        }

        _sh = 0;

        //SetAgent(Characters.ENGINEER, name, false);
    }

    public IBaseAgent SetAgent(Characters character, string name, bool isteamB)
    {
        ScreenHandler.WaitingForNFC.SetActive(false);
        ScreenHandler.PlayersMatchSettings.SetActive(true);

        IBaseAgent baseAgent = null;
        if (!isteamB)
        {
            Agent agent = ScreenHandler.TeamAAgents[teamASlot];
            agent.DecorateAgentPanel(name, character.ToString(), isteamB);
            baseAgent = CharacterFactory.Instance.CreateAgent(character, agent);
            ScreenHandler.TeamAAgents[teamASlot].gameObject.SetActive(true);
            ScreenHandler.TeamAAgents[teamASlot].Initiliaze();
            teamASlot++;
        }
        else
        {
            Agent agent = ScreenHandler.TeamBAgents[teamBSlot];
            agent.DecorateAgentPanel(name, character.ToString(), isteamB);
            baseAgent = CharacterFactory.Instance.CreateAgent(character, agent);
            ScreenHandler.TeamBAgents[teamBSlot].gameObject.SetActive(true);
            ScreenHandler.TeamBAgents[teamBSlot].Initiliaze();
            teamBSlot++;
        }

        return baseAgent;
    }

    public void UpdatePlayerMatchSettings(Characters[] teamA, Characters[] teamB)
    {
        if (!teamB.Contains(Characters.NONE) && teamA.Contains(Characters.NONE))
        {
            ScreenHandler.BlockToggle(false);
        }
        if (!teamA.Contains(Characters.NONE) && teamB.Contains(Characters.NONE))
        {
            ScreenHandler.BlockToggle(true);
        }
        if (!teamB.Contains(Characters.NONE) && !teamB.Contains(Characters.NONE))
        {
            ScreenHandler.SwitchToList();
            ScreenHandler.ListSwitch.gameObject.SetActive(false);
        }

        ScreenHandler.TeamAReceivedList = teamA;
        ScreenHandler.TeamBReceivedList = teamB;
        ScreenHandler.UpdateCharacterButtons();
    }
}