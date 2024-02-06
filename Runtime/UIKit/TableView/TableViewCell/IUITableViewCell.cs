using UnityEngine;

namespace CT.UIKit
{
    public interface IUITableViewCell
    {
        IndexPath IndexPath { get; }
        IUITableViewCellDelegate TVCDelegate { get; set; }

        RectTransform Rect { get; }

        void Init(IndexPath indexPath);
        void SetSelected(bool isSelected);
        void OnCellTapped();
    }
}