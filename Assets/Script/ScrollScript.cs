using System.Collections;
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

    private void Awake()
    {
        instance = this;
        isOut = false;
        animator = GetComponent<Animator>();
        isIn = false;
    }

    private void Update()
    {
        if(isOut && Input.GetKeyDown(KeyCode.Escape)) rollOutScroll();
    }

    public void rollInScroll(string text, Sprite image, int fontSize)
    {
        if (text == null || image == null || fontSize == null || !canOpen) return;
        //isIn = true;
        scrollAnimator[0].Play("ScrollFadeOut", 1);
        scrollAnimator[1].Play("ScrollFadeOut", 1);
        canOpen = false;
        scrollText.text = text;
        scrollText.fontSize = fontSize;
        headerImage.sprite = image;
        // Time.timeScale = 0;
        animator.Play("ScrollRollIn");
        StartCoroutine(toggleOut());
    }

    public void rollOutScroll()
    {
        if (!isOut) return;
        Time.timeScale = 1;
        animator.Play("ScrollRollOut");
        StartCoroutine(toggleOut());
    }

    private IEnumerator toggleOut()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        isOut = !isOut;
        if (isOut) yield break;
        yield return new WaitForSeconds(1f);
        scrollAnimator[0].Play("ScrollActualFadeIn", 1);
        scrollAnimator[1].Play("ScrollActualFadeIn", 1);
        canOpen = true;
    }
}
