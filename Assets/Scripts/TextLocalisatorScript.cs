using UnityEngine;
using UnityEngine.UI;

public class TextLocalisatorScript : MonoBehaviour
{
    Text text;
    int fontSize;

    [SerializeField]
    private string key;

    [SerializeField]
    private float germanFontSizeModificator = 0.5f, italianFontSizeModificator = 0.8f;
    public bool pronemodify;

    private void Awake()
    {
        text = GetComponent<Text>();
        fontSize = text.fontSize;
    }

    private void Start()
    {
        OnLanguageChange();
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
        if (text != null)
        {
            text.text = LanguageData.instance.getCurrentLangText(key);
            if (!pronemodify)
            {
                if (LanguageData.currLanguage == LanguageData.Languages.DE) text.fontSize = (int)(fontSize * germanFontSizeModificator);
                else if (LanguageData.currLanguage == LanguageData.Languages.IT) text.fontSize = (int)(fontSize * italianFontSizeModificator);
                else text.fontSize = fontSize;
            }
            else text.fontSize = fontSize;
        }
    }
}
