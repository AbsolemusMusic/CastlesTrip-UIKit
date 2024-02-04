using System;

public interface ITabbarView
{
    public int CountOfItems { get; }
    event Action<int, bool> OnTabbarButtonTapped;
}