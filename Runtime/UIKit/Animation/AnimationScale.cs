using UnityEngine;

namespace CT.UIKit
{
    public class AnimationScale
    {
        public static float GetScale(AnimationType type, float progress)
        {
            switch (type)
            {
                case AnimationType.LinearAnimationType: return progress;
                case AnimationType.SqrtAnimationType: return Mathf.Pow(progress, 0.5f);
                case AnimationType.Pow2AnimationType: return Mathf.Pow(progress, 2f);
                default: return progress;
            }
        }
    }
}