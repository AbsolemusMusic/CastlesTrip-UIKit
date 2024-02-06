namespace CT.UIKit
{
    public interface IUITableViewCellDelegate
    {
        public void OnCellTapped(UITableViewCell cell, IndexPath indexPath);
    }
}