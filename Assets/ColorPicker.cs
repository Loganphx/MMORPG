using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour, IPointerClickHandler
{
    public Color pickedColor;

    private Texture2D _texture2D;
    [Serializable]
    public class ColorEvent: UnityEvent<Color> {}

    public ColorEvent OnColorPicked = new ColorEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        pickedColor = GetColor(GetPointerPosition());
        OnColorPicked.Invoke(pickedColor);
    }

    private void Awake()
    {
        Texture texture = GetComponent<RawImage>().texture;
        _texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
 
        RenderTexture currentRT = RenderTexture.active;
 
        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);
 
        RenderTexture.active = renderTexture;
        _texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        _texture2D.Apply();
    }

    private Color GetColor(Vector2 pos)
    {
        Color selectedColor = _texture2D.GetPixelBilinear(pos.x, pos.y);
        selectedColor.a = 1;
        return selectedColor;
    }

    private Vector2 GetPointerPosition()
    {
        Vector3[] imageCorners = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(imageCorners);
        float textureWidth = imageCorners[2].x - imageCorners[0].x;
        float textureHeight = imageCorners[2].y - imageCorners[0].y;
        float uvXPos = (Input.mousePosition.x - imageCorners[0].x) / textureWidth;
        float uvYPos = (Input.mousePosition.y - imageCorners[0].y) / textureHeight;

        return new Vector2(uvXPos, uvYPos);
    }
}
