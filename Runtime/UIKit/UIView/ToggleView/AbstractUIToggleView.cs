using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public abstract class AbstractUIToggleView : UIView, IAbstractUIToggleView
    {
        [SerializeField]
        private Button _mainButton;

        public event OnToggleStateChanged OnToggleStateChanged;

        public bool State { get; private set; }

        public abstract void UpdateState(AbstractUIToggleView view, bool state);


        public override void OnEnable()
        {
            _mainButton?.onClick.AddListener(OnMainTapped);
            base.OnEnable();
        }

        public override void OnDisable()
        {
            _mainButton?.onClick.RemoveListener(OnMainTapped);
            base.OnDisable();
        }

        public override void Start()
        {
            UpdateState(this, State);
            base.Start();
        }

        private void OnMainTapped()
        {
            bool canSwitch = CanSwitch(this);
            if (!canSwitch)
                return;

            State = !State;
            OnToggleStateChanged?.Invoke(this, State);
            UpdateState(this, State);
        }

        public virtual void SetState(bool state)
        {
            State = state;
            UpdateState(this, State);
        }

        public virtual bool CanSwitch(AbstractUIToggleView toggleView)
        {
            return true;
        }
    }
}