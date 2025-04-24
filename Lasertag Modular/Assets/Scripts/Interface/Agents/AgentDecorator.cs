using UnityEngine;

public abstract class AgentDecorator : BaseAgent
{
    protected BaseAgent decoratedAgent;

    protected ushort _cellCounter = 0;

    protected CheckCell _checkCell;

    protected string _imagePath;    

    public AgentDecorator(BaseAgent decoratedAgent, string imagePath)
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