using UnityEngine;
using UnityEngine.UI;

public class AlphaRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    [Range(0f, 1f)]
    public float alphaThreshold = 0.1f;

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Image image = GetComponent<Image>();

        if (image == null || image.sprite == null)
            return false;

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            screenPoint,
            eventCamera,
            out localPoint
        );

        Rect rect = image.sprite.rect;

        float x = localPoint.x / image.rectTransform.rect.width;
        float y = localPoint.y / image.rectTransform.rect.height;

        x += 0.5f;
        y += 0.5f;

        int textureX = Mathf.FloorToInt(rect.x + x * rect.width);
        int textureY = Mathf.FloorToInt(rect.y + y * rect.height);

        if (textureX < 0 || textureX >= image.sprite.texture.width ||
            textureY < 0 || textureY >= image.sprite.texture.height)
        {
            return false;
        }

        Color pixel = image.sprite.texture.GetPixel(textureX, textureY);

        return pixel.a >= alphaThreshold;
    }
}
