namespace CT.UIKit
{
    public interface IUIViewControllerMono
    {
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void Awake();
        public abstract void Start();
        public abstract void OnDestroy();
    }
}