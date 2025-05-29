#include "LaserServerConnection.hpp"

const char* WIFI_SSID = "IORouter";
const char* WIFI_PASSWORD = "00000000";
const char* TCP_SERVER_ADDR = "192.168.1.26";
const int TCP_SERVER_PORT = 3000;

LaserServerConnection laserServerConnection;

//HARDWARE VARIABLES
const int BUZZER_PIN = 9;       //TODO: Review number
int _lengthAlarmSound = 1000;

//GAME LOGIC VARIABLES

void setup()
{
  Serial.begin(115200);
  Serial.println("Arduino: TCP CLIENT");

  WifiStatus status;
  do
  {
    status = laserServerConnection.ConnectToServer(WIFI_SSID, WIFI_PASSWORD, TCP_SERVER_ADDR, TCP_SERVER_PORT);
    Serial.println((int)status);
  } while(status != WifiStatus::ALL_OK);
}


void loop()
{
  Serial.println("Update");
  laserServerConnection.Update(); 
  ProcessPackets();
}


//NETWORK FUNCTIONS
void ProcessPackets()
{
    Packet* packet = nullptr;

    if(laserServerConnection.TryGetPacketWithId((int)PacketKeys::PLANT_BOMB_RESPONSE, packet))
    {
        PlantBombRequest* pbr = (PlantBombRequest*)packet;

        //Do stuff with packet
        Serial.println(pbr->site);
    }

}
//GAME LOGIC FUNCTIONS
//Countdown