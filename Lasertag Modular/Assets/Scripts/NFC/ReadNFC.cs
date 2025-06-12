using System.Collections.Generic;
using System.Text;
using DigitsNFCToolkit;
using Network.NetEntities;
using Network.Packets;
using UnityEngine;

public class ReadNFC : MonoBehaviour
{
    [SerializeField] private Client client;

    public void Start()
    {
        #if (UNITY_EDITOR)
            return; //Dont init nfc libraries unless in build
        #endif
        NativeNFCManager.AddNFCTagDetectedListener(OnNFCTagDetected);
	    NativeNFCManager.AddNDEFReadFinishedListener(OnNDEFReadFinished);
    }
    
    public void OnNFCTagDetected(NFCTag tag)
    {
        
    }

    public void OnNDEFReadFinished(NDEFReadResult result)
    {
        string readResultString = string.Empty;
        if (result.Success)
        {
            readResultString = string.Format("NDEF Message was read successfully from tag {0}", result.TagID);
            ReadNDEFMessage(result.Message);
        }
        else
        {
            readResultString = string.Format("Failed to read NDEF Message from tag {0}\nError: {1}", result.TagID, result.Error);
        }
        Debug.Log(readResultString);
    }

    private void ReadNDEFMessage(NDEFMessage message)
    {
        List<NDEFRecord> records = message.Records;

        CardInformation cardInfo = new CardInformation();

        int length = records.Count;
        for (int i = 0; i < length; i++)
        {
            NDEFRecord record = records[i];
            if (record.Type != NDEFRecordType.EXTERNAL_TYPE) continue;
            ExternalTypeRecord externalTypeRecord = (ExternalTypeRecord)record;
            int dataLength = externalTypeRecord.domainData.Length;
            string dataValue = Encoding.UTF8.GetString(externalTypeRecord.domainData);
            var domainName = externalTypeRecord.domainName;
            var domainType = externalTypeRecord.domainType;

            switch (domainType)
            {
                case "ip":
                    cardInfo.ipAddress = dataValue;
                    break;
                case "champion":
                    cardInfo.character = (Characters)int.Parse(dataValue);
                    break;
                case "gameid":
                    cardInfo.gameId = int.Parse(dataValue);
                    break;
                case "playerid":
                    cardInfo.playerId = byte.Parse(dataValue);
                    break;
                case "hexcolor":
                    cardInfo.isTeamB = byte.Parse(dataValue);
                    break;
                case "porttolisten":
                    cardInfo.portToListen = int.Parse(dataValue);
                    break;
                default:
                    break;
            }
        }

        client.ReceiveInformationFromCard(cardInfo);
    }

}
