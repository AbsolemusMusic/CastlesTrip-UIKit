using UnityEngine;
using UnityEngine.UI;

namespace CT.UIKit
{
    public interface IUIViewController : IUIViewControllerMono
    {
        Canvas Canvas { get; }
        CanvasScaler CanvasScaler { get; }
        UIViewControllerPresentType PresentType { set; }

        void OnRenderSuccess();
        void OnBackTapped();
        void UpdateContent();

        void Present(UIViewController currentVC);
        void Present(bool isShow = true);
        void Show(bool isShow);

        void TryClose(CloseHandlerUIViewController handler);
        void DidClosed();
    }
}