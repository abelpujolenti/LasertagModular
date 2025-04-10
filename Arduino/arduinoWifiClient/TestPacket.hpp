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

class SetupMobile
{
    
};

class SetupMobileResponse
{
    
};

class SetupVest
{
    
};

class SetupVestResponse
{
    
};

class SetupWeapon
{
    
};

class SetupWeaponResponse
{
    
};

class PlayerReadyToChecked
{
    
};

class PlayerReadyToPlay
{
    
};

class CheckedPlayersAmount
{
    
};

class ReadyPlayersAmount
{
    
};

class StartGame
{
    
};

class Hit
{
    
};

class HitResponse
{
    
};

class Heal
{
    
};

class EndGame
{
    
};