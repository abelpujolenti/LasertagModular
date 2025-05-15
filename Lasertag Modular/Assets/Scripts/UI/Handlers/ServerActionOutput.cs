using Network.NetEntities;
using Network.Packets;
using UI.Agent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ServerActionOutput : MonoBehaviour
{
    public ServerScreenHandler ScreenHandler;

    public void MatchSetupped()
    {
        ScreenHandler.NormalModeSettings.SetActive(false);
        ScreenHandler.PlayersMatchSettings.SetActive(true);
    }

    public void AddAgent() //TODO: Llegará un agent
    {
        /*GameObject newAgent = Instantiate(ScreenHandler.AgentPrefab);
        newAgent.GetComponent<Agent>().DecorateAgentPanel(name, character.ToString(), team);
        newAgent.transform.SetParent(ScreenHandler.AgentsGroup.transform, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ScreenHandler.AgentsGroup.GetComponent<RectTransform>());*/
    }

    public void UpdatePlayerMatchSettings() //TODO: Llegarán dos listas de agent
    {
        //TODO: Bloquear las clases ya selecionadas
        //TODO: Bloquear el team a "A" o "B"
    }
}