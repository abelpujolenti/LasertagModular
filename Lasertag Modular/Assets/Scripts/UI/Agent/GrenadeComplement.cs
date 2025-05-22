using Interface.Agent;

namespace UI.Agent
{
    public class GrenadeComplement : AgentDecorator
    {
        private const string IMAGE_PATH = "RCXD";

        public GrenadeComplement(IBaseAgent decoratedAgent) : base(decoratedAgent, IMAGE_PATH)
        {}
    }
}
