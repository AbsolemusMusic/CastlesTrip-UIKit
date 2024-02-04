using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Text))]
public class LocaleTextView : LocaleAbstractView
{
    [SerializeField]
    private Text targetText;

    [SerializeField]
    private string valueUS = "";
    [SerializeField]
    private string valueOther = "";

    public override void UpdateLocaleState(bool isLocale)
    {
        if (!targetText)
            targetText = GetComponent<Text>();
        targetText.text = isLocale ? valueUS : valueOther;
    }
}