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

    public bool isOut;
    public static bool isIn = false;

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
        if (text == null || image == null || fontSize == null) return;
        isIn = true;
        scrollText.text = text;
        scrollText.fontSize = fontSize;
        headerImage.sprite = image;
        Time.timeScale = 0;
        animator.Play("ScrollRollIn");
        StartCoroutine(toggleOut());
    }

    public void rollOutScroll()
    {
        Time.timeScale = 1;
        animator.Play("ScrollRollOut");
        StartCoroutine(toggleOut());
        isIn = false;
    }

    private IEnumerator toggleOut()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        isOut = !isOut;
    }
}
