using System;

namespace CT.UIKit
{
    public interface ITabbarView
    {
        public int CountOfItems { get; }
        event Action<int, bool> OnTabbarButtonTapped;
    }
}