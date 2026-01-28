using UnityEngine;
using UnityEngine.UI;

public class TextLocalisatorScript : MonoBehaviour
{
    Text text;
    int fontSize;

    [SerializeField]
    private string key;

    [SerializeField]
    private float germanFontSizeModificator = 0.5f;
    private void Awake()
    {
        text = GetComponent<Text>();
        fontSize = text.fontSize;
        print(fontSize);
    }

    private void OnEnable()
    {
        LanguageData.onLanguageChange += OnLanguageChange;
    }

    private void OnDisable()
    {
        LanguageData.onLanguageChange -= OnLanguageChange;
    }
    private void OnLanguageChange()
    {
        text.text = LanguageData.instance.getCurrentLangText(key);
        if (LanguageData.currLanguage == LanguageData.Languages.DE) text.fontSize = (int)(fontSize * germanFontSizeModificator);
        else text.fontSize = fontSize;
    }
}
