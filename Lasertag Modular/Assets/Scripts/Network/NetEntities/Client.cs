using System;
using System.Collections.Generic;
using System.Net;
using Network.Packets;
using Network.Sockets;
using Stream;
using UnityEngine;

//typdef

namespace Network.NetEntities
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _portToListen;
        [SerializeField] private int _triesToFindPort;

        private CardInformation _cardInformation;

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
            
            ConnectToServer(new IPEndPoint(IPAddress.Parse(_ipAddress), _portToListen));
        }
        
        public void ReceiveInformationFromCard(CardInformation cardInformation)
        {
            ConnectToServer(new IPEndPoint(IPAddress.Parse(cardInformation.ipAddress), cardInformation.portToListen));

            _cardInformation = cardInformation;
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
                    
                    SendSetupMobile();
                    
                    SubscribeToLobbyPackets();
                }
                    
                _serverSocketManager.StartLoop();
            };

            _serverSocketManager.GetSocketsAsync(action);
        }

        private void SendSetupMobile()
        {
            SetupMobile setupMobile = new SetupMobile
            {
                gameId = _cardInformation.gameId,
                playerId = _cardInformation.playerId,
                character = _cardInformation.character
            };
            
            _socketWithServer.SendPacket(PacketKeys.SETUP_MOBILE, setupMobile);
        }

        private void SubscribeToLobbyPackets()
        {
            SubscribeToSetupResponse();
            SubscribeToPlayerReadyToPlay();
            SubscribeToCheckedPlayersAmount();
            SubscribeToReadyPlayersAmount();
            SubscribeToStartGame();
        }

        private void UnsubscribeToLobbyPackets()
        {
            _socketWithServer.Unsubscribe(PacketKeys.SETUP_RESPONSE);
            _socketWithServer.Unsubscribe(PacketKeys.PLAYER_READY_TO_PLAY);
            _socketWithServer.Unsubscribe(PacketKeys.CHECKED_PLAYERS_AMOUNT);
            _socketWithServer.Unsubscribe(PacketKeys.READY_PLAYERS_AMOUNT);
            _socketWithServer.Unsubscribe(PacketKeys.START_GAME);
        }

        private void SubscribeToInGamePackets()
        {
            SubscribeToHitResponse();
            SubscribeToHealResponse();
        }

        private void UnsubscribeToInGamePackets()
        {
            _socketWithServer.Unsubscribe(PacketKeys.HIT_RESPONSE);
            _socketWithServer.Unsubscribe(PacketKeys.HEAL);
        }

        private void SubscribeToSetupResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_RESPONSE, (bytes) =>
            {
                SetupResponse setupResponse = bytes.ByteArrayToObjectT<SetupResponse>();
                
                //TODO PASS PLAYERNAME
                //TODO PASS IS VEST CHECKED
                //TODO PASS IS WEAPON CHECKED
            });
        }

        private void SubscribeToPlayerReadyToPlay()
        {
            _socketWithServer.Subscribe(PacketKeys.PLAYER_READY_TO_PLAY, (bytes) =>
            {
                //TODO ENABLE READY BUTTON
            });
        }

        private void SubscribeToCheckedPlayersAmount()
        {
            _socketWithServer.Subscribe(PacketKeys.CHECKED_PLAYERS_AMOUNT, (bytes) =>
            {
                CheckedPlayersAmount checkedPlayersAmount = bytes.ByteArrayToObjectT<CheckedPlayersAmount>();
                
                //TODO PASS CHECKED PLAYERS AMOUNT
            });
        }

        private void SubscribeToReadyPlayersAmount()
        {
            _socketWithServer.Subscribe(PacketKeys.READY_PLAYERS_AMOUNT, (bytes) =>
            {
                ReadyPlayersAmount readyPlayersAmount = bytes.ByteArrayToObjectT<ReadyPlayersAmount>();

                //TODO PASS READY PLAYERS AMOUNT
            });
        }

        private void SubscribeToStartGame()
        {
            _socketWithServer.Subscribe(PacketKeys.START_GAME, (bytes) =>
            {
                //TODO START GAME
                UnsubscribeToLobbyPackets();
                SubscribeToInGamePackets();
            });
        }

        private void SubscribeToHitResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.HIT_RESPONSE, (bytes) =>
            {
                
            });
        }

        private void SubscribeToHealResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.HEAL_RESPONSE, (bytes) =>
            {
                
            });
        }

        private void SubscribeToEndGame()
        {
            _socketWithServer.Subscribe(PacketKeys.END_GAME, (bytes) =>
            {
                UnsubscribeToInGamePackets();
                SubscribeToLobbyPackets();
            });
        }
    }
}