using UnityEngine;

namespace CT.UIKit
{
    public interface ITableViewDataSource : ITableViewDesignDataSource, ITableViewContentDataSource
    {
        
    }

    public interface ITableViewDesignDataSource
    {
        public Vector2 GetSize(UITableView tableView, UITableViewCell cell = null);
        public Vector2 GetSpace(UITableView tableView);
        public RectOffset GetPadding(UITableView tableView);
    }

    public interface ITableViewContentDataSource
    {
        public bool GetOcclussionState(UITableView tableView) { return false; }
        public bool CanChoose(UITableView tableView, IndexPath indexPath) { return true; }
        public int GetNumberOfRows(UITableView tableView);
        public int GetCellSelectedID(UITableView tableView);
        public UITableViewCell TableViewCell(UITableView tableView, IndexPath indexPath);
    }
}