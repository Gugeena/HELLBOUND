using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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

    public Image fill;

    bool hasalreadydonethis = false;

    bool shouldShow = true;
    public Transform visualizerLocation, multiplierLocation;
    public Text visualizer;
    public GameObject Canvas;

    public GlobalSettings globalSettings;

    public bool canMultiply = true;

    public GameObject LoopMultiplier, RecklessMultiplier, lawnmowerMultiplier, mayhemultiplier, richochetmultiplier, aerialMultiplier;

    public bool isTutorial;

    private void Start()
    {
        lightGrey = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        darkGrey = new Color(0.3f, 0.3f, 0.3f, 1f);
        instance = this;
        currStyle = Style.Forsaken;
        fillImage.color = Color.red;
        switchStyle();
        StartCoroutine(styleLife());
        playerMovement = GameObject.Find("Player(Clone)").GetComponent<PlayerMovement>();
        PlayerMovement.hasAscendedonce = false;
        hasalreadydonethis = false;
        isAngelic = false;
        shouldTurnOff = false;
        globalSettings = SaveSystem.Load();
        canMultiply = globalSettings.information.visualizer == 1 ? true : false;
        StartCoroutine(ascentiontextcontrol());
    }

    private void Update()
    {
        if(stylePoints != Mathf.RoundToInt(styleSlider.value) && shouldShow && stylePoints > 0 && stylePoints > styleSlider.value &&    globalSettings.information.visualizer == 1)
        {
            StartCoroutine(styleVisualizer());
        }

        if (PlayerMovement.hasAscendedonce && !hasalreadydonethis)
        {
            styleAnim.enabled = false;
            shaker.stopShake();
            stylePoints = 70;
            styleSlider.value = stylePoints;
            currStyle = Style.Angelic;
            switchLetter(Style.Angelic);
            playerMovement.readyAngelic();
            shaker._distance = 2f;
            shaker.Begin();
            styleText.text = "ANGELIC!!!";
            isAngelic = true;
            fill.color = Color.yellow;
            hasalreadydonethis = true;
            return;
        }

        if(hasalreadydonethis)
        {
            return;
        }

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

        if(stylePoints >= 70 && currStyle != Style.Angelic)
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

    public IEnumerator styleVisualizer()
    {
        if (isTutorial) yield break;
        shouldShow = false;
        int gain = (int)(stylePoints - styleSlider.value);
        gain = Mathf.Clamp(gain, 1, 99);
        Text visualizer = Instantiate(this.visualizer, Canvas.transform);
        visualizer.text = "+" + gain;
        yield return new WaitForSeconds(0.2f);
        shouldShow = true;
    }

    public IEnumerator Multiplicator(int multiple)
    {
        if (!canMultiply || PlayerMovement.hasAscendedonce || isTutorial) yield break;
        //canMultiply = false;
        GameObject toSpawn = LoopMultiplier;
        if (multiple == 1) toSpawn = RecklessMultiplier;
        else if (multiple == 2) toSpawn = lawnmowerMultiplier;
        else if (multiple == 3) toSpawn = mayhemultiplier;
        else if (multiple == 4) toSpawn = richochetmultiplier;
        else if (multiple == 5) toSpawn = aerialMultiplier;
        Instantiate(toSpawn, multiplierLocation.transform.position, Quaternion.identity, Canvas.transform);
        yield return new WaitForSeconds(0.2f);
        canMultiply = true;
    }

    public void undisputed(int multiple)
    {
        StartCoroutine(Multiplicator(multiple));
    }

    public IEnumerator ascentiontextcontrol()
    {
        while (true)
        {
            /*
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("hasascended: " + PlayerMovement.hasAscendedonce + ", isAngelic: " + isAngelic + ", stylePoints: " + stylePoints + ", shouldturnoff: " + shouldTurnOff + ", active: " + Ascend.activeSelf);
            }
            */
            while (!PlayerMovement.hasAscendedonce)
            {
                if (isAngelic && stylePoints >= 70 && !shouldTurnOff)
                {
                    if (!Ascend.activeSelf)
                    {
                        canAscend = true;
                        Ascend.SetActive(true);
                    }
                }
                else
                {
                    if (Ascend.activeSelf)
                    {
                        canAscend = false;
                        ascentionanimator.Play("AsecndTextDissapear");
                        yield return new WaitForSeconds(1f);
                        Ascend.SetActive(false);
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
            canAscend = false;
            ascentionanimator.Play("AsecndTextDissapear");
            yield return new WaitForSeconds(1f);
            Ascend.SetActive(false);
            yield break;
        }
    }

    void changeColors(string anim)
    {
        if (lastPlayedAnim == anim) return;
        styleAnim.Play(anim);
        lastPlayedAnim = anim;
    }

    private void FixedUpdate()
    {
        handleSlider();
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
        stylePoints = 0;
        totalStylePoints = 0;
        currStyle = Style.Forsaken;
        isAngelic = false;
        shouldTurnOff = false;
        canAscend = false;
        wasLosingstyle = false;
        lastPlayedAnim = "";
        fillImage.color = Color.red;
        styleSlider.value = 0;
        styleAnim.Play("red-lightgrey");
        styleText.text = "Forsaken";
        letterOutline.sprite = letterOutlines[(int)Style.Forsaken];
        letterFill.sprite = letterFills[(int)Style.Forsaken];
        shaker.stopShake();
        textShaker.stopShake();
        if (Ascend != null)
        {
            Ascend.SetActive(false);
            ascentionanimator.Play("AsecndTextDissapear");
        }
        if (playerMovement != null)
        {
            playerMovement.unreadyAngelic();
        }
        PlayerMovement.hasAscendedonce = false;
        hasalreadydonethis = false;
        StartCoroutine(styleLife());
    }

    private IEnumerator styleLife()
    {
        yield return new WaitForSeconds(0.3f);
        if (playerMovement.shouldLoseStyle && !playerMovement.shouldGainStyle && !PlayerMovement.hasAscendedonce)
        {
            changeColors("lightgrey-darkgrey");
            if (stylePoints > -3) stylePoints--;
        }
        else
        {
            if (!PlayerMovement.hasAscendedonce)
            {
                if (!isAngelic) changeColors("darkgrey-red");
                else
                {
                    if (!shouldTurnOff) changeColors("darkgrey-yellow");
                }
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
        if (isTutorial && currStyle == Style.Exorcist || playerMovement.tutorialLock) return;
        styleAnim.Play("grow");
        stylePoints += ptsToAdd;
        stylePoints = Mathf.Clamp(stylePoints, -3, 70);
    }
}

