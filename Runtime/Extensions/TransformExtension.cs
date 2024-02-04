using UnityEngine;

public static class TransformExtension
{
    public static void Reset(this Transform transform)
    {
        if (!transform) return;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }
}
