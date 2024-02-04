using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TabbarButton : MonoBehaviour, ITabbarButton
{
    [SerializeField]
    private Button mainButton;

    [SerializeField]
    private Image iconImg;

    [SerializeField]
    private Sprite activeSprite;

    [SerializeField]
    private Sprite inactiveSprite;

    public event Action<TabbarButton> OnClicked;

    private void Start()
    {
        mainButton.onClick.AddListener(OnMainTapped);
    }

    private void OnDestroy()
    {
        mainButton.onClick.RemoveListener(OnMainTapped);
    }

    public virtual void SetSelected(bool state)
    {
        try
        {
            iconImg.sprite = state ? activeSprite : inactiveSprite;
        }
        catch { }
    }

    private void OnMainTapped()
    {
        OnClicked?.Invoke(this);
    }
}