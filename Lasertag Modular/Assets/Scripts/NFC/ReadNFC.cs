using DigitsNFCToolkit;
using Network.NetEntities;
using Network.Packets;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ReadNFC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private Client client;

    public void Start()
    {
        Debug.Log(ushort.Parse("2711"));

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

        CardInfo cardInfo = new CardInfo();

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

            tmp.text += domainType + "." + domainName + ":" + dataValue;

            switch (domainName)
            {
                case "IP":
                    cardInfo.ipAddress = dataValue;
                    break;
                case "Champion":
                    cardInfo.champion = (Champions)int.Parse(dataValue);
                    break;
                case "GameId":
                    cardInfo.gameId = (ushort)int.Parse(dataValue);
                    break;
                case "PlayerId":
                    cardInfo.playerId = (ushort)int.Parse(dataValue);
                    break;
                case "HexColor":
                    cardInfo.hexColor = dataValue;
                    break;
                case "PortToListen":
                    cardInfo.portToListen = int.Parse(dataValue);
                    break;
                default:
                    break;
            }
        }

        tmp.text += "\n" + cardInfo.Debug() + "\n";
        client.ReceiveInformationFromCard(cardInfo);
    }

}
