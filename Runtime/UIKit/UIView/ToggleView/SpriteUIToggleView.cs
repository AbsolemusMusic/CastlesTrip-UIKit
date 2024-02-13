using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class SpriteUIToggleView : AbstractUIToggleView
    {
        [Space()]
        [SerializeField]
        private Image _targetImage;

        [Space()]
        [SerializeField]
        private Sprite inactiveSprite;

        [SerializeField]
        private Sprite activeSprite;

        public override void UpdateState(AbstractUIToggleView view, bool state)
        {
            if (!_targetImage) return;
            Sprite targetSprite = state ? activeSprite : inactiveSprite;
            SetSprite(targetSprite);
        }

        private void SetSprite(Sprite sprite)
        {
            if (!sprite) return;
            _targetImage.sprite = sprite;
        }
    }
}
