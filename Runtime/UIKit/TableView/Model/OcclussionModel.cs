using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class OcclussionModel
    {
        private UITableView tableView;
        private ScrollRect _scrollRect;

        private Transform srTransform;
        private bool isInit;
        private bool _isVertical, _isHorizontal;
        private float _disableMarginX, _disableMarginY;

        private Vector2 offset = new Vector2(20, 20);
        public virtual Vector2 Offset => offset;

        public virtual float WAIT { get; } = 0.1f;

        private bool[] cellStates;

        public OcclussionModel(UITableView _tableView)
        {
            tableView = _tableView;
            _scrollRect = _tableView.ScrollRect;
        }

        public void Subscribe(UITableView _tableView)
        {
            isInit = false;
            tableView = _tableView;
            _scrollRect = _tableView.ScrollRect;
            _scrollRect.onValueChanged.AddListener(UpdateCellsState);

            ITableViewContentDataSource dataSource = tableView.m_dataSource;
            cellStates = new bool[dataSource.GetNumberOfRows(tableView)];
        }

        public void Unsubscribe()
        {
            if (_scrollRect != null)
                _scrollRect.onValueChanged.RemoveListener(UpdateCellsState);
        }

        public void UpdateModel()
        {
            SetEnabledState(true);
        }

        public void ForceRebuildLayout()
        {
            _isVertical = _scrollRect.vertical;
            _isHorizontal = _scrollRect.horizontal;
        }

        public void Init()
        {
            srTransform = _scrollRect.transform;

            if (_scrollRect.TryGetComponent(out RectTransform rectTransform))
            {
                Rect rect = rectTransform.rect;
                Vector2 sizeDelta = tableView.Cells[0].Rect.sizeDelta;
                _disableMarginX = rect.width / 2 + sizeDelta.x;
                _disableMarginY = rect.height / 2 + sizeDelta.y;
            }

            SetEnabledState(false);

            isInit = true;
            UpdateCellsState(_scrollRect.normalizedPosition);
        }

        private void SetEnabledState(bool isEnabled)
        {
            tableView.LayoutGroup.enabled = isEnabled;
            if (tableView.LayoutGroup.TryGetComponent(out ContentSizeFitter fitter))
                fitter.enabled = isEnabled;
        }

        private void UpdateCellsState(Vector2 value = default)
        {
            if (!isInit)
                return;

            foreach (UITableViewCell cell in tableView.Cells)
                UpdateCellState(cell);
        }

        private void UpdateCellState(UITableViewCell cell)
        {
            RectTransform rect = cell.Rect;

            Vector3 inverseTrPoint = srTransform.InverseTransformPoint(rect.position);
            bool isLessVertical = inverseTrPoint.y < -_disableMarginY - Offset.y;
            bool isGreatVertical = inverseTrPoint.y > _disableMarginY + Offset.y;

            bool isLessHorizontal = inverseTrPoint.x < -_disableMarginX - Offset.x;
            bool isGreatHorizontal = inverseTrPoint.x > _disableMarginX + Offset.x;

            bool isInactive = true;

            bool isLessOrGreatVertical = isLessVertical || isGreatVertical;
            bool isLessOrGreatHorizontal = isLessHorizontal || isGreatHorizontal;

            if (_isVertical && _isHorizontal)
            {
                isInactive = isLessOrGreatVertical || isLessOrGreatHorizontal;
                SetActiveState(cell, !isInactive);
                return;
            }

            if (_isVertical)
                isInactive = isLessOrGreatVertical;

            if (_isHorizontal)
                isInactive = isLessOrGreatHorizontal;

            SetActiveState(cell, !isInactive);
        }

        private void SetActiveState(UITableViewCell cell, bool isActive)
        {
            GameObject cellGO = cell.Rect.gameObject;
            IndexPath indexPath = cell.IndexPath;
            bool isDisplay = isActive && !cellStates[indexPath.row];
            cellStates[indexPath.row] = isActive;
            cellGO.SetActive(isActive);
            if (!isDisplay)
                return;
            tableView.OnWillDisplay(tableView, cell, indexPath);
        }
    }
}