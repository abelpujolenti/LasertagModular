using TMPro;
using UnityEngine;

public class ColorLerpTextMeshPro : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public Color color1 = Color.red;
    public Color color2 = Color.green;
    public Color color3 = Color.blue;
    public Color color4 = Color.yellow;
    public float lerpSpeed = 2.0f;

    private Color[] colors;
    private float lerpTime;

    void Start()
    {
        colors = new Color[] { color1, color2, color3, color4 };
    }

    void Update()
    {
        lerpTime += Time.deltaTime * lerpSpeed;
        float index = Mathf.PingPong(lerpTime, 3);
        int colorIndex1 = Mathf.FloorToInt(index);
        int colorIndex2 = (colorIndex1 + 1) % colors.Length;
        float lerpFactor = index - colorIndex1;
        textMeshPro.color = Color.Lerp(colors[colorIndex1], colors[colorIndex2], lerpFactor);
    }
}