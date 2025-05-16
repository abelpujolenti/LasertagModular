using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Interface.Agent;
using Managers;
using Network.Packets;
using Network.Sockets;
using Stream;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Network.NetEntities
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private ServerActionOutput _serverActionOutput;
        
        private int _portToListen = 3000;
        private SocketManager _serverSocketManager;
        
        private Func<CardInformation> _onReadCharacter;

        private byte _gameId;

        private byte[] _playersId;
        
        private Characters[] _characterTeamA;
        private Characters[] _characterTeamB;

        private Dictionary<byte, IBaseAgent> _agents;
        private Dictionary<byte, byte[]> _agentsChecks;
        private Dictionary<byte, Characters> _characterPerPlayer;

        private void Start()
        {
            _serverSocketManager = new SocketManager((socket) =>
            {
                Debug.Log("Socket Connected: " + socket.GetRemoteAddress());
                
                socket.SubscribeOnDisconnect((socketDisconnected) =>
                {
                    Debug.Log("Socket Disconnected: " + socketDisconnected.GetRemoteAddress());
                });
                
                SubscribeToLobbyPackets(socket);
            });

            _portToListen = FindAvailablePort(_portToListen);

            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    if (_serverSocketManager.StartListener(_portToListen))
                    {
                        Debug.Log($"Server started on ip {ip.ToString()}, on port {_portToListen})");
                        
                        _serverSocketManager.ipEndPoint = new IPEndPoint(ip, _portToListen);

                        _serverSocketManager.StartLoop();
                        return;
                    }
                }
            }
            
            _gameId = (byte)Random.Range(0, Byte.MaxValue);
        }
        
        private int FindAvailablePort(int startPort)
        {
            int testPort = startPort;
            
            while (!IsPortAvailable(testPort))
            {
                testPort++;
            }

            return testPort;
        }

        private bool IsPortAvailable(int port)
        {
            try
            {
                TcpListener testListener = new TcpListener(IPAddress.Any, port);
                testListener.Start();
                testListener.Stop();
                return true; // Port is available
            }
            catch (SocketException)
            {
                return false; // Port is in use
            }
        }
        
        public void SetupMatch(int numberOfPlayers)
        {
            _playersId = new byte[numberOfPlayers];
            _characterTeamA = new Characters[numberOfPlayers / 2];
            _characterTeamB = new Characters[numberOfPlayers / 2];
            
            _serverActionOutput.MatchSetupped();
        }

        public void PrepareCharacter(Characters character, string name, bool isTeamB)
        {
            _onReadCharacter = () =>
            {
                byte newPlayerId;

                do
                {
                    newPlayerId = (byte)Random.Range(0, Byte.MaxValue);

                } while (_playersId.Contains(newPlayerId));
            
                CardInformation cardInformation = new CardInformation
                {
                    ipAddress = _serverSocketManager.ipEndPoint.Address.ToString(),
                    portToListen = _serverSocketManager.ipEndPoint.Port,
                    gameId = _gameId,
                    playerId = newPlayerId,
                    character = character
                };

                _onReadCharacter = () => new CardInformation();
                
                _agents[newPlayerId] = _serverActionOutput.SetAgent(character, name, isTeamB);
                
                _serverActionOutput.UpdatePlayerMatchSettings(_characterTeamA, _characterTeamB);

                return cardInformation;
            };
        }

        private void SubscribeToLobbyPackets(TcpSocket socket)
        {
            SubscribeToSetupMobile(socket);
            SubscribeToSetupVest(socket);
            SubscribeToSetupWeapon(socket);
            SubscribeToSetupGrenade(socket);
            SubscribeToSetupCar(socket);
        }

        private void UnsubscribeToLobbyPacket(TcpSocket socket)
        {
            socket.Unsubscribe(PacketKeys.SETUP_MOBILE);
            socket.Unsubscribe(PacketKeys.SETUP_VEST);
            socket.Unsubscribe(PacketKeys.SETUP_WEAPON);
        }

        private void SubscribeToInGamePackets(TcpSocket socket)
        {
            SubscribeToHit(socket);
        }

        private void UnsubscribeToInGamePackets(TcpSocket socket)
        {
            socket.Unsubscribe(PacketKeys.HIT);
        }

        private byte[] UpdateCheck(byte playerId, Equipment equipment)
        {
            Characters character = _characterPerPlayer[playerId];

            byte[] agentCheck = _agentsChecks[playerId];
            
            int position = CharacterManager.Instance.GetEquipmentOrder(character, equipment);

            if (position == -1)
            {
                return agentCheck;
            }

            agentCheck[position] = 1;
                
            _agentsChecks[playerId] = agentCheck;
                
            _agents[playerId].CheckState(agentCheck);

            return agentCheck;
        }

        private void SubscribeToSetupMobile(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_MOBILE, (bytes) =>
            {
                SetupMobile setupMobile = bytes.ByteArrayToObjectT<SetupMobile>();
                
                SetupResponse setupResponse = new SetupResponse
                {
                    setupResponse = UpdateCheck(setupMobile.playerId, Equipment.MOBILE)
                };
                
                socket.SendPacket(PacketKeys.SETUP_RESPONSE, setupResponse);
            });
        }

        private void SubscribeToSetupVest(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_VEST, (bytes) => 
            {
                SetupVest setupMobile = bytes.ByteArrayToObjectT<SetupVest>();
                
                SetupResponse setupResponse = new SetupResponse
                {
                    setupResponse = UpdateCheck(setupMobile.playerId, Equipment.VEST)
                };
                
                socket.SendPacket(PacketKeys.SETUP_RESPONSE, setupResponse);
            });
        }

        private void SubscribeToSetupWeapon(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_WEAPON, (bytes) => 
            {
                SetupWeapon setupMobile = bytes.ByteArrayToObjectT<SetupWeapon>();
                
                SetupResponse setupResponse = new SetupResponse
                {
                    setupResponse = UpdateCheck(setupMobile.playerId, Equipment.WEAPON)
                };
                
                socket.SendPacket(PacketKeys.SETUP_RESPONSE, setupResponse);
            });
        }

        private void SubscribeToSetupGrenade(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_GRENADE, (bytes) => 
            {
                SetupGrenade setupMobile = bytes.ByteArrayToObjectT<SetupGrenade>();
                
                SetupResponse setupResponse = new SetupResponse
                {
                    setupResponse = UpdateCheck(setupMobile.playerId, Equipment.GRENADE)
                };
                
                socket.SendPacket(PacketKeys.SETUP_RESPONSE, setupResponse);
            });
        }

        private void SubscribeToSetupCar(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.SETUP_CAR, (bytes) => 
            {
                SetupCar setupCar = bytes.ByteArrayToObjectT<SetupCar>();

                IDictionary<byte, TcpSocket> _playerIds = new Dictionary<byte, TcpSocket>();    //TODO: Erase this when real dictionary is created

                foreach (var pair in _playerIds ){

                    if (pair.Key == setupCar.playerId)
                    {
                        pair.Value.SendPacket(PacketKeys.SETUP_CAR, setupCar);
                    }

                }

            });
        }

        private void SubscribeToHit(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.HIT, (bytes) =>
            {
                
            });
        }
    }
}