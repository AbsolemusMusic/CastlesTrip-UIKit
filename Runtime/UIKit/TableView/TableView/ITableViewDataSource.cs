using UnityEngine;

namespace CT.UIKit
{
    public interface ITableViewDataSource
    {
        public int GetNumberOfRows(UITableView tableView);
        public int GetCellSelectedID(UITableView tableView);
        public Vector2 GetSize(UITableView tableView, UITableViewCell cell = null);
        public Vector2 GetSpace(UITableView tableView);
        public RectOffset GetPadding(UITableView tableView);
        public UITableViewCell TableViewCell(UITableView tableView, IndexPath indexPath);
    }
}