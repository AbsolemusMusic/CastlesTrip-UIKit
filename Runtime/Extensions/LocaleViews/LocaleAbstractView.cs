using UnityEngine;

public abstract class LocaleAbstractView : MonoBehaviour
{
    private bool isLocale
    {
        get
        {
            bool _isUS = "US".Equals(System.Globalization.RegionInfo.CurrentRegion.ToString().ToUpper());
            bool _isEN = SystemLanguage.English.Equals(Application.systemLanguage);
            return _isUS && _isEN;
        }
    }

    private void OnEnable()
    {
        UpdateLocaleState(isLocale);
    }

    private void OnDisable()
    {
        UpdateLocaleState(isLocale);
    }

    public abstract void UpdateLocaleState(bool isLocale);
}