using UnityEngine;

namespace CT.UIKit
{
    public interface ITableView
    {
        public void Reload();
        public void RemoveItems();
        public void Fetch();
        public void ForceRebuildLayout();
        public void ForceRebuildLayout(RectTransform parentRT);
    }
}