namespace CT.UIKit
{
    public interface IUIViewController : IUIViewControllerMono
    {
        public abstract void OnRenderSuccess();
        public abstract void OnBackTapped();
        public abstract void UpdateContent();
    }
}