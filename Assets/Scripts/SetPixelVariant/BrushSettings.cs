using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class BrushSettings : MonoBehaviour
{
    public Color Color = Color.black;
    public int Size = 1;

    public Color[] Colors
    {
        get
        {
            var count = Mathf.Pow(Size, 2);
            return Enumerable.Repeat(Color,(int)count).ToArray();
        }
    }

    public void SetSize(Slider slider)
    {
        Size = (int)slider.value;
    }

    public void SetColor(Image image)
    {
        Color = image.color;
    }
}
