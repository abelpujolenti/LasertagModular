using Interface.Agent;

namespace UI.Agent
{
    public class CarComplement : AgentDecorator
    {
        private const string IMAGE_PATH = "RCXD";

        public CarComplement(IBaseAgent decoratedAgent) : base(decoratedAgent, IMAGE_PATH)
        {}
    }
}
