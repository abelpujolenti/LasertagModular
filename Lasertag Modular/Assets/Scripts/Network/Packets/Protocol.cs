namespace Network.Packets
{
    public enum PacketKeys : byte
    {
        SETUP_MOBILE = 0,
        SETUP_VEST = 1,
        SETUP_WEAPON = 2,
        SETUP_GRENADE = 3,
        SETUP_CAR = 4,
        SETUP_CHARACTER_RESPONSE = 5,
        SETUP_RESPONSE = 6,
        PLAYER_READY_TO_PLAY = 7,
        CHECKED_PLAYERS_AMOUNT = 8,
        READY_PLAYERS_AMOUNT = 9,
        START_GAME = 10,
        HIT = 11,
        HIT_RESPONSE = 12,
        HEAL = 13,
        HEAL_RESPONSE = 14,
        END_GAME = 20
    }

    public enum Characters : byte
    {
        NONE,
        ENGINEER,
        SCOUT,
        DEFENDER,
        DEMOLISHER,
        REFLECTOR,
        NINJA,
        HEALER,
        HACKER
    }

    public enum Equipment : byte
    {
        MOBILE = 1,
        VEST = 2,
        WEAPON = 4,
        GRENADE = 8,
        CAR = 16
    }

    public class CardInformation
    {
        public string ipAddress;
        public int portToListen;
        public byte gameId;
        public byte playerId;
        public Characters character;
    }

    public class SetupMobile
    {
        public byte gameId;
        public byte playerId;
        public Characters character;
    }

    public class SetupVest
    {
        public byte gameId;
        public byte playerId;
    }

    public class SetupWeapon
    {
        public byte gameId;
        public byte playerId;
    }

    public class SetupGrenade
    {
        public byte gameId;
        public byte playerId;
    }

    public class SetupCar
    {
        public byte gameId;
        public byte playerId;
        public string ipAddress;
        public int portToListen;
    }

    public class SetupCharacterResponse
    {
        public Characters character;
        public string playerName;
        public bool isTeamB;
        public byte[] setupResponse;
    }

    public class SetupResponse
    {
        public byte[] setupResponse;
    }

    public class CheckedPlayersAmount
    {
        public byte checkedPlayersAmount;
    }

    public class ReadyPlayersAmount
    {
        public byte readyPlayersAmount;
    }

    public class Hit
    {
        public byte device;
        public byte player;
        public byte zone;
    }

    public class HitResponse
    {
        public byte currentLives;
        public byte zone;
    }

    public class Heal
    {
        public byte healAmount;
        public byte player;
    }

    public class HealResponse
    {
        public byte currentLife;
    }
}