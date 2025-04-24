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

        private CardInfo _cardInfo;

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
        
        public void ReceiveInformationFromCard(CardInfo cardInfo)
        {
            ConnectToServer(new IPEndPoint(IPAddress.Parse(cardInfo.ipAddress), cardInfo.portToListen));

            _cardInfo = cardInfo;
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
                gameId = _cardInfo.gameId,
                playerId = _cardInfo.playerId,
                champion = _cardInfo.champion
            };
            
            _socketWithServer.SendPacket(PacketKeys.SETUP_MOBILE, setupMobile);
        }

        private void SubscribeToLobbyPackets()
        {
            SubscribeToSetupMobileResponse();
            SubscribeToSetupVestResponse();
            SubscribeToSetupWeaponResponse();
            SubscribeToPlayerReadyToPlay();
            SubscribeToCheckedPlayersAmount();
            SubscribeToReadyPlayersAmount();
            SubscribeToStartGame();
        }

        private void UnsubscribeToLobbyPackets()
        {
            _socketWithServer.Unsubscribe(PacketKeys.SETUP_MOBILE_RESPONSE);
            _socketWithServer.Unsubscribe(PacketKeys.SETUP_VEST_RESPONSE);
            _socketWithServer.Unsubscribe(PacketKeys.SETUP_WEAPON_RESPONSE);
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

        private void SubscribeToSetupMobileResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_MOBILE_RESPONSE, (bytes) =>
            {
                SetupMobileResponse setupMobileResponse = bytes.ByteArrayToObjectT<SetupMobileResponse>();

                if (!setupMobileResponse.isCorrect)
                {
                    //TODO ERROR FEEDBACK
                    return;
                }
                
                //TODO PASS PLAYERNAME
                //TODO PASS IS VEST CHECKED
                //TODO PASS IS WEAPON CHECKED
            });
        }

        private void SubscribeToSetupVestResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_VEST_RESPONSE, (bytes) =>
            {
                SetupVestResponse setupVestResponse = bytes.ByteArrayToObjectT<SetupVestResponse>();

                if (!setupVestResponse.isCorrect)
                {
                    //TODO ERROR FEEDBACK
                    return;
                }
                
                //TODO PASS IS VEST CHECKED
            });
        }

        private void SubscribeToSetupWeaponResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_WEAPON_RESPONSE, (bytes) =>
            {
                SetupWeaponResponse setupWeaponResponse = bytes.ByteArrayToObjectT<SetupWeaponResponse>();

                if (!setupWeaponResponse.isCorrect)
                {
                    //TODO ERROR FEEDBACK
                    return;
                }
                
                //TODO PASS IS VEST CHECKED
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