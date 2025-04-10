namespace Network.Packets
{
    public enum PacketKeys
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
        END_GAME = 20,
    }

    public enum Champions
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

    public class SetupMobile
    {
        public ushort gameId;
        public ushort playerId;
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
        public ushort gameId;
        public ushort playerId;
    }

    public class SetupVestResponse
    {
        public bool isCorrect;
    }

    public class SetupWeapon
    {
        public ushort gameId;
        public ushort playerId;
    }

    public class SetupWeaponResponse
    {
        public bool isCorrect;
    }

    public class CheckedPlayersAmount
    {
        public ushort checkedPlayersAmount;
    }

    public class ReadyPlayersAmount
    {
        public ushort readyPlayersAmount;
    }

    public class Hit
    {
        
    }

    public class HitResponse
    {
        
    }

    public class Heal
    {
        
    }

    public class EndGame
    {
        
    }
}