using UnityEngine;

public class WobbleElement : MonoBehaviour
{
    public float wobbleAngle = 15f;
    public float wobbleSpeed = 1f;

    private float initialZRotation;

    void Start()
    {
        initialZRotation = transform.localEulerAngles.z;
    }

    void Update()
    {
        float wobble = Mathf.Sin(Time.unscaledTime * wobbleSpeed * Mathf.PI * 2) * wobbleAngle;
        Vector3 rotation = transform.localEulerAngles;
        rotation.z = initialZRotation + wobble;
        transform.localEulerAngles = rotation;
    }
}