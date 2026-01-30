using NUnit.Framework;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class subtitlesScript : MonoBehaviour
{
    public static Text txt;
    public static Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        txt = GetComponent<Text>();
    }

    public static IEnumerator subtitleStart(string[] subtitles, float time, float beetwen, float specialCase)
    {
        yield return new WaitForSeconds(time);
        anim.Play("fadeInSubtitles");
        for (int i = 0; i < subtitles.Length; i++)
        {
            txt.text = "~ " + subtitles[i] + " ~";
            if(specialCase > 0 && i == subtitles.Length - 1) yield return new WaitForSeconds(specialCase);
            else yield return new WaitForSeconds(beetwen);
        }
        anim.Play("fadeOutSubtitles");
    }
}
