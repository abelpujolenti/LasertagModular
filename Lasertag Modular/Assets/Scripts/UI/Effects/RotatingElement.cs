using UnityEngine;

public class RotatingElement : MonoBehaviour
{
    public float rotationSpeed = 180f;
    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.unscaledDeltaTime);
    }
}