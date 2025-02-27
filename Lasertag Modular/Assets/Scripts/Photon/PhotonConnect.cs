using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class PhotonConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado a Photon.");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Soy Server");
        }
        else
        {
            Debug.Log("Soy Cliente");
        }
    }
}

