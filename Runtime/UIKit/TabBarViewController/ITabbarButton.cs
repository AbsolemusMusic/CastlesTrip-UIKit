using System;

namespace CastlesTrip.UIKit
{
    public interface ITabbarButton
    {
        public void SetSelected(bool state);
        public event Action<TabbarButton> OnClicked;
    }
}