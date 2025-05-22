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
        baseAgent = CharacterFactory.Instance.CreateAgent(character, agent);
        return baseAgent;
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