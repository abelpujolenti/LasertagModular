using DigitsNFCToolkit;
using DigitsNFCToolkit.Samples;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ReadNFC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmp;

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
        }
    }

}
