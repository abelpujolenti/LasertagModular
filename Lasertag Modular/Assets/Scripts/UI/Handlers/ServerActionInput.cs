using Network.NetEntities;
using Network.Packets;
using UnityEngine;
using UnityEngine.EventSystems;

public class ServerActionInput : MonoBehaviour
{
    public ServerScreenHandler ScreenHandler;
    public Server Server;

    public void TryApplyGameMode()
    {
        //Server.SetupMatch(ScreenHandler.NormalModeSelected);
    }

    public void TryCreatePlayer()
    {
        //Server.PrepareCharacter(ScreenHandler.CurrentCharacterSelected, ScreenHandler.PlayerName.text, ScreenHandler.TeamSelect.isOn)
    }
}