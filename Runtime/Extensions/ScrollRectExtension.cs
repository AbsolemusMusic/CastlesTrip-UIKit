using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public static class ScrollRectExtension
{
    public static void Scroll(this ScrollRect scrollRect, bool isDown)
    {
        float target = isDown ? 0f : 1f;
        scrollRect.verticalNormalizedPosition = target;
    }

    public static void Scroll(this ScrollRect scrollRect, bool isDown, float time, MonoBehaviour beh)
    {
        float target = isDown ? 0f : 1f;
        beh.StartCoroutine(ScrollAnim(scrollRect, target, time));
    }

    public static IEnumerator ScrollAnim(ScrollRect scrollRect, float target, float time)
    {
        float start = scrollRect.verticalNormalizedPosition;
        for (float curTime = 0f; curTime < time; curTime += Time.deltaTime)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, curTime / time);
            yield return null;
        }
        scrollRect.verticalNormalizedPosition = target;
    }
}
