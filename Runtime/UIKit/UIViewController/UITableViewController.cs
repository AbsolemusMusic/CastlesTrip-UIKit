using UnityEngine;

namespace CT.UIKit
{
    // TODO: At Work
    public class UITableViewController : UIViewController, ITableViewDelegate, ITableViewDataSource
    {
        [SerializeField]
        private UITableView tableView;

        public override void Present(bool isShow = true)
        {
            tableView.m_delegate = this;
            tableView.m_dataSource = this;
            tableView.Reload();
            base.Present(isShow);
        }

        public virtual bool CanChoose(UITableView tableView, IndexPath indexPath)
        {
            return true;
        }

        public virtual void DidSelected(UITableView tableView, UITableViewCell cell, IndexPath indexPath)
        {
            cell.SetSelected(true);
        }

        public virtual int GetCellSelectedID(UITableView tableView)
        {
            return tableView.CellSelectedID;
        }

        public virtual int GetNumberOfRows(UITableView tableView)
        {
            return 0;
        }

        public virtual RectOffset GetPadding(UITableView tableView)
        {
            return new RectOffset(0, 0, 0, 0);
        }

        public virtual Vector2 GetSize(UITableView tableView, UITableViewCell cell = null)
        {
            return Vector2.zero;
        }

        public virtual Vector2 GetSpace(UITableView tableView)
        {
            return Vector2.zero;
        }

        public virtual UITableViewCell TableViewCell(UITableView tableView, IndexPath indexPath)
        {
            throw new System.NotImplementedException();
        }
    }
}