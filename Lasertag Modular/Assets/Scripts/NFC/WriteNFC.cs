using DigitsNFCToolkit;
using DigitsNFCToolkit.Samples;
using System.Text;
using System;
using UnityEngine;
using static DigitsNFCToolkit.Samples.WriteScreenControl;

public class WriteNFC : MonoBehaviour
{
    private string domainType = "PlayerSetup";
    private NDEFMessage pendingMessage;

    void Start()
    {
        NativeNFCManager.AddNDEFWriteFinishedListener(OnNDEFWriteFinished);
        NativeNFCManager.AddNDEFPushFinishedListener(OnNDEFPushFinished);

        AddRecord();
    }

    public void AddRecord()
    {
        Network.Packets.CardInfo cardInfo = new Network.Packets.CardInfo();
        cardInfo.ipAddress = "192.168.1.1";
        cardInfo.champion = Network.Packets.Champions.DEMOLISHER;
        cardInfo.playerId = 1;
        cardInfo.gameId = 2711;
        cardInfo.hexColor = "FFAAAA";
        cardInfo.portToListen = 25565;

        if (pendingMessage == null)
        {
            pendingMessage = new NDEFMessage();
        }

        NDEFRecord record = null;

        record = new ExternalTypeRecord("IP", domainType, Encoding.UTF8.GetBytes(cardInfo.ipAddress));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord("Champion", domainType, Encoding.UTF8.GetBytes(cardInfo.champion.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord("GameId", domainType, Encoding.UTF8.GetBytes(cardInfo.gameId.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord("PlayerId", domainType, Encoding.UTF8.GetBytes(cardInfo.playerId.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord("HexColor", domainType, Encoding.UTF8.GetBytes(cardInfo.hexColor.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord("PortToListen", domainType, Encoding.UTF8.GetBytes(cardInfo.portToListen.ToString()));
        pendingMessage.Records.Add(record);

        //view.UpdateNDEFMessage(pendingMessage);
    }

    //To NFC tag
    public void WriteMessage()
    {
        if (pendingMessage != null)
        {
#if (!UNITY_EDITOR)
				NativeNFCManager.RequestNDEFWrite(pendingMessage);
#endif
        }
    }

    //To device
    public void PushMessage()
    {
        if (pendingMessage != null)
        {
#if (!UNITY_EDITOR) && UNITY_ANDROID
				NativeNFCManager.RequestNDEFPush(pendingMessage);
#endif
        }
    }

    public void OnNDEFWriteFinished(NDEFWriteResult result)
    {
        //view.UpdateNDEFMessage(result.Message);

        string writeResultString = string.Empty;
        if (result.Success)
        {
            writeResultString = string.Format("NDEF Message written successfully to tag {0}", result.TagID);

        }
        else
        {
            writeResultString = string.Format("NDEF Message failed to write to tag {0}\nError: {1}", result.TagID, result.Error);
        }
        Debug.Log(writeResultString);
    }

    public void OnNDEFPushFinished(NDEFPushResult result)
    {
        //view.UpdateNDEFMessage(result.Message);

        string pushResultString = string.Empty;
        if (result.Success)
        {
            pushResultString = "NDEF Message pushed successfully to other device";
        }
        else
        {
            pushResultString = "NDEF Message failed to push to other device";
        }
        Debug.Log(pushResultString);
    }
}
