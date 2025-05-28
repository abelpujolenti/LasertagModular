using System;
using System.Collections.Generic;
using Interface.Agent;
using Network.Packets;
using UnityEngine;

namespace Factory
{
    public class CharacterFactory : MonoBehaviour
    {
        private static CharacterFactory _instance;

        public static CharacterFactory Instance => _instance;

        private Dictionary<Characters, Func<IBaseAgent, IBaseAgent>> _agentCreatorFuncs; 

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            
            InitializeDictionaries();
            
            DontDestroyOnLoad(gameObject);
        }

        private void InitializeDictionaries()
        {
            _agentCreatorFuncs = new Dictionary<Characters, Func<IBaseAgent, IBaseAgent>>
            {
                { Characters.ENGINEER , CreateEngineer},
                { Characters.SCOUT , CreateScout},
                { Characters.DEFENDER , CreateDefender},
                { Characters.DEMOLISHER , CreateDemolisher},
                { Characters.REFLECTOR , CreateReflector},
                { Characters.NINJA , CreateNinja},
                { Characters.HEALER , CreateHealer},
                { Characters.HACKER , CreateHacker}
            };
        }

        public IBaseAgent CreateAgent(Characters character, IBaseAgent agent)
        {
            return _agentCreatorFuncs[character](agent);
        }

        private IBaseAgent CreateEngineer(IBaseAgent agent)
        {
            return agent;
        }

        private IBaseAgent CreateScout(IBaseAgent agent)
        {
            return agent;
        }

        private IBaseAgent CreateDefender(IBaseAgent agent)
        {
            return agent;
        }

        private IBaseAgent CreateDemolisher(IBaseAgent agent)
        {
            return agent;
        }

        private IBaseAgent CreateReflector(IBaseAgent agent)
        {
            return agent;
        }

        private IBaseAgent CreateNinja(IBaseAgent agent)
        {
            return agent;   
        }

        private IBaseAgent CreateHealer(IBaseAgent agent)
        {
            return agent;
        }

        private IBaseAgent CreateHacker(IBaseAgent agent)
        {
            return agent;
        }
    }
}