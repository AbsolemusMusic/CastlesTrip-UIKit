using System;
using System.Collections.Generic;
using UnityEngine;

namespace CastlesTrip.UIKit
{
    public class TabbarView : MonoBehaviour, ITabbarView
    {
        [SerializeField]
        private List<TabbarButton> buttons;

        private int selectedID = 0;
        public int SelectedID => selectedID;

        public int CountOfItems => buttons.Count;

        public event Action<int, bool> OnTabbarButtonTapped;

        public virtual void Start()
        {
            foreach (TabbarButton tabbar in buttons)
            {
                tabbar.OnClicked += Tabbar_OnClicked;
            }
        }

        public virtual void Select(int index)
        {
            Tabbar_OnClicked(buttons[index]);
        }

        public virtual void Tabbar_OnClicked(TabbarButton obj)
        {
            foreach (TabbarButton tabbar in buttons)
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