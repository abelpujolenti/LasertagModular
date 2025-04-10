using UnityEngine;
using UnityEngine.UI;

public class MovingImage : MonoBehaviour
{
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;
    public Vector2 tiling = new Vector2(2, 2);
    public Vector2 offset = Vector2.zero;

    void Start()
    {
        if (_img != null)
        {
            _img.uvRect = new Rect(offset, tiling);
        }
        else
        {
            Debug.LogError("RawImage no asignado en el inspector");
        }
    }

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
    }
}