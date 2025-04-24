#include <WiFiS3.h>
#include <Arduino_JSON.h>
#include "LaserServerConnection.hpp"

const char* WIFI_SSID = "ENTI";
const char* WIFI_PASSWORD = "Entialum#1714";
const char* TCP_SERVER_ADDR = "10.40.2.134";
const int TCP_SERVER_PORT = 3000;

LaserServerConnection laserServerConnection;

void setup()
{
  Serial.begin(115200);
  Serial.println("Arduino: TCP CLIENT");

  while(!laserServerConnection.ConnectToServer(WIFI_SSID, WIFI_PASSWORD, TCP_SERVER_ADDR, TCP_SERVER_PORT))
  {
    Serial.println("Trying to connect");
  }
}


void loop()
{
  laserServerConnection.Update(); 
}
