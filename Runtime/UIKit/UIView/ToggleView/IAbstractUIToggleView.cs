namespace CT.UIKit
{
    public interface IAbstractUIToggleView : IUIView
    {
        bool State { get; }
        bool CanSwitch(AbstractUIToggleView toggleView);
        void SetState(bool isActive);

        event OnToggleStateChanged OnToggleStateChanged;
    }
}