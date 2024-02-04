using UnityEngine;

namespace Extension.UI
{
    public static class RectTransformExtension
    {
        public static void AnchorReset(this RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }
    }
}