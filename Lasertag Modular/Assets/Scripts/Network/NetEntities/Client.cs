using System;
using System.Collections.Generic;
using System.Net;
using Network.Packets;
using Network.Sockets;
using UnityEngine;
//typdef

namespace Network.NetEntities
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _portToListen;
        [SerializeField] private int _triesToFindPort;

        private TcpSocket _socketWithServer;
        
        private SocketManager _serverSocketManager;

        private void Start()
        {
            _serverSocketManager = new SocketManager((socket) =>
            {
                Debug.Log("Socket Connected: " + socket.GetRemoteAddress());
                
                socket.SubscribeOnDisconnect((socketDisconnected) =>
                {
                    Debug.Log("Socket Disconnected: " + socketDisconnected.GetRemoteAddress());
                });
            });
        }

        public void Connect()
        {
            ConnectToServer(new IPEndPoint(IPAddress.Parse(_ipAddress), _portToListen));
        }

        private void ConnectToServer(IPEndPoint ipEndPoint)
        {
            if (!_serverSocketManager.ConnectToNetEntity(ipEndPoint, _triesToFindPort))
            {
                return;
            }

            Action<List<TcpSocket>> action = (sockets) =>
            {
                if (!_serverSocketManager.StartListener(_portToListen))
                {
                    return;
                }

                foreach (TcpSocket socket in sockets)
                {
                    _socketWithServer = socket;
                    
                    SubscribeToServerPackets();
                }
                    
                _serverSocketManager.StartLoop();
            };

            _serverSocketManager.GetSocketsAsync(action);
        }

        private void SubscribeToServerPackets()
        {
            SubscribeToSetupMobileResponse();
            SubscribeToSetupVestResponse();
            SubscribeToSetupWeaponResponse();
            SubscribeToPlayerReadyToPlay();
            SubscribeToCheckedPlayersAmount();
            SubscribeToReadyPlayersAmount();
            SubscribeToStartGame();
        }

        private void SubscribeToSetupMobileResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_MOBILE_RESPONSE, (bytes) =>
            {
                
            });
        }

        private void SubscribeToSetupVestResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_VEST_RESPONSE, (bytes) =>
            {
                
            });
        }

        private void SubscribeToSetupWeaponResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_WEAPON_RESPONSE, (bytes) =>
            {
                
            });
        }

        private void SubscribeToPlayerReadyToPlay()
        {
            _socketWithServer.Subscribe(PacketKeys.PLAYER_READY_TO_PLAY, (bytes) =>
            {
                
            });
        }

        private void SubscribeToCheckedPlayersAmount()
        {
            _socketWithServer.Subscribe(PacketKeys.CHECKED_PLAYERS_AMOUNT, (bytes) =>
            {
                
            });
        }

        private void SubscribeToReadyPlayersAmount()
        {
            _socketWithServer.Subscribe(PacketKeys.READY_PLAYERS_AMOUNT, (bytes) =>
            {
                
            });
        }

        private void SubscribeToStartGame()
        {
            _socketWithServer.Subscribe(PacketKeys.START_GAME, (bytes) =>
            {
                
            });
        }
    }
}