using UnityEngine;

public class TutorialNoteScript : MonoBehaviour
{
    [Multiline] public string noteText;
    public Sprite headerImage;
    public int fontSize;
    public string key;

    private void Awake()
    {
        noteText = LanguageData.instance.getCurrentLangText(key);
        if (key == "second_note")
        {
            if(LanguageData.currLanguage == LanguageData.Languages.DE || LanguageData.currLanguage == LanguageData.Languages.ES) fontSize = 131;
            else if (LanguageData.currLanguage == LanguageData.Languages.FR || LanguageData.currLanguage == LanguageData.Languages.IT) fontSize = 124;
        }
        //if (LanguageData.currLanguage == LanguageData.Languages.DE) fontSize = (int)(fontSize * 0.5f);
        //else if (LanguageData.currLanguage == LanguageData.Languages.IT) fontSize = (int)(fontSize * 0.8f);
        //else fontSize = fontSize;
    }
}
