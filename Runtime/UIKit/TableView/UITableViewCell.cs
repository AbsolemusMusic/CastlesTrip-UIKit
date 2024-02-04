using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class UITableViewCell : MonoBehaviour
    {
        private IndexPath indexPath;
        public IndexPath IndexPath => indexPath;

        [SerializeField]
        private Button cellButton;

        public IUITableViewCell tvcDelegate;

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
            tvcDelegate?.OnCellTapped(this, indexPath);
        }
    }

    public interface IUITableViewCell
    {
        public void OnCellTapped(UITableViewCell cell, IndexPath indexPath);
    }
}