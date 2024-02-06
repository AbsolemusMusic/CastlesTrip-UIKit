using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class UITableViewCell : MonoBehaviour, IUITableViewCell
    {
        private IndexPath indexPath;
        public IndexPath IndexPath => indexPath;

        [SerializeField]
        private Button cellButton;

        public IUITableViewCellDelegate TVCDelegate { get; set; }

        private RectTransform rect;
        public RectTransform Rect
        {
            get
            {
                if (!rect && TryGetComponent(out RectTransform _rect))
                    rect = _rect;
                return rect;
            }
        }

        public virtual void Init(IndexPath indexPath)
        {
            this.indexPath = indexPath;

            cellButton?.onClick.AddListener(OnCellTapped);
        }

        public virtual void SetSelected(bool isSelected)
        {

        }

        public virtual void OnCellTapped()
        {
            TVCDelegate?.OnCellTapped(this, indexPath);
        }
    }
}