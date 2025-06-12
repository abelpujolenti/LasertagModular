using UI.Agent;
using UnityEngine;

namespace Interface.Agent
{
    public interface IBaseAgent
    {
        public void IncrementCounter();
        public CheckCell CreateImage();
        public void AddImageToGrid(GameObject cellGameObject);

        public void CheckState(byte[] checkState);
    }
}