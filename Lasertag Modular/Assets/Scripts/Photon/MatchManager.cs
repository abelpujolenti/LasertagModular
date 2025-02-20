using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MatchManager : MonoBehaviourPunCallbacks
{
    private string roomName = "SalaDePrueba";

    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado a Photon.");
        CreateOrJoinRoom(roomName);
    }

    public void CreateOrJoinRoom(string roomName)
    {
        Debug.Log("Intentando unirse o crear la sala: " + roomName);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 100;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Jugador unido a la sala: " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Es el MasterClient. Esperando al segundo jugador...");
        }
        else
        {
            Debug.Log("Juego listo para empezar, ambos jugadores están en la sala.");
        }

        if (playerPrefab != null)
        {
            // Aixi es com s'instancien coses, amb PhotonNetwork.Instantiate, l'objecte que es vol instanciar ha de tindre el component PhotonView
            // GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);

            // Aixi es criden les funcions
            // En string el nom de la funcio, diria que no cal que estigui en el mateix .cs la funcio.
            // RpcTarget a qui es vol enviar la funcio
            // Parametres que te la propia funcio
            // GetComponent<PhotonView>().RPC("SetPlayerColor", RpcTarget.AllBuffered, true);
        }
        else
        {
            Debug.LogError("Player prefab is missing.");
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Error al intentar unirse o crear la sala: " + message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Jugador entró a la sala: " + newPlayer.NickName);

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.Log("Ambos jugadores están en la sala. Comenzando el juego...");
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Jugador salió de la sala: " + otherPlayer.NickName);
    }

}
