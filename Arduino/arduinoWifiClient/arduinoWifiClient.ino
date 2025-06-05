#include <WiFiS3.h>
#include <Arduino_JSON.h>
#include "LaserServerConnection.hpp"
#include "BuzzerHelper.hpp"

//NETWORK VARIABLES
const char* WIFI_SSID = "IORouter";
const char* WIFI_PASSWORD = "00000000";
const char* TCP_SERVER_ADDR = "192.168.1.51";
const int TCP_SERVER_PORT = 3000;
LaserServerConnection laserServerConnection;

//HARDWARE VARIABLES
const int CONNECTOR_PIN = 7;
const int BUZZER_PIN = 8; 

//GAME LOGIC VARIABLES
//Buzzer alarm
BuzzerHelper buzzerComponent;
float buzzerMaxTimeActive = 500.f;

//Connection to site
bool isConnectedToSite = false;

void setup()
{
  Serial.begin(115200);
  Serial.println("Arduino: TCP CLIENT");
  InitPins();


  WifiStatus status;
  do
  {
    status = laserServerConnection.ConnectToServer(WIFI_SSID, WIFI_PASSWORD, TCP_SERVER_ADDR, TCP_SERVER_PORT);
    Serial.println((int)status);
  } while(status != WifiStatus::ALL_OK);
  


  //TEST
  //buzzerComponent.Buzz(buzzerMaxTimeActive, 10);
}


void loop()
{
  //Serial.println("Update");
  laserServerConnection.Update(); 

  ProcessPackets();

/*
  //Connection to bomb site
  int connectToSiteResult = CheckConnectedToSite();
  if(connectToSiteResult == 0 && isConnectedToSite)
    HandleDisconnectionToSite();
  else if(connectToSiteResult == 1 && !isConnectedToSite)
    HandleConnectionToSite();

  //Update buzzer
  buzzerComponent.Update();
  */
}


//NETWORK FUNCTIONS
void ProcessPackets()
{
    Packet* packet = nullptr;
    Serial.println("Processing");

    if(laserServerConnection.TryGetPacketWithId((int)PacketKeys::PLANT_BOMB_REQUEST, packet))
    {
        PlantBombRequest* pbr = (PlantBombRequest*)packet;

        //Do stuff with packet
        Serial.println(pbr->site);
    }

}

void InitPins()
{
  pinMode(CONNECTOR_PIN, INPUT);
  buzzerComponent.Init(BUZZER_PIN);
}

//GAME LOGIC FUNCTIONS

//Connection to bomb site
int CheckConnectedToSite()
{
  int bombConnectorSignal = digitalRead(CONNECTOR_PIN);

  if(bombConnectorSignal == LOW)
    return 0;

  if(bombConnectorSignal == HIGH)
    return 1;

  return -1;
}

void HandleConnectionToSite()
{
  //TODO
  isConnectedToSite = true;
}

void HandleDisconnectionToSite()
{
  //TODO
  isConnectedToSite = false;
}