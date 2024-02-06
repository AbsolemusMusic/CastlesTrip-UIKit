namespace CT.UIKit
{
    public interface ITableViewDelegate
    {
        public bool CanChoose(UITableView tableView, IndexPath indexPath);
        public void DidSelected(UITableView tableView, UITableViewCell cell, IndexPath indexPath);
    }
}