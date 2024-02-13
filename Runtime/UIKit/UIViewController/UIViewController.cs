using UnityEngine;
using UnityEngine.UI;
using Extension.UI;

namespace CT.UIKit
{
    public class UIViewController : MonoBehaviour, IUIViewController
    {
        [SerializeField]
        private Button backButton;

        private UIViewController previousVC;

        private UIViewControllerAnimation anim = new UIViewControllerAnimation();
        private Canvas canvas;
        public Canvas Canvas
        {
            get
            {
                if (!canvas)
                {
                    canvas = GetComponent<Canvas>();
                }
                return canvas;
            }
        }

        private CanvasScaler _canvasScaler;
        public CanvasScaler CanvasScaler
        {
            get
            {
                if (!_canvasScaler)
                    _canvasScaler = GetComponent<CanvasScaler>();
                return _canvasScaler;
            }
        }

        public UIViewControllerPresentType PresentType
        {
            set
            {
                anim.PresentType = value;
            }
        }

        [SerializeField]
        private RectTransform contentViewRectTr;

        public virtual void Awake()
        {
            gameObject.SetActive(false);
            if (!contentViewRectTr)
                contentViewRectTr = transform.Find("ContentView").GetComponent<RectTransform>();
            anim.Init(contentViewRectTr);

            if (!backButton) return;
            backButton.onClick.AddListener(delegate { OnBackTapped(); });
            AddBlockRaycasts();
        }

        public virtual void OnEnable()
        {
            UpdatePosition();
        }

        public virtual void OnDisable()
        {

        }

        public virtual void Start()
        {
            var waiter = new WaitFramesModel(1);
            waiter.Start(delegate
            {
                OnRenderSuccess();
            });
        }

        public virtual void OnRenderSuccess()
        {

        }

        public virtual void OnDestroy()
        {

        }

        public virtual void OnBackTapped()
        {
            CloseHandlerUIViewController handler = delegate (bool canClose)
            {
                if (!canClose) return;
                DidClosed();
            };

            TryClose(handler);
        }

        public virtual void UpdateContent()
        {

        }

        private void UpdatePosition()
        {
            if (TryGetComponent(out RectTransform rectTr))
            {
                rectTr.AnchorReset();
            }
        }

        private void AddBlockRaycasts()
        {
            GameObject block = new GameObject("BlockRaycasts");
            Image blockImg = block.AddComponent<Image>();
            blockImg.color = Color.clear;
            RectTransform blockRT = block.GetComponent<RectTransform>();
            blockRT.SetParent(transform);
            blockRT.SetSiblingIndex(0);
            blockRT.Reset();
            blockRT.AnchorReset();
        }

        public virtual void Present(UIViewController currentVC)
        {
            previousVC = currentVC;
            Present();
        }

        public virtual void Present(bool isShow = true)
        {
            if (!isShow)
                return;

            Show(true);
        }

        public virtual void TryClose(CloseHandlerUIViewController handler)
        {
            handler?.Invoke(true);
        }

        public virtual void DidClosed()
        {
            if (anim.PresentType == UIViewControllerPresentType.none) { Destroy(gameObject); return; }
            CompletionEH completion = delegate
            {
                if (this == null) return;
                gameObject.SetActive(false);
                Destroy(gameObject);
            };
            anim.PresentAnimation(false, completion);

            UpdatePrevViewController();
        }

        public virtual void Show(bool isShow)
        {
            if (isShow)
            {
                gameObject.SetActive(true);
                if (anim.PresentType == UIViewControllerPresentType.none)
                    return;

                anim.PresentAnimation(true, null);
                return;
            }

            if (anim.PresentType == UIViewControllerPresentType.none)
            {
                gameObject.SetActive(false);
                return;
            }

            CompletionEH completion = delegate
            {
                if (this == null) return;
            };

            anim.PresentAnimation(false, completion);

            UpdatePrevViewController();
        }

        private void UpdatePrevViewController()
        {
            if (!previousVC)
                return;

            previousVC.UpdateContent();
        }
    }
}