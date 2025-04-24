namespace Network.Packets
{
    public enum PacketKeys : ushort
    {
        SETUP_MOBILE = 0,
        SETUP_MOBILE_RESPONSE = 1,
        SETUP_VEST = 2,
        SETUP_VEST_RESPONSE = 3,
        SETUP_WEAPON = 4,
        SETUP_WEAPON_RESPONSE = 5,
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

    public enum Champions : ushort
    {
        ENGINEER,
        SCOUT,
        DEFENDER,
        DEMOLISHER,
        REFLECTOR,
        NINJA,
        HEALER,
        HACKER
    }

    public class CardInfo
    {
        public string ipAddress;
        public int portToListen;
        public byte gameId;
        public byte playerId;
        public Champions champion;
        public uint hexColor;
    }

    public class SetupMobile
    {
        public byte gameId;
        public byte playerId;
        public Champions champion;
    }

    public class SetupMobileResponse
    {
        public bool isCorrect;
        public string playerName;
        public bool isVestChecked;
        public bool isWeaponChecked;
    }

    public class SetupVest
    {
        public byte gameId;
        public byte playerId;
    }

    public class SetupVestResponse
    {
        public bool isCorrect;
    }

    public class SetupWeapon
    {
        public byte gameId;
        public byte playerId;
    }

    public class SetupWeaponResponse
    {
        public bool isCorrect;
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