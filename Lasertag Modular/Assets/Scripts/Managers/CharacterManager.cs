using System;
using System.Collections.Generic;
using Network.Packets;
using UnityEngine;

namespace Managers
{
    public class CharacterManager : MonoBehaviour
    {
        private static CharacterManager _instance;
        
        public static CharacterManager Instance => _instance;

        private Dictionary<Characters, Dictionary<Equipment, byte>> _equipmentOrder;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            
            InstantiateDictionaries();
            
            DontDestroyOnLoad(gameObject);
        }

        private void InstantiateDictionaries()
        {
            Dictionary<Equipment, byte> engineerEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> scoutEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> defenderEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> demolisherEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> reflectorEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> ninjaEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> healerEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };
            
            Dictionary<Equipment, byte> hackerEquipmentOrder = new Dictionary<Equipment, byte>
            {
                { Equipment.VEST , 0},
                { Equipment.WEAPON , 1},
                { Equipment.MOBILE , 2},
            };

            _equipmentOrder = new Dictionary<Characters, Dictionary<Equipment, byte>>
            {
                { Characters.ENGINEER, engineerEquipmentOrder },
                { Characters.SCOUT, engineerEquipmentOrder },
                { Characters.DEFENDER, engineerEquipmentOrder },
                { Characters.DEMOLISHER, engineerEquipmentOrder },
                { Characters.REFLECTOR, engineerEquipmentOrder },
                { Characters.NINJA, engineerEquipmentOrder },
                { Characters.HEALER, engineerEquipmentOrder },
                { Characters.HACKER, engineerEquipmentOrder },
            };
        }

        public int GetEquipmentOrder(Characters character, Equipment equipment)
        {
            try
            {
                return _equipmentOrder[character][equipment];
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}