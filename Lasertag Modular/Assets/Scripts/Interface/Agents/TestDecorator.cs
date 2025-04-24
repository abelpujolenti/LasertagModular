using UnityEngine;

public class TestDecorator : MonoBehaviour
{
    [SerializeField] private GameObject _gameObject;
    private BaseAgent agent;

    void Start()
    {
        agent = _gameObject.GetComponent<BaseAgent>();

        agent = new RCXDComplement(agent);

    }

    [SerializeField] private byte[] data; // Starts from the right, data[0] = 0 0 0 X, data[1] 0 0 X 0

    // Update is called once per frame
    void Update()
    {
        agent.CheckState(data);
    }
}
