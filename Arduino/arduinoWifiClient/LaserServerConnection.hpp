#include <WiFiS3.h>
#include <Arduino_JSON.h>
#include "TestPacket.hpp"

class LaserServerConnection
{
private:
  WiFiClient TCP_client;

public:

  //Function will be called while return value is false
  bool ConnectToServer(const char* WIFI_SSID, const char* WIFI_PASSWORD, const char* TCP_SERVER_ADDR, const int TCP_SERVER_PORT)
  {
    //Attempt to connect to wifi -> fail: return false

    //Attempt to connect to server -> fail: return false

    //Send "Hello" to server

    //Receive "Welcome" from server

    return true;
  }

  void SendPacket(Packet* packet)
  {
    //Each child constructs its own tailored json
    JSONVar json = packet->ToJson();

    //Generic stringify code
    String jsonString = JSON.stringify(myObject);

    //Generic ToByte code
    byte jsonBuffer[BYTE_BUFFER_SIZE - 4];
    jsonString.getBytes(jsonBuffer, BYTE_BUFFER_SIZE - 4);
  
    //Add key to byte array
    byte buffer[BYTE_BUFFER_SIZE];
  
    //Extract byte per byte
    uint32_t key = (int)PacketKeys::TEST;
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
    byte buffer[256];
    TCP_client.read(buffer, 256);

    //Generic key extraction code
    uint32_t key = buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24;

    int sizeOfUint = sizeof(uint32_t);

    byte data[sizeof(buffer) - sizeOfUint];

    for (int i = 0; i < sizeof(data); i++)
    {
      data[i] = buffer[i + sizeOfUint];
    }

    Packet* packet;

    switch(key)
    {
      case SETUP_MOBILE:
        //...
        break;
      case SETUP_MOBILE_RESPONSE:
        //...
        break;
      case SETUP_VEST:
        //...
        break;
      case SETUP_VEST_RESPONSE:
        //...
        break;
      case SETUP_WEAPON:
        //...
        break;
      case SETUP_WEAPON_RESPONSE:
        //...
        break;
      case PLAYER_READY_TO_CHECKED:
        //...
        break;
      case PLAYER_READY_TO_PLAY:
        //...
        break;
      case CHECKED_PLAYERS_AMOUNT:
  READY_PLAYERS_AMOUNT = 9,
  START_GAME = 10,
  HIT = 11,
  HIT_RESPONSE = 12,
  HEAL = 13,
  END_GAME = 20,
    }
    ProcessPacket(key, data);


    //return?
  }
};