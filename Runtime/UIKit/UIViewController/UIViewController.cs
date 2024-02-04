using UnityEngine;
using UnityEngine.UI;
using Extension.UI;

namespace CastlesTrip.UIKit
{
    public enum ViewControllerPresentType
    {
        none, fromUp, fromDown, fromLeft, fromRight
    }

    public delegate void CloseHandlerUIViewController(bool canClose);

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

        public ViewControllerPresentType PresentType
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
            if (anim.PresentType == ViewControllerPresentType.none) { Destroy(gameObject); return; }
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
                if (anim.PresentType == ViewControllerPresentType.none)
                    return;

                anim.PresentAnimation(true, null);
                return;
            }

            if (anim.PresentType == ViewControllerPresentType.none)
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

    public interface IUIViewController
    {
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void Awake();
        public abstract void Start();
        public abstract void OnRenderSuccess();
        public abstract void OnDestroy();
        public abstract void OnBackTapped();
        public abstract void UpdateContent();
    }

    public struct Anchors
    {
        public Vector2 min;
        public Vector2 max;

        public Anchors(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }
    }

    public class UIViewControllerAnimation
    {
        private RectTransform m_contentRT;
        private RectTransform contentRT => m_contentRT;

        private ViewControllerPresentType presentType = ViewControllerPresentType.none;
        public ViewControllerPresentType PresentType
        {
            get
            {
                return presentType;
            }

            set
            {
                presentType = value;
                target = GetStartAnchors(presentType);
                // TODO: Разобраться нужно ли
                //UpdatePosition();
            }
        }
        public virtual float CloseWaitTime => presentType != ViewControllerPresentType.none ? 0.8f : 0f;

        private Anchors target = new Anchors();

        public void Init(RectTransform rect)
        {
            m_contentRT = rect;
        }

        public void PresentAnimation(bool isPresent, CompletionEH completion)
        {
            ProgressChangedEH handler = GetHandler(isPresent);
            Animation anim = new Animation();
            anim.AnimTime = CloseWaitTime;
            anim.AnimType = AnimationType.Pow2AnimationType;
            anim.StartAnim(handler, completion);
            if (!isPresent) return;
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            contentRT.anchorMin = target.min;
            contentRT.anchorMax = target.max;
        }

        private Anchors GetStartAnchors(ViewControllerPresentType presentType)
        {
            switch (presentType)
            {
                case ViewControllerPresentType.fromUp: return new Anchors(new Vector2(0f, 1f), new Vector2(1f, 2f));
                case ViewControllerPresentType.fromRight: return new Anchors(new Vector2(1f, 0f), new Vector2(2f, 1f));
                case ViewControllerPresentType.fromDown: return new Anchors(new Vector2(0f, -1f), new Vector2(1f, 0f));
                case ViewControllerPresentType.fromLeft: return new Anchors(new Vector2(-1f, 0f), new Vector2(0f, 1f));
                default: return new Anchors(Vector2.zero, Vector2.one);
            }
        }

        private ProgressChangedEH GetHandler(bool isPresent)
        {
            ProgressChangedEH handler = delegate (float progress, float scale)
            {
                if (!contentRT) return;
                contentRT.anchorMin = Vector2.Lerp(contentRT.anchorMin, isPresent ? Vector2.zero : target.min, scale);
                contentRT.anchorMax = Vector2.Lerp(contentRT.anchorMax, isPresent ? Vector2.one : target.max, scale);
            };
            return handler;
        }
    }
}