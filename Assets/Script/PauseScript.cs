using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public GameObject Scroll;

    public static bool Paused = false;

    public GameObject Fadeout;

    public Text kills, damage, time;
    public static float kill, dmg, dro;

    string scenename;

    public bool alreadyshown = false;

    float minutes, seconds;

    public GameObject TLOH;

    private void Start()
    {
        scenename = SceneManager.GetActiveScene().name;

        if (scenename == "LashaiasScena" || scenename == "TenthLayerOfHell")
        {
            kill = 0;
            dmg = 0;
            dro = 0;
        }

        if(scenename == "TenthLayerOfHell")
        {
            StartCoroutine(TLOHCoroutine());
        }

        StartCoroutine(Timer());
    }

    public IEnumerator TLOHCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        TLOH.SetActive(true);   
    }

    public IEnumerator Timer()
    {
        while (true)
        {
            if (!Paused)
            {
                yield return new WaitForSeconds(1f);
                dro++;
                if (scenename != "LashaiasScene" && scenename != "TenthLayerOfHell")
                    break;
            }
        }
    }

    private void Update()
    {
        if (scenename == "LashaiasScene" || scenename == "TenthLayerOfHell")
        {
            if (Input.GetKeyDown(KeyCode.Escape) && PlayerMovement.canPause)
            {
                if (!Paused)
                {
                    Scroll.SetActive(true);
                    Time.timeScale = 0f;
                    Paused = true;
                }
                else
                {
                    Scroll.SetActive(false);
                    Time.timeScale = 1f;
                    Paused = false;
                }
            }
        }
        else if(scenename != "LashaiasScene" && !alreadyshown || scenename != "TenthLayerOfHell" && !alreadyshown)
        {
            StartCoroutine(death());
        }
    }

    private IEnumerator MainMenu()
    {
        Fadeout.SetActive(true);
        yield return new WaitForSecondsRealtime(0.95f);
        SceneManager.LoadScene(1);
    }

    public void YesMainMenu()
    {
        StartCoroutine(MainMenu());
    }

    public void No()
    {
        Scroll.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public IEnumerator death()
    {
        alreadyshown = true;
        kills.text = "" + kill;
        damage.text = "" + dmg;
        int totalseconds = (int)dro;
        int minutes = totalseconds / 60;
        int seconds = totalseconds % 60;
        time.text = minutes + ":" + seconds.ToString("00");
        yield return new WaitForSeconds(5f);
        Fadeout.SetActive(true);
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene(1);
    }
}
