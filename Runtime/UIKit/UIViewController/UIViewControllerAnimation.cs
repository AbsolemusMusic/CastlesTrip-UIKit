using UnityEngine;

namespace CT.UIKit
{
    public class UIViewControllerAnimation
    {
        private RectTransform m_contentRT;
        private RectTransform contentRT => m_contentRT;

        private UIViewControllerPresentType presentType = UIViewControllerPresentType.none;
        public UIViewControllerPresentType PresentType
        {
            get
            {
                return presentType;
            }

            set
            {
                presentType = value;
                target = GetStartAnchors(presentType);
                // TODO: Разобраться нужно ли
                //UpdatePosition();
            }
        }
        public virtual float CloseWaitTime => presentType != UIViewControllerPresentType.none ? 0.8f : 0f;

        private AnchorData target = new AnchorData();

        public void Init(RectTransform rect)
        {
            m_contentRT = rect;
        }

        public void PresentAnimation(bool isPresent, CompletionEH completion)
        {
            ProgressChangedEH handler = GetHandler(isPresent);
            Animation anim = new Animation();
            anim.AnimTime = CloseWaitTime;
            anim.AnimType = AnimationType.Pow2AnimationType;
            anim.StartAnim(handler, completion);
            if (!isPresent) return;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            contentRT.anchorMin = target.min;
            contentRT.anchorMax = target.max;
        }

        private AnchorData GetStartAnchors(UIViewControllerPresentType presentType)
        {
            switch (presentType)
            {
                case UIViewControllerPresentType.fromUp: return new AnchorData(new Vector2(0f, 1f), new Vector2(1f, 2f));
                case UIViewControllerPresentType.fromRight: return new AnchorData(new Vector2(1f, 0f), new Vector2(2f, 1f));
                case UIViewControllerPresentType.fromDown: return new AnchorData(new Vector2(0f, -1f), new Vector2(1f, 0f));
                case UIViewControllerPresentType.fromLeft: return new AnchorData(new Vector2(-1f, 0f), new Vector2(0f, 1f));
                default: return new AnchorData(Vector2.zero, Vector2.one);
            }
        }

        private ProgressChangedEH GetHandler(bool isPresent)
        {
            ProgressChangedEH handler = delegate (float progress, float scale)
            {
                if (!contentRT) return;
                contentRT.anchorMin = Vector2.Lerp(contentRT.anchorMin, isPresent ? Vector2.zero : target.min, scale);
                contentRT.anchorMax = Vector2.Lerp(contentRT.anchorMax, isPresent ? Vector2.one : target.max, scale);
            };
            return handler;
        }
    }
}