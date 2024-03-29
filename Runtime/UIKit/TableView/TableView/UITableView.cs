using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class UITableView : MonoBehaviour, ITableView, ITableViewDelegate, IUITableViewCellDelegate
    {
        [SerializeField]
        private ScrollRect _scrollRect;
        public ScrollRect ScrollRect => _scrollRect;

        [SerializeField]
        private LayoutGroup layoutGroup;
        public LayoutGroup LayoutGroup => layoutGroup;

        private OcclussionModel occlussionModel;

        public ITableViewDataSource m_dataSource;
        public ITableViewDelegate m_delegate;

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

        public virtual void Reload()
        {
            occlussionModel?.Unsubscribe();
            if (m_dataSource.GetOcclussionState(this))
            {
                if (occlussionModel == null)
                    occlussionModel = new OcclussionModel(this);
                occlussionModel?.Subscribe(this);
            }

            RemoveItems();
            Fetch();
            ForceRebuildLayout();

            occlussionModel?.UpdateModel();
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

        public virtual void RemoveItems()
        {
            layoutGroup?.gameObject.SetActive(false);
            foreach (UITableViewCell cell in cells)
                Destroy(cell.gameObject);

            foreach (Transform child in layoutGroup.transform)
                Destroy(child.gameObject);

            cells.Clear();
        }

        public virtual void OnCellTapped(UITableViewCell cell, IndexPath indexPath)
        {
            if (!m_dataSource.CanChoose(this, indexPath)) return;
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

        public void OnWillDisplay(UITableView tableView, UITableViewCell cell, IndexPath indexPath)
        {
            m_delegate?.OnWillDisplay(tableView, cell, indexPath);
        }

        public virtual void ForceRebuildLayout()
        {
            if (layoutGroup.transform.parent.TryGetComponent(out RectTransform rectTr))
                ForceRebuildLayout(rectTr);
        }

        public virtual void ForceRebuildLayout(RectTransform parentRT)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(parentRT);
            occlussionModel?.ForceRebuildLayout();

            if (occlussionModel == null)
                return;
            // TODO: ????????? ???????
            Invoke(nameof(TryInitOcculussion), occlussionModel.WAIT);
        }

        // TODO: ????????? ???????
        private void TryInitOcculussion()
        {
            occlussionModel?.Init();
        }
    }
}