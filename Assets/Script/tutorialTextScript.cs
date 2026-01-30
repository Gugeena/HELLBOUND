
using UnityEngine;

public class tutorialTextScript : MonoBehaviour
{
    private UnityEngine.UI.Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        LanguageData.TextData[0].Add("movement_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey) + " - Dash\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey) + " - Slide");
        LanguageData.TextData[0].Add("combat_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey) + " - Special\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey) + " - Drop");

        LanguageData.TextData[1].Add("movement_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey) + " - Ausweichen\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey) + " - Rutschen");
        LanguageData.TextData[1].Add("combat_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey) + " - Spezial\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey) + " - Wegwerfen");

        LanguageData.TextData[4].Add("movement_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey) + " - Carerra\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey) + " - Deslizar");
        LanguageData.TextData[4].Add("combat_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey) + " - Especial\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey) + " - Soltar");

        LanguageData.TextData[2].Add("movement_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey) + " - Fugir\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey) + " - Deslizar");
        LanguageData.TextData[2].Add("combat_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey) + " - Especial\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey) + " - Soltar");

        LanguageData.TextData[5].Add("movement_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey) + " - Ruee\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey) + " - Glisser");
        LanguageData.TextData[5].Add("combat_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey) + " - Special\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey) + " - Lacher");

        LanguageData.TextData[3].Add("movement_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey) + " - Scatto\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey) + " - Scivolare");
        LanguageData.TextData[3].Add("combat_tip", KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey) + " - Speciale\n" + KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey) + " - Lasciare");

    }
}
