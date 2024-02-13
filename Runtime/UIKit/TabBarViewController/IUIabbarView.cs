using System;

namespace CT.UIKit
{
    public interface IUITabbarView
    {
        int CountOfItems { get; }
        int SelectedID { get; }

        event Action<int, bool> OnTabbarButtonTapped;

        void Select(int index);
    }
}