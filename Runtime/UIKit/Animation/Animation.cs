using System.Collections;
using UnityEngine;

namespace CT.UIKit
{
    public class Animation
    {
        public float AnimTime;
        public AnimationType AnimType;
        public ProgressChangedEH progressEH;
        public CompletionEH completionEH;

        private IEnumerator animator;


        public Animation() { }
        public Animation(float time, AnimationType type, MonoBehaviour mono)
        {
            AnimTime = time;
            AnimType = type;
        }

        public void StartAnim(ProgressChangedEH progressEH, CompletionEH completion)
        {
            this.progressEH = progressEH;
            this.completionEH = completion;

            StopAnim();
            animator = Anim();
            Coroutines.StartRoutine(animator);
        }

        public void StopAnim()
        {
            if (animator == null) return;
            Coroutines.StopRoutine(animator);
            animator = null;
        }

        private IEnumerator Anim()
        {
            float progress = 0f;
            float scale = 0f;

            progressEH?.Invoke(progress, scale);
            while (progress < AnimTime)
            {
                scale = AnimationScale.GetScale(AnimType, progress);
                progress += Time.deltaTime;
                progressEH?.Invoke(progress, scale);
                yield return null;
            }

            progressEH?.Invoke(1f, 1f);
            completionEH?.Invoke();
        }
    }
}