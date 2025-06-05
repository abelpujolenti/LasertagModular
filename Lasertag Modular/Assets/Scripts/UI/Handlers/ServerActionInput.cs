using Network.NetEntities;
using Network.Packets;
using UnityEngine;

public class ServerActionInput : MonoBehaviour
{
    public ServerScreenHandler ScreenHandler;
    public Server Server;

    public void TryConnectSever()
    {
        Server.SetWiFi(ScreenHandler.SSID.text, ScreenHandler.Password.text);
    }

    public void TryApplyGameMode()
    {
        if (ScreenHandler.NormalModeSelected != 0)
        {
            Server.SetupMatch(ScreenHandler.NormalModeSelected);
            ScreenHandler.SelectButtonAnimator.SetTrigger("Normal");
        }
    }

    public void TryCreatePlayer()
    {
        if (ScreenHandler.CurrentCharacterSelected != Characters.NONE && ScreenHandler.PlayerName.text!= "")
        {
            ScreenHandler.PlayersMatchSettings.SetActive(false);
            ScreenHandler.MatchWaitRoom.SetActive(false);
            ScreenHandler.WaitingForNFC.SetActive(true);
            Server.PrepareCharacter(ScreenHandler.CurrentCharacterSelected, ScreenHandler.PlayerName.text, ScreenHandler.TeamSelect.GetIsOn());
        }
    }
}