using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    public static void SetAlpha(this Image image, bool isActive)
    {
        if (!image) return;
        image.color = isActive ? Color.white : Color.clear;
    }
}
