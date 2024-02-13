using System;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    [Serializable]
    public class UITabbarButton : MonoBehaviour, IUITabbarButton
    {
        [SerializeField]
        private Button mainButton;

        [SerializeField]
        private Image iconImg;

        [SerializeField]
        private Sprite activeSprite;

        [SerializeField]
        private Sprite inactiveSprite;

        public event Action<UITabbarButton> OnClicked;

        private void Start()
        {
            mainButton.onClick.AddListener(OnMainTapped);
        }

        private void OnDestroy()
        {
            mainButton.onClick.RemoveListener(OnMainTapped);
        }

        public virtual void SetSelected(bool state)
        {
            try
            {
                iconImg.sprite = state ? activeSprite : inactiveSprite;
            }
            catch { }
        }

        private void OnMainTapped()
        {
            OnClicked?.Invoke(this);
        }
    }
}