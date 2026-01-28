using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollScript : MonoBehaviour
{
    public static ScrollScript instance;

    [SerializeField] private Text scrollText;
    [SerializeField] private int fontSize;
    [SerializeField] private Image headerImage;

    private Animator animator;
    public Animator[] scrollAnimator;

    public bool isOut;
    public static bool isIn = false;
    public bool canOpen = true;
    public bool tutorialEnded;
    public bool isGoingOut;
    public Vector2 openingLocation;
    public float distanceToPlayer;
    public GameObject player;
    public Text tipText, tipTextMovement;

    private void Awake()
    {
        instance = this;
        isOut = false;
        animator = GetComponent<Animator>();
        isIn = false;
        isGoingOut = false;
    }

    private void Start()
    {
        StartCoroutine(tipwaiter());
    }

    private IEnumerator tipwaiter()
    {
        yield return null;
        if (tipText != null)
        {
            if (KeyBindManagerScript.instance == null) yield break;
            string special = KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.heavyKey);
            string drop = KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.DropKey);
            string slide = KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.slideKey);
            string dash = KeyBindManagerScript.instance.KeyToString(KeyBindManagerScript.dashKey);
            tipText.text = special + " - Special\n" + drop + " - Drop";
            tipTextMovement.text = dash + " - Dash\n" + slide + " - Slide";
        }
    }

    private void Update()
    {
        if(isOut)
        {
            distanceToPlayer = Vector2.Distance(player.transform.position, openingLocation);
            if (Input.GetKeyDown(KeyCode.Escape) || tutorialEnded && !isGoingOut || distanceToPlayer > 6f) rollOutScroll();
        }
    }

    public void rollInScroll(string text, Sprite image, int fontSize)
    {
        if (text == null || image == null || fontSize == null || !canOpen) return;
        openingLocation = player.transform.position;
        //isIn = true;
        scrollAnimator[0].Play("ScrollFadeOut", 1);
        scrollAnimator[1].Play("ScrollFadeOut", 1);
        canOpen = false;
        scrollText.text = text;
        scrollText.fontSize = fontSize;
        headerImage.sprite = image;
        // Time.timeScale = 0;
        animator.Play("ScrollRollIn");
        StartCoroutine(isOutTruer());
    }

    public void rollOutScroll()
    {
        if (!isOut) return;
        isGoingOut = true;
        if (tutorialEnded) tutorialEnded = false;
        Time.timeScale = 1;
        animator.Play("ScrollRollOut");
        StartCoroutine(toggleOut());
    }

    private IEnumerator isOutTruer()
    {
        yield return new WaitForSeconds(1.5f);
        isOut = true;
    }

    private IEnumerator toggleOut()
    {
        isOut = false;
        yield return new WaitForSecondsRealtime(1.5f);
        isGoingOut = false; 
        if (isOut) yield break;
        yield return new WaitForSeconds(1f);
        scrollAnimator[0].Play("ScrollActualFadeIn", 1);
        scrollAnimator[1].Play("ScrollActualFadeIn", 1);
        canOpen = true;
    }
}
