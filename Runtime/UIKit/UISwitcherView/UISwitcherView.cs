using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public abstract class UISwitcherView : MonoBehaviour, IUISwitcherView
    {
        [SerializeField]
        private Button _leftButton;
        public Button LeftButton => _leftButton;

        [SerializeField]
        private Button _rightButton;
        public Button RightButton => _rightButton;

        public bool IsLeft { get; private set; }

        public abstract bool IsLeftStartState { get; }
        public abstract void SetSelected(bool isLeft, bool isClicked);

        public virtual void Awake()
        {
            _leftButton.onClick.AddListener(delegate() { SetSelected(true, true); });
            _rightButton.onClick.AddListener(delegate() { SetSelected(false, true); });

            SetSelected(IsLeftStartState, false);
        }

        public virtual void OnDestroy()
        {
            _leftButton.onClick.RemoveListener(delegate () { SetSelected(true, true); });
            _rightButton.onClick.RemoveListener(delegate () { SetSelected(false, true); });
        }
    }

    public interface IUISwitcherView
    {
        Button LeftButton { get; }
        Button RightButton { get; }

        bool IsLeft { get; }
        abstract bool IsLeftStartState { get; }
        abstract void SetSelected(bool isLeft, bool isClicked);
    }

    public interface IUISwitcherViewDelegate
    {
        bool CanSelected(bool isLeft);
    }
}