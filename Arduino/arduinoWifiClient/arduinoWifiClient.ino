#include <WiFiS3.h>
#include <Arduino_JSON.h>
#include "TestPacket.hpp"

const char* WIFI_SSID = "ENTI";          // CHANGE TO YOUR WIFI SSID
const char* WIFI_PASSWORD = "Entialum#1714";       // CHANGE TO YOUR WIFI PASSWORD
const char* TCP_SERVER_ADDR = "10.40.2.35";  // CHANGE TO TCP SERVER'S IP ADDRESS
const int TCP_SERVER_PORT = 3000;
const int BYTE_BUFFER_SIZE = 256;

WiFiClient TCP_client;

void setup() {
  Serial.begin(9600);


  Serial.println("Arduino: TCP CLIENT");


  // check for the WiFi module:
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    // don't continue
    while (true)
      ;
  }


  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }


  Serial.print("Attempting to connect to SSID: ");
  Serial.println(WIFI_SSID);
  // attempt to connect to WiFi network:
  while (WiFi.begin(WIFI_SSID, WIFI_PASSWORD) != WL_CONNECTED) {
    delay(1000);  // wait 10 seconds for connection:
  }


  Serial.print("Connected to WiFi ");
  Serial.println(WIFI_SSID);


  // connect to TCP server
  if (TCP_client.connect(TCP_SERVER_ADDR, TCP_SERVER_PORT)) {
    Serial.println("Connected to TCP server");
    TCP_client.write("Hello!");  // send to TCP Server
    TCP_client.flush();
  } else {
    Serial.println("Failed to connect to TCP server");
  }

  Packet* packets[2];
  SetupMobile* setupPacket = new SetupMobile();
  packets[0] = setupPacket;
  SetupMobileResponse* setupPacket2 = (SetupMobileResponse*) packets[0];

  
  Serial.println(setupPacket2);

  while(true)
  {

  }
}


void loop() {
  // Read data from server and print them to Serial
  if (TCP_client.available()) {
    //Serial.println("Hello");
   
    byte buffer[256];


    TCP_client.read(buffer, 256);


    //uint32_t key = Byte.toUnsignedInt(buffer[0]);
    uint32_t key = buffer[0] | buffer[1] << 8 | buffer[2] << 16 | buffer[3] << 24;
    Serial.println(key);


    int sizeOfUint = sizeof(uint32_t);


    byte data[sizeof(buffer) - sizeOfUint];


    for (int i = 0; i < sizeof(data); i++)
    {
      data[i] = buffer[i + sizeOfUint];
      Serial.println(data[i]);
    }
   
    ProcessPacket(key, data);
  }
 


  if (!TCP_client.connected()) {
    Serial.println("Connection is disconnected");
    TCP_client.stop();


    // reconnect to TCP server
    if (TCP_client.connect(TCP_SERVER_ADDR, TCP_SERVER_PORT)) {
      Serial.println("Reconnected to TCP server");
      TCP_client.write("Hello!");  // send to TCP Server
      TCP_client.flush();
    } else {
      Serial.println("Failed to reconnect to TCP server");
      delay(1000);
    }
  }
}


void ProcessPacket(int key, byte data[])
{
  switch(key)
  {
    case -1:
      ProcessTestPacket(data);
      break;
  }
}


Test ProcessTestPacket(byte data[])
{
  String json = (char*)data;


    JSONVar myObject = JSON.parse(json);


    if (myObject.hasOwnProperty("sos"))
    {
      Serial.print("sos = ");


      Serial.println((String) myObject["sos"]);
    }


    if (myObject.hasOwnProperty("puto"))
    {
      Serial.print("puto = ");


      Serial.println((int) myObject["puto"]);
    }


    Test newTest = Test((String) myObject["sos"], (int) myObject["puto"]);


    newTest.PrintThings();
    SendTestPacket(newTest);
}


void SendTestPacket(Test test)
{
  //Create JSON
  JSONVar myObject;
  myObject["sos"] = test.sos;
  myObject["puto"] = test.puto;
  String jsonString = JSON.stringify(myObject);


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
