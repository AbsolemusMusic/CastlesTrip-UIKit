using System;
using System.Collections.Generic;
using UnityEngine;

namespace CT.UIKit
{
    public class UITabbarView : MonoBehaviour, IUITabbarView
    {
        [SerializeField]
        private List<UITabbarButton> buttons;

        private int selectedID = 0;
        public int SelectedID => selectedID;

        public int CountOfItems => buttons.Count;

        public event Action<int, bool> OnTabbarButtonTapped;

        public virtual void Start()
        {
            foreach (UITabbarButton tabbar in buttons)
            {
                tabbar.OnClicked += OnTabbarTapped;
            }
        }

        public virtual void Select(int index)
        {
            OnTabbarTapped(buttons[index]);
        }

        public virtual void OnTabbarTapped(UITabbarButton obj)
        {
            foreach (UITabbarButton tabbar in buttons)
            {
                int id = buttons.IndexOf(tabbar);
                bool state = obj.Equals(tabbar);
                tabbar.SetSelected(state);
                if (!state)
                    continue;

                selectedID = id;
                OnTabbarButtonTapped?.Invoke(id, state);
                id++;
            }
        }
    }
}