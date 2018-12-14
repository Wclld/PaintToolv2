using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasManager: MonoBehaviour
{   
    [SerializeField]
    private Texture2D _texture;
    private RectTransform _rectTransform;
    private Vector2 _lastMousePosition;

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
        clone.Apply();
        _texture = clone;

        _width = 256;
        _height= 256;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _lastMousePosition = GetMousePosition();
            DrawPoint(_lastMousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            var currentMousePosition = GetMousePosition();
            if(currentMousePosition != _lastMousePosition)
                DrawLine(_lastMousePosition, currentMousePosition);
            _lastMousePosition = currentMousePosition;
        }
    }

    private void DrawPoint(Vector2 position)
    {
        if (IsInBounds(position))
        {
            var brushOffset = _brush.Size / 2;
            _texture.SetPixels((int)position.x - brushOffset, (int)position.y - brushOffset, _brush.Size, _brush.Size, _brush.Colors);
            _texture.Apply();
        }
    }

    private Vector2 GetMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, Input.mousePosition, Camera.main, out mousePosition);
        mousePosition.x = mousePosition.x + _width * 0.5f;
        mousePosition.y = mousePosition.y + _height * 0.5f;

        return mousePosition;
    }

    private bool IsInBounds(Vector2 position)
    {
        bool result = false;
        var brushOffset = _brush.Size / 2;
        if (position.x - brushOffset >= 0 && position.x + brushOffset <= _width)
        {
            if (position.y - brushOffset >= 0 && position.y + brushOffset <= _height)
            {
                result = true;
            }
        }
        return result;
    }
    
    private void DrawLine(Vector2 firstPos, Vector2 secondPos)
    {
        var x0 = (int)firstPos.x;
        var y0 = (int)firstPos.y;
        var x1 = (int)secondPos.x;
        var y1 = (int)secondPos.y;

        bool steep = Mathf.Abs(y1 - y0) > Mathf.Abs(x1 - x0);

        if (steep)
        {
            SwapInt(ref x0, ref y0);
            SwapInt(ref x1, ref y1);
        }
        if (x0 > x1)
        {
            SwapInt(ref x0, ref x1);
            SwapInt(ref y0, ref y1);
        }
        int dX = (x1 - x0);
        int dY = Math.Abs(y1 - y0);
        int err = (dX / 2);
        int ystep = (y0 < y1 ? 1 : -1);
        int y = y0;

        for (int x = x0; x <= x1; ++x)
        {
            if (steep)
                DrawPoint(new Vector2(y, x));
            else
                DrawPoint(new Vector2(x, y));

            err = err - dY;

            if (err < 0)
            {
                y += ystep;
                err += dX;
            }
        }
    }

    public void Save()
    {
        var image = _texture.EncodeToPNG();
        var path = Application.dataPath + "/SavedImage.png";
        File.WriteAllBytes(path, image);
    }

    private void SwapInt(ref int firstElement, ref int secondElement)
    {
        firstElement += secondElement;
        secondElement = firstElement - secondElement;
        firstElement -= secondElement;
    }
}
