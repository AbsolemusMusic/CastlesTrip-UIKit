namespace CT.UIKit
{
    public interface ITableViewDelegate
    {
        public void DidSelected(UITableView tableView, UITableViewCell cell, IndexPath indexPath) { }
        public void OnWillDisplay(UITableView tableView, UITableViewCell cell, IndexPath indexPath) { }
    }
}