using System.Collections.Generic;
using UnityEngine;

public class ObjectsLocaleView : LocaleAbstractView
{
    [SerializeField]
    private List<GameObject> localeObjects;

    [SerializeField]
    private List<GameObject> otherObjects;

    public override void UpdateLocaleState(bool isLocale)
    {
        SetObjectsState(localeObjects, isLocale);
        SetObjectsState(otherObjects, !isLocale);
    }

    private void SetObjectsState(List<GameObject> objs, bool state)
    {
        foreach (GameObject obj in objs)
            obj?.SetActive(state);
    }
}