using Interface.Agent;
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

    public IBaseAgent SetAgent(Characters character, string name, bool isTeamB)
    {
        /*GameObject newAgent = Instantiate(ScreenHandler.AgentPrefab);
        newAgent.GetComponent<Agent>().DecorateAgentPanel(name, character.ToString(), team);
        newAgent.transform.SetParent(ScreenHandler.AgentsGroup.transform, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(ScreenHandler.AgentsGroup.GetComponent<RectTransform>());*/
        return null;
    }

    public void UpdatePlayerMatchSettings(Characters[] teamA, Characters[] teamB) //TODO: Llegar�n dos listas de agent
    {
        //TODO: Bloquear las clases ya selecionadas
        //TODO: Bloquear el team a "A" o "B"
    }
}