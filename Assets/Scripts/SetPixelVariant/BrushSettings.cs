using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BrushSettings : MonoBehaviour
{
    public Color Color;
    public int Size;
    public Color[] Colors
    {
        get
        {
            var count = Mathf.Pow(Size, 2);
            return Enumerable.Repeat<Color>(Color,(int)count).ToArray();
        }
    }

    public void SetSize(Slider slider)
    {
        Size = (int)slider.value;
    }
    public void SetColor(int color)
    {
        if (color == 1)
            Color = Color.red;
        if (color == 2)
            Color = Color.green;
        if (color == 3)
            Color = Color.black;
        if (color == 4)
            Color.a = 0;
    }
}
