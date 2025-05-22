using System;
using System.Collections.Generic;
using System.Net;
using Interface.Agent;
using Network.Packets;
using Network.Sockets;
using Stream;
using UnityEngine;

//typdef

namespace Network.NetEntities
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private ClientActionOutput _clientActionOutput;
        
        [SerializeField] private string _ipAddress;
        [SerializeField] private int _portToListen;
        [SerializeField] private int _triesToFindPort;

        private CardInformation _cardInformation;

        private TcpSocket _socketWithServer;
        
        private SocketManager _serverSocketManager;

        private IBaseAgent _agent;

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
            SubscribeToSetupCharacterResponse();
            SubscribeToSetupResponse();
            SubscribeToPlayerReadyToPlay();
            SubscribeToCheckedPlayersAmount();
            SubscribeToReadyPlayersAmount();
            SubscribeToStartGame();
        }

        private void UnsubscribeToLobbyPackets()
        {
            _socketWithServer.Unsubscribe(PacketKeys.SETUP_CHARACTER_RESPONSE);
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

            switch (_cardInformation.character)
            {
                case Characters.ENGINEER:
                    SetupEngineer();
                    break;
                case Characters.SCOUT:
                    SetupScout();
                    break;
                case Characters.DEFENDER:
                    SetupDefender();
                    break;
                case Characters.DEMOLISHER:
                    SetupDemolisher();
                    break;
                case Characters.REFLECTOR:
                    SetupReflector();
                    break;
                case Characters.NINJA:
                    SetupNinja();
                    break;
                case Characters.HEALER:
                    SetupHealer();
                    break;
                case Characters.HACKER:
                    SetupHacker();
                    break;
                default:
                    break;
            }
        }

        private void UnsubscribeToInGamePackets()
        {
            _socketWithServer.Unsubscribe(PacketKeys.HIT_RESPONSE);
            _socketWithServer.Unsubscribe(PacketKeys.HEAL);
        }

        private void SubscribeToSetupCharacterResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_CHARACTER_RESPONSE, (bytes) =>
            {
                SetupCharacterResponse setupResponse = bytes.ByteArrayToObjectT<SetupCharacterResponse>();

                //_clientActionOutput.PlayerConfirmed(setupResponse.character, setupResponse.playerName, setupResponse.isTeamB);
            });
        }

        private void SubscribeToSetupResponse()
        {
            _socketWithServer.Subscribe(PacketKeys.SETUP_RESPONSE, (bytes) =>
            {
                SetupResponse setupResponse = bytes.ByteArrayToObjectT<SetupResponse>();
                
                _agent.CheckState(setupResponse.setupResponse);
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

                //_clientActionOutput.UpdateCheckedPlayers(checkedPlayersAmount.checkedPlayersAmount);
            });
        }

        private void SubscribeToReadyPlayersAmount()
        {
            _socketWithServer.Subscribe(PacketKeys.READY_PLAYERS_AMOUNT, (bytes) =>
            {
                ReadyPlayersAmount readyPlayersAmount = bytes.ByteArrayToObjectT<ReadyPlayersAmount>();
                
                //_clientActionOutput.UpdateAllPlayers(readyPlayersAmount.readyPlayersAmount);
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

        private void SubscribeToSetupCar()
        {

            _socketWithServer.Subscribe(PacketKeys.SETUP_CAR, (bytes) =>
            {
                SetupCar setupCar = bytes.ByteArrayToObjectT<SetupCar>();

                GameObject gameObject = new GameObject("CarVideo");
                VideoReceiver vr = gameObject.AddComponent<VideoReceiver>();

                vr.Connect(IPAddress.Parse(setupCar.ipAddress), setupCar.portToListen);

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

        private void SetupEngineer()
        {
            SubscribeToSetupCar();
        }

        private void SetupScout()
        {

        }

        private void SetupDefender()
        {

        }

        private void SetupDemolisher()
        {

        }

        private void SetupReflector()
        {

        }

        private void SetupNinja()
        {

        }

        private void SetupHealer()
        {

        }

        private void SetupHacker()
        {

        }
    }
}