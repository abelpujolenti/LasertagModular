#include <Arduino_JSON.h>

#define BYTE_BUFFER_SIZE 256

//Keys
enum class PacketKeys
{
  //SETUP_MOBILE = 0,
  //SETUP_MOBILE_RESPONSE = 1,
  SETUP_VEST = 2,
  SETUP_VEST_RESPONSE = 3,
  SETUP_WEAPON = 4,
  SETUP_WEAPON_RESPONSE = 5,
  //PLAYER_READY_TO_CHECKED = 6,
  //PLAYER_READY_TO_PLAY = 7,
  //CHECKED_PLAYERS_AMOUNT = 8,
  //READY_PLAYERS_AMOUNT = 9,
  START_GAME = 10,
  HIT = 11,
  HIT_RESPONSE = 12,
  HEAL = 13,
  END_GAME = 20,
};

enum class Champions
{
  ENGINEER,
  SCOUT,
  DEFENDER,
  DEMOLISHER,
  REFLECTOR,
  NINJA,
  HEALER,
  HACKER
};

class Packet
{
public:
  uint32_t key;

  Packet(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) { }
  Packet() { }
  ~Packet() { }

  JSONVar BufferToJson(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)])
  {
    String jsonString = (char*)data;
    JSONVar jsonObject = JSON.parse(jsonString);
    return jsonObject;
  }

  virtual JSONVar ClassToJson() = 0;
};


class SetupVest : public Packet
{
public:
  unsigned short gameId;
  unsigned short playerId;

  SetupVest(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    gameId = jsonObject["gameId"];
    playerId =jsonObject["playerId"];
  }

  SetupVest(unsigned short _gameId, unsigned short _playerId)
  {
    gameId = _gameId;
    playerId = _playerId;
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    jsonObject["gameId"] = gameId;
    jsonObject["playerId"] = playerId;
    return jsonObject;
  }
};

class SetupVestResponse : public Packet
{
public:
  bool isCorrect;

  SetupVestResponse(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    isCorrect = jsonObject["isCorrect"];
  }

  SetupVestResponse(bool _isCorrect)
  {
    isCorrect = _isCorrect;
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    jsonObject["isCorrect"] = isCorrect;
    return jsonObject;
  }
};

class SetupWeapon : public Packet
{
public:
  unsigned short gameId;
  unsigned short playerId;

  SetupWeapon(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    gameId = jsonObject["gameId"];
    playerId =jsonObject["playerId"];
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    jsonObject["gameId"] = gameId;
    jsonObject["playerId"] = playerId;
    return jsonObject;
  }
};

class SetupWeaponResponse : public Packet
{
public:
  bool isCorrect;

  SetupWeaponResponse(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    isCorrect = jsonObject["isCorrect"];
  }

JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    jsonObject["isCorrect"] = isCorrect;
    return jsonObject;
  }
};

class StartGame : public Packet
{
public:
  StartGame(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    //Assign variables
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    //Assign variables
    return jsonObject;
  }
};

class Hit : public Packet
{
public:
  Hit(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    //Assign variables
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    //Assign variables
    return jsonObject;
  }
};

class HitResponse : public Packet
{
public:
  HitResponse(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    //Assign variables
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    //Assign variables
    return jsonObject;
  }
};

class Heal : public Packet
{
public:
  Heal(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    //Assign variables
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    //Assign variables
    return jsonObject;
  }
};

class EndGame : public Packet
{
public:
  EndGame(byte data[BYTE_BUFFER_SIZE - sizeof(uint32_t)]) : Packet(data)
  {
    JSONVar jsonObject = BufferToJson(data);
    //Assign variables
  }

  JSONVar ClassToJson() override
  {
    JSONVar jsonObject;
    //Assign variables
    return jsonObject;
  }
};