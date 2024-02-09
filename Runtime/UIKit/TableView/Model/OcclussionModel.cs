using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class OcclussionModel : IOcclussionModel
    {
        private UITableView _tableView;
        private ScrollRect _scrollRect;

        private RectTransform _srRTransform;
        private bool isInit;
        private bool _isVertical, _isHorizontal;
        private float _disableMarginX, _disableMarginY;

        private Vector2 offset = new Vector2(20, 20);
        public virtual Vector2 Offset => offset;

        public virtual float WAIT { get; } = 0.1f;

        private bool[] cellStates;

        public bool IsSubscribed { get; private set; }

        public OcclussionModel()
        {

        }

        public void Init(UITableView _tableView)
        {
            this._tableView = _tableView;

            isInit = false;

            _scrollRect = _tableView.ScrollRect;
            _srRTransform = _scrollRect.GetComponent<RectTransform>();
            _isVertical = _scrollRect.vertical;
            _isHorizontal = _scrollRect.horizontal;
        }

        public void Subscribe()
        {
            _scrollRect.onValueChanged.AddListener(Check);
            IsSubscribed = true;
        }

        public void Unsubscribe()
        {
            if (_scrollRect != null)
                _scrollRect.onValueChanged.RemoveListener(Check);
            IsSubscribed = false;
        }

        public void UpdateValues()
        {
            if (!isInit)
                isInit = true;

            ITableViewContentDataSource dataSource = _tableView.m_dataSource;
            Debug.Log($"Count {dataSource.GetNumberOfRows(_tableView)}");
            cellStates = new bool[dataSource.GetNumberOfRows(_tableView)];

            if (_srRTransform)
            {
                Rect rect = _srRTransform.rect;
                Vector2 sizeDelta = _tableView.LayoutGroup.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
                _disableMarginX = rect.width / 2 + sizeDelta.x;
                _disableMarginY = rect.height / 2 + sizeDelta.y;
            }
        }

        public void SetEnabledLayoutGroupState(bool isEnabled)
        {
            _tableView.LayoutGroup.enabled = isEnabled;
            if (_tableView.LayoutGroup.TryGetComponent(out ContentSizeFitter fitter))
                fitter.enabled = isEnabled;
        }

        public void Check(Vector2 value = default)
        {
            if (!isInit)
                return;

            foreach (UITableViewCell cell in _tableView.Cells)
                UpdateCellState(cell);
        }

        private void UpdateCellState(UITableViewCell cell)
        {
            RectTransform rect = cell.Rect;

            Vector3 inverseTrPoint = _srRTransform.InverseTransformPoint(rect.position);
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
            _tableView.OnWillDisplay(_tableView, cell, indexPath);
        }
    }

    public interface IOcclussionModel
    {
        bool IsSubscribed { get; }
        void Init(UITableView _tableView);
        void Subscribe();
        void Unsubscribe();
        void UpdateValues();
        void Check(Vector2 value = default);
        void SetEnabledLayoutGroupState(bool isEnabled);
    }
}