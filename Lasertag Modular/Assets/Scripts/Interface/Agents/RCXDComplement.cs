using UnityEngine;

public class RCXDComplement : AgentDecorator
{
    private const string IMAGE_PATH = "RCXD";

    public RCXDComplement(BaseAgent decoratedAgent) : base(decoratedAgent, IMAGE_PATH)
    {}
}
