using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StyleManager : MonoBehaviour
{
    enum Style { Forsaken = 0, Exorcist = 1, Dominating = 2, Chastising = 3, Bane = 4, Hellbound = 5, Angelic = 6}

    public static StyleManager instance;

    public int stylePoints;

    public int totalStylePoints;

    [SerializeField]
    private Slider styleSlider;
    private float styleCurVel = 0;

    [SerializeField]
    private Animator styleAnim;

    [SerializeField]
    private Text styleText;
    private Style currStyle;

    [SerializeField]
    private Image letterOutline, letterFill;

    public UIShakeSelf shaker, textShaker;

    [SerializeField]
    private Sprite[] letterOutlines, letterFills;

    [SerializeField]
    private PlayerMovement playerMovement;

    public Image fillImage;

    public bool isAngelic = false;

    Color lightGrey;
    Color darkGrey;

    Color startColor;
    Color endColor;

    private float startTime = 0f;

    bool wasLosingstyle = false;

    string lastPlayedAnim;

    public GameObject Ascend;
    public Animator ascentionanimator;

    public static bool canAscend;
    public static bool shouldTurnOff = false;

    private void Start()
    {
        lightGrey = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        darkGrey = new Color(0.3f, 0.3f, 0.3f, 1f);
        instance = this;
        currStyle = Style.Forsaken;
        fillImage.color = Color.red;
        switchStyle();
        StartCoroutine(styleLife());
        PlayerMovement playerMovement = GameObject.Find("Player(Clone)").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        StartCoroutine(ascentiontextcontrol());

        if (!playerMovement.shouldGainStyle && !playerMovement.shouldLoseStyle && !PlayerMovement.hasAscendedonce)
        {
            if (!isAngelic) changeColors("red-lightgrey");
            else changeColors("yellow-lightgrey");
        }

        /*
        else if (playerMovement.shouldGainStyle && !isAngelic)
        {
            if (wasLosingstyle) changeColors("darkGrey-red");
            //else fillImage.color = Color.red;
        }
        */
       
        handleSlider();

        if(stylePoints >= 30 && currStyle != Style.Angelic)
        {
            currStyle++;
            styleAnim.Play("styleUp");
            switchStyle();
            stylePoints = 0;
        }
        else if (stylePoints <= -2 && currStyle != Style.Forsaken)
        {
            shrinkStyle();
        }

    }

    public IEnumerator ascentiontextcontrol()
    {
        if (isAngelic && stylePoints >= 30 && !shouldTurnOff)
        {
            canAscend = true;
            Ascend.SetActive(true);
        }
        else
        {
            canAscend = false;
            ascentionanimator.Play("AsecndTextDissapear");
            yield return new WaitForSeconds(1f);
            Ascend.SetActive(false);
        }
        if(shouldTurnOff)
        {
            canAscend = false;
            ascentionanimator.Play("AsecndTextDissapear");
            yield return new WaitForSeconds(1f);
            Ascend.SetActive(false);
        }
    }

    void changeColors(string anim)
    {
        if (lastPlayedAnim == anim) return;
        styleAnim.Play(anim);
        lastPlayedAnim = anim;
    }

    private void handleSlider()
    {
        float styleSliderValue = Mathf.SmoothDamp(styleSlider.value, stylePoints, ref styleCurVel, 0.08f);
        styleSlider.value = styleSliderValue;
    }

    private void switchStyle()
    {
        switch (currStyle)
        {
            case Style.Forsaken:
                switchLetter(Style.Forsaken);
                styleText.text = "Forsaken";
                isAngelic = false;
                //fillImage.color = Color.red;
                break;
            case Style.Exorcist:
                switchLetter(Style.Exorcist);
                styleText.text = "Exorcist";
                isAngelic = false;
                //fillImage.color = Color.red;
                break;
            case Style.Dominating:
                switchLetter(Style.Dominating);
                styleText.text = "Dominating";
                isAngelic = false;
                //fillImage.color = Color.red;
                break;
            case Style.Chastising:
                switchLetter(Style.Chastising);
                styleText.text = "Chastising";
                isAngelic = false;
                //fillImage.color = Color.red;
                break;
            case Style.Bane:
                switchLetter(Style.Bane);
                shaker.stopShake();
                textShaker.stopShake();
                styleText.text = "Bane";
                isAngelic = false;
                //fillImage.color = Color.red;
                break;
            case Style.Hellbound:
                switchLetter(Style.Hellbound);
                playerMovement.unreadyAngelic();
                shaker._distance = 1.4f;
                textShaker.Begin();
                shaker.Begin();
                styleText.text = "HELLBOUND!";
                isAngelic = false;
                //fillImage.color = Color.red;
                break;
            case Style.Angelic:
                switchLetter(Style.Angelic);
                playerMovement.readyAngelic();
                shaker._distance = 2f;
                shaker.Begin();
                styleText.text = "ANGELIC!!!";
                isAngelic = true;
                //fillImage.color = Color.yellow;
                break;
        }

    }

    private void switchLetter(Style style) 
    {
        letterOutline.sprite = letterOutlines[(int)style];
        letterFill.sprite = letterFills[(int)style];
    }

    public void reset()
    {
        StopAllCoroutines();

        // Reset all variables to their starting state
        stylePoints = 0;
        totalStylePoints = 0;
        currStyle = Style.Forsaken;
        isAngelic = false;
        shouldTurnOff = false;
        canAscend = false;
        wasLosingstyle = false;
        lastPlayedAnim = "";

        // Reset UI and visuals
        fillImage.color = Color.red;
        styleSlider.value = 0;
        styleAnim.Play("red-lightgrey");
        styleText.text = "Forsaken";
        letterOutline.sprite = letterOutlines[(int)Style.Forsaken];
        letterFill.sprite = letterFills[(int)Style.Forsaken];

        // Stop any shaking or animations that were active
        shaker.stopShake();
        textShaker.stopShake();

        // Turn off Ascend text and reset related flags
        if (Ascend != null)
        {
            Ascend.SetActive(false);
            ascentionanimator.Play("AsecndTextDissapear");
        }

        // Reset player’s angelic readiness
        if (playerMovement != null)
        {
            playerMovement.unreadyAngelic();
        }

        // Restart the main style coroutine
        StartCoroutine(styleLife());
    }

    private IEnumerator styleLife()
    {
        yield return new WaitForSeconds(0.3f);
        if (playerMovement.shouldLoseStyle && !playerMovement.shouldGainStyle)
        {
            changeColors("lightgrey-darkgrey");
            if (stylePoints > -3) stylePoints--;
        }
        else
        {
            if (!isAngelic) changeColors("darkgrey-red");
            else
            {
                if(!shouldTurnOff) changeColors("darkgrey-yellow");
            }
        }

        /*
        else if (!playerMovement.shouldLoseStyle && playerMovement.shouldGainStyle)
        {
            fillImage.color = Color.red;
        }
        else if (!playerMovement.shouldGainStyle && !playerMovement.shouldLoseStyle)
        {
            fillImage.color = lightGrey;
        }
        */

        StartCoroutine(styleLife());
    }

    public void shrinkStyle()
    {
        currStyle--;
        switchStyle();
        stylePoints = 19;
    }

    public void growStyle(int ptsToAdd)
    {
        styleAnim.Play("grow");
        stylePoints += ptsToAdd;
        stylePoints = Mathf.Clamp(stylePoints, -3, 30);
    }
}

