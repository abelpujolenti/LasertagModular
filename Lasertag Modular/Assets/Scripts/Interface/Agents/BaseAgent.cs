using System;
using Unity.VisualScripting;
using UnityEngine;

public interface BaseAgent
{
    public void IncrementCounter();
    public CheckCell CreateImage();
    public void AddImageToGrid(GameObject cellGameObject);

    public void CheckState(byte[] checkState);
}