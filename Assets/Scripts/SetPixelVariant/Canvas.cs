using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Canvas : MonoBehaviour, IPointerDownHandler, IDragHandler
{   
    [SerializeField]
    private Texture2D _texture;
    private RectTransform _rectTransform;

    private int _width;
    private int _height;

    [SerializeField]
    private BrushSettings _brush;

    private void Start()
    {
        var rawImage = GetComponent<RawImage>();
        
        _rectTransform = rawImage.GetComponent<RectTransform>();
        _texture = (Texture2D)rawImage.texture;

        Texture2D clone = Instantiate(_texture);
        rawImage.texture = clone;
        clone.SetPixel(0, 0, Color.red);
        clone.Apply();
        _texture = clone;

        _width = 256;
        _height= 256;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Draw();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Draw();
    }

    private void Draw()
    {
        var mousePosition = GetMousePosition();

        _texture.SetPixel((int)mousePosition.x, (int)mousePosition.y, _brush.Color);
        _texture.SetPixels((int)mousePosition.x, (int)mousePosition.y, _brush.Size, _brush.Size, _brush.Colors);
        _texture.Apply();
    }

    private Vector2 GetMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, Camera.main, out mousePosition);
        mousePosition.x = mousePosition.x + _width * 0.5f;
        mousePosition.y = mousePosition.y + _height * 0.5f;

        return mousePosition;
    }
    
    public void Save()
    {
        var image = _texture.EncodeToPNG();
        var path = Application.dataPath + "/SavedImage.png";
        File.WriteAllBytes(path, image);
    }
}
