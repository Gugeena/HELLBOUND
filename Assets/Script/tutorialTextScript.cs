using UnityEngine;

public class tutorialTextScript : MonoBehaviour
{
    private UnityEngine.UI.Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<UnityEngine.UI.Text>();

        text.text = "Drop weapon - " + PlayerMovement.instance.Drop
                    + "\nSpecial move (Spear & Macuahuitl) - " + PlayerMovement.instance.Special;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
