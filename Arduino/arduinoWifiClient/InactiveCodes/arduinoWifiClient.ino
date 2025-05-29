#include "LaserServerConnection.hpp"

const char* WIFI_SSID = "IORouter";
const char* WIFI_PASSWORD = "00000000";
const char* TCP_SERVER_ADDR = "192.168.1.26";
const int TCP_SERVER_PORT = 3000;

LaserServerConnection laserServerConnection;

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
}
