using System.Collections.Generic;
using UnityEngine;

namespace CT.UIKit
{
    public abstract class UITabbarViewController : UIViewController, IUITabbarViewController
    {
        public virtual IUITabbarView tabbarView { get; }

        public abstract bool IsHorizontal { get; }

        [SerializeField]
        private RectTransform parentVCRectTr;
        private bool isInited;
        private int currentID = -1;
        public int CurrentID => currentID;

        private List<UIViewController> viewControllers = new List<UIViewController>();

        private UIViewControllerPresentType[] horizontal = new UIViewControllerPresentType[] { UIViewControllerPresentType.fromRight, UIViewControllerPresentType.fromLeft };
        private UIViewControllerPresentType[] vertical = new UIViewControllerPresentType[] { UIViewControllerPresentType.fromDown, UIViewControllerPresentType.fromUp };
        private UIViewControllerPresentType[] targetPresentTypes => IsHorizontal ? horizontal : vertical;

        public override void Present(bool isShow = true)
        {
            tabbarView.OnTabbarButtonTapped += TabbarView_OnTabbarButtonTapped;
            base.Present(isShow);
        }

        public override void OnDestroy()
        {
            tabbarView.OnTabbarButtonTapped -= TabbarView_OnTabbarButtonTapped;
            base.OnDestroy();
        }

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < tabbarView.CountOfItems; i++)
            {
                UIViewController loadVC = GetViewControllerForTabbar(i);
                loadVC.PresentType = GetPresentType(i != 0);
                PresentVC(loadVC, i);
                viewControllers.Add(loadVC);
                if (i == tabbarView.SelectedID)
                {
                    loadVC.Present(this);
                    continue;
                }
                loadVC.Present(false);
            }

            tabbarView.Select(tabbarView.SelectedID);
        }

        private void PresentVC(UIViewController vc, int index)
        {
            RectTransform rectTR = vc.GetComponent<RectTransform>();
            rectTR.SetParent(parentVCRectTr);
            rectTR.Reset();
        }

        private void TabbarView_OnTabbarButtonTapped(int targetID, bool state)
        {
            if (currentID == targetID)
                return;

            if (!CanSelect(targetID))
                return;

            bool isRightTarget = currentID < targetID;
            UIViewController targetVC = viewControllers[targetID];
            targetVC.PresentType = GetPresentType(isRightTarget);

            if (!isInited)
            {
                currentID = targetID;
                targetVC.PresentType = UIViewControllerPresentType.none;
                isInited = true;
                targetVC.Show(true);
                OnSelected(currentID);
                return;
            }

            UIViewController last = viewControllers[currentID];
            last.PresentType = GetPresentType(!isRightTarget);
            last.Show(false);
            targetVC.Show(true);
            currentID = targetID;
            OnSelected(currentID);
        }

        private UIViewControllerPresentType GetPresentType(bool isNext)
        {
            int id = isNext ? 0 : 1;
            return targetPresentTypes[id];
        }

        public UIViewController GetTabbarViewController()
        {
            if (currentID < 0)
                return null;

            return GetTabbarViewController(currentID);
        }

        public UIViewController GetTabbarViewController(int itemID)
        {
            try
            {
                return viewControllers[itemID];
            }
            catch { }

            return null;
        }
        public abstract bool CanSelect(int itemID);
        public abstract UIViewController GetViewControllerForTabbar(int itemID);
        public virtual void OnSelected(int itemID) { }
    }

    public interface IUITabbarViewController
    {
        abstract UIViewController GetViewControllerForTabbar(int itemID);
        abstract bool CanSelect(int itemID);
        abstract void OnSelected(int itemID);
    }
}