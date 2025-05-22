using DigitsNFCToolkit;
using DigitsNFCToolkit.Samples;
using System.Text;
using System;
using UnityEngine;
using static DigitsNFCToolkit.Samples.WriteScreenControl;

public class WriteNFC : MonoBehaviour
{
    private string domainType = "playersetup";
    private NDEFMessage pendingMessage;
    [SerializeField]private ReadNFC readModule;

    void Start()
    {
        NativeNFCManager.AddNDEFWriteFinishedListener(OnNDEFWriteFinished);
        NativeNFCManager.AddNDEFPushFinishedListener(OnNDEFPushFinished);

        AddRecord();
    }

    public void AddRecord()
    {
        Network.Packets.CardInformation cardInfo = new Network.Packets.CardInformation();
        cardInfo.ipAddress = "192.168.1.1";
        cardInfo.character = Network.Packets.Characters.DEMOLISHER;
        cardInfo.playerId = 1;
        cardInfo.gameId = 2711;
        cardInfo.isTeamB = 1;
        cardInfo.portToListen = 25565;

        if (pendingMessage == null)
        {
            pendingMessage = new NDEFMessage();
        }

        NDEFRecord record = null;

        record = new ExternalTypeRecord(domainType, "ip", Encoding.UTF8.GetBytes(cardInfo.ipAddress));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "champion", Encoding.UTF8.GetBytes(((int)(cardInfo.character)).ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "gameid", Encoding.UTF8.GetBytes(cardInfo.gameId.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "playerid", Encoding.UTF8.GetBytes(cardInfo.playerId.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "isb", Encoding.UTF8.GetBytes(cardInfo.isTeamB.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "porttolisten", Encoding.UTF8.GetBytes(cardInfo.portToListen.ToString()));
        pendingMessage.Records.Add(record);

        //view.UpdateNDEFMessage(pendingMessage);
    }

    //To NFC tag
    public void WriteMessage()
    {
        readModule.gameObject.SetActive(false);

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

        readModule.gameObject.SetActive(true);
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
