using Interface.Agent;
using UnityEngine;

namespace UI.Agent
{
    public abstract class AgentDecorator : IBaseAgent
    {
        private IBaseAgent decoratedAgent;

        private ushort _cellCounter = 0;

        private CheckCell _checkCell;

        protected string _imagePath;    

        protected AgentDecorator(IBaseAgent decoratedAgent, string imagePath)
        {
            this.decoratedAgent = decoratedAgent;

            _checkCell = CreateImage();
            _checkCell.SetImage(imagePath);
            AddImageToGrid(_checkCell.gameObject);
            decoratedAgent.IncrementCounter();
        }

        public void IncrementCounter()
        {
            _cellCounter++;
            decoratedAgent.IncrementCounter();
        }

        public CheckCell CreateImage()
        {
            return decoratedAgent.CreateImage();
        }

        public void AddImageToGrid(GameObject cellGameObject)
        {
            decoratedAgent.AddImageToGrid(cellGameObject);
        }

        public void CheckState(byte[] checkState)
        {
            _checkCell.ToggleCheck(checkState[_cellCounter] == 1);
            decoratedAgent.CheckState(checkState);
        }
    }
}