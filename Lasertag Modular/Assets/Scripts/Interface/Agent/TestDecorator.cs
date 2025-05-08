using UI.Agent;
using UnityEngine;

namespace Interface.Agent
{
    public class TestDecorator : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        private IBaseAgent agent;

        void Start()
        {
            agent = _gameObject.GetComponent<IBaseAgent>();

            agent = new CarComplement(agent);
        }

        [SerializeField] private byte[] data; // Starts from the right, data[0] = 0 0 0 X, data[1] 0 0 X 0

        // Update is called once per frame
        void Update()
        {
            agent.CheckState(data);
        }
    }
}