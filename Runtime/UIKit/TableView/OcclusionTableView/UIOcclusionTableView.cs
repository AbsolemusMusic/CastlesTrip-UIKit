using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class UIOcclusionTableView : UITableView
    {
        [SerializeField]
        private ScrollRect _scrollRect;
        public ScrollRect ScrollRect => _scrollRect;

        private Transform srTransform;
        private bool isInit;
        private bool _isVertical, _isHorizontal;
        private float _disableMarginX, _disableMarginY;

        private Vector2 offset = new Vector2(20, 20);
        public virtual Vector2 Offset => offset;

        public virtual float WAIT { get; } = 0.1f;

        private bool[] cellStates;

        public override void Reload()
        {
            SetEnabledState(true);
            base.Reload();
            cellStates = new bool[m_dataSource.GetNumberOfRows(this)];
        }

        public override void ForceRebuildLayout(RectTransform parentRT)
        {
            base.ForceRebuildLayout(parentRT);

            _isVertical = _scrollRect.vertical;
            _isHorizontal = _scrollRect.horizontal;

            _scrollRect.onValueChanged.AddListener(UpdateCellsState);

            // TODO: Костыль, что с этим делать непонятно
            Invoke("Init", WAIT);
        }

        private void Init()
        {
            srTransform = _scrollRect.transform;

            if (_scrollRect.TryGetComponent(out RectTransform rectTransform))
            {
                Rect rect = rectTransform.rect;
                Vector2 sizeDelta = Cells[0].Rect.sizeDelta;
                _disableMarginX = rect.width / 2 + sizeDelta.x;
                _disableMarginY = rect.height / 2 + sizeDelta.y;
            }

            SetEnabledState(false);

            isInit = true;
            UpdateCellsState(_scrollRect.normalizedPosition);
        }

        private void SetEnabledState(bool isEnabled)
        {
            LayoutGroup.enabled = isEnabled;
            if (LayoutGroup.TryGetComponent(out ContentSizeFitter fitter))
                fitter.enabled = isEnabled;
        }

        private void UpdateCellsState(Vector2 value = default)
        {
            if (!isInit)
                return;

            foreach (UITableViewCell cell in Cells)
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
            OnWillDisplay(this, cell, indexPath);
        }

        public virtual void OnWillDisplay(UITableView tableView, UITableViewCell cell, IndexPath indexPath)
        {

        }
    }
}
