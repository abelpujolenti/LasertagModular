#include <WiFiS3.h>
#include <Arduino_JSON.h>
#include "TestPacket.hpp"

class LaserServerConnection
{
private:
  WiFiClient TCP_client;
  char* TCP_SERVER_ADDR;
  int TCP_SERVER_PORT;


public:

  //Function will be called while return value is false
  bool ConnectToServer(const char* _WIFI_SSID, const char* _WIFI_PASSWORD, const char* _TCP_SERVER_ADDR, const int _TCP_SERVER_PORT)
  {
    //ATTEMPT TO CONNECT TO WIFI
    if (WiFi.status() == WL_NO_MODULE)
      //Communication with WiFi module failed
      return false;

    String fv = WiFi.firmwareVersion();
    if (fv < WIFI_FIRMWARE_LATEST_VERSION)
      //Upgrade firmware (?)
      return false;

    while (WiFi.begin(_WIFI_SSID, _WIFI_PASSWORD) != WL_CONNECTED)
    {
      //Failed to connect to WiFi
      return false;
    }

    //ATTEMPT TO CONNECT TO SERVER
    if (!TCP_client.connect(_TCP_SERVER_ADDR, _TCP_SERVER_PORT))
      //Failed to connect to TCP server
      return false;

    //SEND HELLO TO SERVER
    SendHello();

    //RECEIVE WELCOME FROM SERVER
    //... (process welcome data)

    TCP_SERVER_ADDR = (char*)_TCP_SERVER_ADDR;
    TCP_SERVER_PORT = (int)_TCP_SERVER_PORT;

    return true;
  }

  void Update()
  {
    ListenToPackets();
    CheckDisconnectionAndReconnection();

    //Test, remove later
    SetupVest packet* testPacket;
    testPacket.gameId = 2;
    testPacket.playerId = 2;

    SendPacket(testPacket, 2);
  }

  void SendHello()
  {
    TCP_client.write("Hello!");
    TCP_client.flush();
  }

  void CheckDisconnectionAndReconnection()
  {
    if(!TCP_client.connected())
    {
      //Connection is disconnected
      TCP_client.stop();

      //Reconnect to TCP server
      if(!TCP_client.connect(TCP_SERVER_ADDR, TCP_SERVER_PORT))
      {
        //Failed to reconnect
        return;
      }

      //Successfully reconnected
      SendHello();
    }
  }

  void ListenToPackets()
  {
    if(TCP_client.available())
    {
      GetPacket(); 
    }
  }

  void SendPacket(Packet* packet, PacketKeys packetKey)
  {
    //Each child constructs its own tailored json
    JSONVar json = packet->ClassToJson();

    //Generic stringify code
    String jsonString = JSON.stringify(json);

    //Generic ToByte code
    byte jsonBuffer[BYTE_BUFFER_SIZE - 4];
    jsonString.getBytes(jsonBuffer, BYTE_BUFFER_SIZE - 4);
  
    //Add key to byte array
    byte buffer[BYTE_BUFFER_SIZE];
  
    //Extract byte per byte
    uint32_t key = (int)packetKey;
    buffer[0] = (byte) key & 255;
    buffer[1] = (byte) key & 255 << 8;
    buffer[2] = (byte) key & 255 << 16;
    buffer[3] = (byte) key & 255 << 32;

    //Add json string
    for(int i = 4; i < BYTE_BUFFER_SIZE; i++)
    {
      buffer[i] = jsonBuffer[i-4];
    }

    //Add json to byte array
    TCP_client.write(buffer, sizeof(buffer));
  }

  Packet* GetPacket()
  {
    //Generic read code
    byte buffer[BYTE_BUFFER_SIZE];
    TCP_client.read(buffer, BYTE_BUFFER_SIZE);

    //Generic key extraction code
    uint32_t key = buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24;

    int sizeOfUint = sizeof(uint32_t);

    byte data[sizeof(buffer) - sizeOfUint];

    for (int i = 0; i < sizeof(data); i++)
    {
      data[i] = buffer[i + sizeOfUint];
    }

    Packet* packet;

    //Interpretar el buffer -> constructor directament amb el buffer -> constructor interpreta buffer
    switch(key)
    {
      case (int)PacketKeys::SETUP_VEST:
        packet = new SetupVest(data);
        break;
      case (int)PacketKeys::SETUP_VEST_RESPONSE:
        packet = new SetupVestResponse(data);
        break;
      case (int)PacketKeys::SETUP_WEAPON:
        packet = new SetupWeapon(data);
        break;
      case (int)PacketKeys::SETUP_WEAPON_RESPONSE:
        packet = new SetupWeaponResponse(data);
        break;
      case (int)PacketKeys::START_GAME:
        packet = new StartGame(data);
        break;
      case (int)PacketKeys::HIT:
        packet = new Hit(data);
        break;
      case (int)PacketKeys::HIT_RESPONSE:
        packet = new HitResponse(data);
        break;
      case (int)PacketKeys::HEAL:
        packet = new Heal(data);
        break;
      case (int)PacketKeys::END_GAME:
        packet = new EndGame(data);
        break;
    }

    packet->key = key;

    return packet;
  }
};