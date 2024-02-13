using System;

namespace CT.UIKit
{
    public interface IUITabbarButton
    {
        public void SetSelected(bool state);
        public event Action<UITabbarButton> OnClicked;
    }
}