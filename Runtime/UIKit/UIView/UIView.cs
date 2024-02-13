using UnityEngine;

namespace CT.UIKit
{
    public class UIView : MonoBehaviour, IUIView
    {
        public virtual void Awake() { }

        public virtual void OnDestroy() { }

        public virtual void OnDisable() { }

        public virtual void OnEnable() { }

        public virtual void Start() { }
    }
}