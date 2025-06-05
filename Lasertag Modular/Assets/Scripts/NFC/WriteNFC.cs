using System;
using System.Text;
using DigitsNFCToolkit;
using Network.Packets;
using UnityEngine;

public class WriteNFC : MonoBehaviour
{
    private string domainType = "playersetup";
    private NDEFMessage pendingMessage;

    private Action _callAction;

    void Start()
    {
        
#if (!UNITY_EDITOR) && UNITY_ANDROID
        NativeNFCManager.AddNDEFWriteFinishedListener(OnNDEFWriteFinished);
        NativeNFCManager.AddNDEFPushFinishedListener(OnNDEFPushFinished);
#endif
    }

    public void SetCallAction(Action callAction)
    {
        _callAction = callAction;
    }

    public void AddRecord(CardWriteInformation cardWriteInformation)
    {
        if (pendingMessage == null)
        {
            pendingMessage = new NDEFMessage();
        }

        NDEFRecord record = null;

        record = new ExternalTypeRecord(domainType, "ip", Encoding.UTF8.GetBytes(cardWriteInformation.ipAddress));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "champion", Encoding.UTF8.GetBytes(((int)(cardWriteInformation.character)).ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "gameid", Encoding.UTF8.GetBytes(cardWriteInformation.gameId.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "playerid", Encoding.UTF8.GetBytes(cardWriteInformation.playerId.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "isb", Encoding.UTF8.GetBytes(cardWriteInformation.isTeamB.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "porttolisten", Encoding.UTF8.GetBytes(cardWriteInformation.portToListen.ToString()));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "wifi", Encoding.UTF8.GetBytes(cardWriteInformation.wifi));
        pendingMessage.Records.Add(record);
        record = new ExternalTypeRecord(domainType, "password", Encoding.UTF8.GetBytes(cardWriteInformation.password));
        pendingMessage.Records.Add(record);

        //view.UpdateNDEFMessage(pendingMessage);
        WriteMessage();
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
            _callAction();
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
