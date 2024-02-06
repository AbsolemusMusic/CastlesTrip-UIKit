using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public class OcclusionUITableView : UITableView
    {
        [SerializeField]
        private ScrollRect scrollRect;

        private Transform srTransform;
        private bool isInit;
        private bool _isVertical, _isHorizontal;
        private float _disableMarginX, _disableMarginY;

        private Vector2 offset = new Vector2(20, 20);
        public virtual Vector2 Offset => offset;

        private float WAIT = 0.1f;

        public override void ForceRebuildLayout(RectTransform parentRT)
        {
            base.ForceRebuildLayout(parentRT);

            _isVertical = scrollRect.vertical;
            _isHorizontal = scrollRect.horizontal;

            scrollRect.onValueChanged.AddListener(UpdateCellsState);

            // TODO: Костыль, что с этим делать непонятно
            Invoke("Init", WAIT);
        }

        private void Init()
        {
            srTransform = scrollRect.transform;

            if (scrollRect.TryGetComponent(out RectTransform rectTransform))
            {
                Rect rect = rectTransform.rect;
                Vector2 sizeDelta = Cells[0].Rect.sizeDelta;
                _disableMarginX = rect.width / 2 + sizeDelta.x;
                _disableMarginY = rect.height / 2 + sizeDelta.y;
            }

            SetEnabledState(false);

            isInit = true;
            UpdateCellsState(scrollRect.normalizedPosition);
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

            foreach (IUITableViewCell cell in Cells)
                UpdateCellState(cell);
        }

        private void UpdateCellState(IUITableViewCell cell)
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
                SetActiveState(rect, !isInactive);
                return;
            }

            if (_isVertical)
                isInactive = isLessOrGreatVertical;

            if (_isHorizontal)
                isInactive = isLessOrGreatHorizontal;

            SetActiveState(rect, !isInactive);
        }

        private void SetActiveState(RectTransform cellRT, bool isActive)
        {
            cellRT.gameObject.SetActive(isActive);
        }
    }
}
