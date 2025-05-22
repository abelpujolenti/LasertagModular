using Network.NetEntities;
using UnityEngine;

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
        //Server.PrepareCharacter(ScreenHandler.CurrentCharacterSelected, ScreenHandler.PlayerName.text, ScreenHandler.TeamSelect.GetIsOn());
        ScreenHandler.PlayersMatchSettings.SetActive(false);
        ScreenHandler.WaitingForNFC.SetActive(true);
    }
}