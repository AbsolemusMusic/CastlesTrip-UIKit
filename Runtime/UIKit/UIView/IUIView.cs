namespace CT.UIKit
{
    public interface IUIView
    {
        void OnEnable();
        void OnDisable();
        void Awake();
        void Start();
        void OnDestroy();
    }
}