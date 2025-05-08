namespace Network.Packets
{
    public enum PacketKeys : byte
    {
        SETUP_MOBILE = 0,
        SETUP_VEST = 1,
        SETUP_WEAPON = 2,
        SETUP_GRENADE = 3,
        SETUP_CAR = 4,
        SETUP_RESPONSE = 5,
        PLAYER_READY_TO_PLAY = 6,
        CHECKED_PLAYERS_AMOUNT = 7,
        READY_PLAYERS_AMOUNT = 8,
        START_GAME = 9,
        HIT = 10,
        HIT_RESPONSE = 11,
        HEAL = 12,
        HEAL_RESPONSE = 13,
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

    public class CardInformation
    {
        public string ipAddress;
        public int portToListen;
        public byte gameId;
        public byte playerId;
        public Characters character;
        public string hexColor;
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
    }

    public class SetupResponse
    {
        public string playerName;
        public byte setupResponse;
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