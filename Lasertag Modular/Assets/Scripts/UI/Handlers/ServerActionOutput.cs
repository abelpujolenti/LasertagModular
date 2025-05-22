using System.Linq;
using Interface.Agent;
using Network.Packets;
using UnityEngine;

public class ServerActionOutput : MonoBehaviour
{
    public ServerScreenHandler ScreenHandler;
    int teamASlot = 0;
    int teamBSlot = 0;

    public void MatchSetupped()
    {
        ScreenHandler.NormalModeSettings.SetActive(false);
        ScreenHandler.PlayersMatchSettings.SetActive(true);
    }

    public IBaseAgent SetAgent(Characters character, string name, bool isteamB)
    {
        ScreenHandler.WaitingForNFC.SetActive(false);
        ScreenHandler.PlayersMatchSettings.SetActive(true);

        IBaseAgent baseAgent = null;
        if (!isteamB)
        {
            ScreenHandler.TeamAAgents[teamASlot].DecorateAgentPanel(name, character.ToString(), isteamB);
            //TODO: rellenar baseAgent
            ScreenHandler.TeamAAgents[teamASlot].gameObject.SetActive(true);
            teamASlot++;
        }
        else
        {
            ScreenHandler.TeamBAgents[teamBSlot].DecorateAgentPanel(name, character.ToString(), isteamB);
            //TODO: rellenar baseAgent
            ScreenHandler.TeamAAgents[teamBSlot].gameObject.SetActive(true);
            teamBSlot++;
        }

        return baseAgent;
    }

    public void UpdatePlayerMatchSettings(Characters[] teamA, Characters[] teamB)
    {
        if (teamA.Contains(Characters.NONE))
        {
            ScreenHandler.BlockToggle(false);
        }
        if (teamB.Contains(Characters.NONE))
        {
            ScreenHandler.BlockToggle(true);
        }
        //TODO next screen in case all players are setted

        ScreenHandler.TeamAReceivedList = teamA;
        ScreenHandler.TeamBReceivedList = teamB;
        ScreenHandler.UpdateCharacterButtons();
    }
}