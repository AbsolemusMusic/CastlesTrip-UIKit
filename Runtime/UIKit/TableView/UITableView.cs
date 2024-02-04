using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class UITableView : MonoBehaviour, ITableView, ITableViewDelegate, IUITableViewCell
    {
        [SerializeField]
        private LayoutGroup layoutGroup;
        public LayoutGroup LayoutGroup => layoutGroup;

        public ITableViewDataSource m_dataSource;
        public ITableViewDelegate m_delegate;

        private GameObject cellPref;

        private UITableViewCell lastSelected;
        public UITableViewCell CellSelected => lastSelected ? lastSelected : null;
        private List<UITableViewCell> cells = new List<UITableViewCell>();
        public List<UITableViewCell> Cells => cells;


        public int CellSelectedID
        {
            get
            {
                if (m_dataSource == null)
                    return -1;
                return m_dataSource.GetCellSelectedID(this);
            }
        }


        public UITableViewCell GetCell(IndexPath indexPath)
        {
            foreach (GameObject cellGO in layoutGroup.transform)
            {
                if (cellGO.TryGetComponent(out UITableViewCell cell))
                {
                    if (cell.IndexPath.row.Equals(indexPath.row))
                    {
                        return cell;
                    }
                }
            }

            return null;
        }

        public void UpdateCellsSize()
        {
            foreach (UITableViewCell cell in cells)
            {
                if (cell.TryGetComponent(out RectTransform cellRT))
                {
                    cellRT.sizeDelta = m_dataSource.GetSize(this, cell);
                }
            }
        }

        public void RegisterCell(GameObject cellPref)
        {
            if (cellPref && TryGetComponent(out UITableViewCell _))
            {
                this.cellPref = cellPref;
            }
        }

        public virtual void Fetch()
        {
            if (m_delegate == null || m_dataSource == null) return;
            for (int i = 0; i < m_dataSource.GetNumberOfRows(this); i++)
            {
                IndexPath indexPath = new IndexPath(i);
                UITableViewCell cell = m_dataSource.TableViewCell(this, indexPath);

                cells.Add(cell);
                // FixMe 
                bool isSelected = CellSelectedID == i;
                cell.SetSelected(isSelected);
                if (isSelected)
                {
                    lastSelected?.SetSelected(false);
                    lastSelected = cell;
                    lastSelected?.SetSelected(true);
                }
            }

            Vector2 space = m_dataSource.GetSpace(this);
            if (layoutGroup is HorizontalLayoutGroup)
            {
                ((HorizontalLayoutGroup)layoutGroup).spacing = space.x;
                ((HorizontalLayoutGroup)layoutGroup).padding = m_dataSource.GetPadding(this);
            }
            else if (layoutGroup is VerticalLayoutGroup)
            {
                ((VerticalLayoutGroup)layoutGroup).spacing = space.y;
                ((VerticalLayoutGroup)layoutGroup).padding = m_dataSource.GetPadding(this);
            }
            else if (layoutGroup is GridLayoutGroup)
            {
                ((GridLayoutGroup)layoutGroup).spacing = space;
                ((GridLayoutGroup)layoutGroup).cellSize = m_dataSource.GetSize(this);
                ((GridLayoutGroup)layoutGroup).padding = m_dataSource.GetPadding(this);
            }

            layoutGroup?.gameObject.SetActive(true);

            UpdateCellsSize();
        }

        public virtual void Reload()
        {
            RemoveItems();
            Fetch();
            ForceRebuildLayout();
        }

        public virtual void RemoveItems()
        {
            layoutGroup?.gameObject.SetActive(false);
            foreach (UITableViewCell cell in cells)
                Destroy(cell.gameObject);

            foreach (Transform child in layoutGroup.transform)
            {
                Destroy(child.gameObject);
            }

            cells.Clear();
        }

        public virtual void OnCellTapped(UITableViewCell cell, IndexPath indexPath)
        {
            if (!m_delegate.CanChoose(this, indexPath)) return;
            DidSelected(this, cell, indexPath);
        }

        public bool CanChoose(UITableView tableView, IndexPath indexPath)
        {
            return true;
        }

        private void DidSelectedWithoutNotify(UITableView tableView, UITableViewCell cell, IndexPath indexPath)
        {
            if (!cell) return;
            lastSelected?.SetSelected(false);
            lastSelected = cell;
            lastSelected?.SetSelected(true);
        }

        public void DidSelected(UITableView tableView, UITableViewCell cell, IndexPath indexPath)
        {
            if (!cell) return;
            DidSelectedWithoutNotify(tableView, cell, indexPath);
            m_delegate?.DidSelected(tableView, cell, indexPath);
        }

        public void SetCellSelected(int indexPath)
        {
            UITableViewCell cell = cells[indexPath];
            DidSelected(this, cell, cell.IndexPath);
        }

        public void SetCellSelectedWithoutNotify(int indexPath)
        {
            if (indexPath < 0)
            {
                foreach (UITableViewCell c in Cells)
                    c.SetSelected(false);
                return;
            }
            UITableViewCell cell = cells[indexPath];
            DidSelectedWithoutNotify(this, cell, cell.IndexPath);
        }

        public virtual void ForceRebuildLayout()
        {
            if (layoutGroup.transform.parent.TryGetComponent(out RectTransform rectTr))
                ForceRebuildLayout(rectTr);
        }

        public virtual void ForceRebuildLayout(RectTransform parentRT)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRT);
        }
    }

    public interface ITableView
    {
        public void RegisterCell(GameObject cellPref);
        public void Reload();
        public void RemoveItems();
        public void Fetch();
        public void ForceRebuildLayout();
        public void ForceRebuildLayout(RectTransform parentRT);
    }

    public interface ITableViewDataSource
    {
        public int GetNumberOfRows(UITableView tableView);
        public int GetCellSelectedID(UITableView tableView);
        public Vector2 GetSize(UITableView tableView, UITableViewCell cell = null);
        public Vector2 GetSpace(UITableView tableView);
        public RectOffset GetPadding(UITableView tableView);
        public UITableViewCell TableViewCell(UITableView tableView, IndexPath indexPath);
    }

    public interface ITableViewDelegate
    {
        public bool CanChoose(UITableView tableView, IndexPath indexPath);
        public void DidSelected(UITableView tableView, UITableViewCell cell, IndexPath indexPath);
    }

    public struct IndexPath
    {
        public int row;
        public IndexPath(int cellID)
        {
            row = cellID;
        }
    }
}