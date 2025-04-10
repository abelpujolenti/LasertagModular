#include <Arduino_JSON.h>

//Keys
enum class PacketKeys
{
  TEST = -1,
  SETUP_MOBILE = 0,
  SETUP_MOBILE_RESPONSE = 1,
  SETUP_VEST = 2,
  SETUP_VEST_RESPONSE = 3,
  SETUP_WEAPON = 4,
  SETUP_WEAPON_RESPONSE = 5,
  PLAYER_READY_TO_CHECKED = 6,
  PLAYER_READY_TO_PLAY = 7,
  CHECKED_PLAYERS_AMOUNT = 8,
  READY_PLAYERS_AMOUNT = 9,
  START_GAME = 10,
  HIT = 11,
  HIT_RESPONSE = 12,
  HEAL = 13,
  END_GAME = 20,
};

class Test
{
  public:
    String sos;
    int puto;
    Test(String _sos, int _puto) { sos = _sos; puto = _puto; }
    void PrintThings() {Serial.println("THINGS INSIDE PACKET ARE: " + sos + ", " + puto);}
};

class Packet
{
  uint32_t key;

  //Passa variables de cada packet específic a json var
  virtual JSONVar ToJson() = 0;
};

class SetupMobile : public Packet
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class SetupMobileResponse : public Packet
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class SetupVest
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class SetupVestResponse
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class SetupWeapon
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class SetupWeaponResponse
{
   JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    }; 
};

class PlayerReadyToChecked
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class PlayerReadyToPlay
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class CheckedPlayersAmount
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class ReadyPlayersAmount
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class StartGame
{
   JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    }; 
};

class Hit
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class HitResponse
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class Heal
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};

class EndGame
{
    JSONVar ToJson() override
    {
      JSONVar json;
      //...
      return json;
    };
};