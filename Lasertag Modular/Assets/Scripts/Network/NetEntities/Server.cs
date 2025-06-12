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
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Network.NetEntities
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private ServerActionOutput _serverActionOutput;
        [SerializeField] private WriteNFC _writeNFC;
        
        private string _ssid;
        private string _password;
        
        private int _portToListen = 3000;
        private SocketManager _serverSocketManager;
        
        private Action _onReadCharacter = () => { };

        private int _gameId;

        private byte[] _playersId;
        
        private Characters[] _characterTeamA;
        private Characters[] _characterTeamB;

        private Dictionary<TcpSocket, byte> _playersSockets = new Dictionary<TcpSocket, byte>();
        private Dictionary<byte, string> _playersName = new Dictionary<byte, string>();
        private Dictionary<byte, bool> _playersTeam = new Dictionary<byte, bool>();
        private Dictionary<byte, IBaseAgent> _agents = new Dictionary<byte, IBaseAgent>();
        private Dictionary<byte, byte[]> _agentsChecks = new Dictionary<byte, byte[]>();
        private Dictionary<byte, Characters> _characterPerPlayer = new Dictionary<byte, Characters>();
        private Dictionary<byte, bool> _playersReadyCheck = new Dictionary<byte, bool>();

        private CardWriteInformation _cardWriteBaseInformation = new CardWriteInformation();
        private CardWriteInformation _cardWriteInformation;

        private byte _teamAPlayers = 0;
        private byte _teamBPlayers = 0;
        
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

            _cardWriteBaseInformation.portToListen = _portToListen;
            
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork)
                {
                    continue;
                }
                
                if (!_serverSocketManager.StartListener(_portToListen))
                {
                    continue;
                }
                
                Debug.Log($"Server started on ip {ip.ToString()}, on port {_portToListen})");
                        
                _serverSocketManager.ipEndPoint = new IPEndPoint(ip, _portToListen);

                _cardWriteBaseInformation.ipAddress = ip.ToString();

                _serverSocketManager.StartLoop();
                
                break;
            }
            
            _gameId = Random.Range(0, Int32.MaxValue);

            _cardWriteBaseInformation.gameId = _gameId;
            
            _writeNFC.SetCallAction(CallOnReadCharacter);
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

        public void SetWiFi(string ssid, string password)
        {
            _ssid = ssid;
            _password = password;
            _serverActionOutput.ConnectionSettuped();

            _cardWriteBaseInformation.wifi = _ssid;
            _cardWriteBaseInformation.password = _password;
        }

        public void SetupMatch(int numberOfPlayers)
        {
            _playersId = new byte[numberOfPlayers];
            _characterTeamA = new Characters[numberOfPlayers / 2];
            _characterTeamB = new Characters[numberOfPlayers / 2];
            
            _serverActionOutput.MatchSetupped();
        }

        private void CallOnReadCharacter()
        {
            _onReadCharacter();
        }

        public void PrepareCharacter(Characters character, string name, bool isTeamB)
        {
            byte newPlayerId;

            do
            {
                newPlayerId = (byte)Random.Range(0, Byte.MaxValue);

            } while (_playersId.Contains(newPlayerId));
            
            _cardWriteInformation = new CardWriteInformation
            {
                ipAddress = _cardWriteBaseInformation.ipAddress,
                portToListen = _cardWriteBaseInformation.portToListen,
                isTeamB = Convert.ToByte(isTeamB),
                gameId = _cardWriteBaseInformation.gameId,
                playerId = newPlayerId,
                character = character,
                wifi = _cardWriteBaseInformation.wifi,
                password = _cardWriteBaseInformation.password
            };

            _writeNFC.AddRecord(_cardWriteInformation);
            
            _onReadCharacter = () =>
            {
                _onReadCharacter = () => {};
                
                _playersName.Add(newPlayerId, name);
                _characterPerPlayer.Add(newPlayerId, character);
                _agentsChecks.Add(newPlayerId, new byte[CharacterManager.Instance.GetEquipmentAmount(character)]);
                _playersTeam.Add(newPlayerId, isTeamB);
                _agents.Add(newPlayerId, _serverActionOutput.SetAgent(character, name, isTeamB));

                if (!isTeamB)
                {
                    _characterTeamA[_teamAPlayers++] = character;
                }
                else
                {
                    _characterTeamB[_teamBPlayers++] = character;   
                }
                
                _serverActionOutput.UpdatePlayerMatchSettings(_characterTeamA, _characterTeamB);
            };
        }

        private void SubscribeToLobbyPackets(TcpSocket socket)
        {
            SubscribeToSetupMobile(socket);
            SubscribeToSetupVest(socket);
            SubscribeToSetupWeapon(socket);
            SubscribeToSetupGrenade(socket);
            SubscribeToSetupCar(socket);
            SubscribeToPlayerReadyToPlay(socket);
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
                
                _playersSockets.Add(socket, setupMobile.playerId);
                _playersReadyCheck.Add(setupMobile.playerId, false);
                
                SetupCharacterResponse setupResponse = new SetupCharacterResponse
                {
                    character = setupMobile.character,
                    playerName = _playersName[setupMobile.playerId],
                    isTeamB = _playersTeam[setupMobile.playerId],
                    setupResponse = UpdateCheck(setupMobile.playerId, Equipment.MOBILE)
                };
                
                socket.SendPacket(PacketKeys.SETUP_CHARACTER_RESPONSE, setupResponse);

                Action<List<TcpSocket>> action = (sockets) =>
                {
                    CheckedPlayersAmount checkedPlayersAmount = new CheckedPlayersAmount
                    {
                        checkedPlayersAmount = (byte)_playersReadyCheck.Count
                    };
                    
                    foreach (TcpSocket socket in sockets)
                    {
                        socket.SendPacket(PacketKeys.CHECKED_PLAYERS_AMOUNT, checkedPlayersAmount);
                    }
                };

                _serverSocketManager.GetSocketsAsync(action);
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

        private void SubscribeToPlayerReadyToPlay(TcpSocket socket)
        {
            socket.Subscribe(PacketKeys.PLAYER_READY_TO_PLAY, (bytes) =>
            {
                byte playerId = _playersSockets[socket];
                
                bool isReady = _playersReadyCheck[playerId];

                _playersReadyCheck[playerId] = !isReady;

                Action<List<TcpSocket>> action = (sockets) =>
                {
                    ReadyPlayersAmount readyPlayersAmount = new ReadyPlayersAmount();

                    foreach (bool isChecked in _playersReadyCheck.Values)
                    {
                        if (!isChecked)
                        {
                            continue;
                        }

                        readyPlayersAmount.readyPlayersAmount++;
                    }
                    
                    foreach (TcpSocket socket in sockets)
                    {
                        socket.SendPacket(PacketKeys.READY_PLAYERS_AMOUNT, readyPlayersAmount);
                    }
                };

                _serverSocketManager.GetSocketsAsync(action);
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