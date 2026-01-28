using NUnit.Framework;
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

    public bool alreadyshown = false, alreadyDummied = false;

    float minutes, seconds;

    public GameObject TLOH, lilith, lilithDummy;

    public Transform OffShoot;

    public static Vector3 lastPosition;

    public UnityEngine.UI.Text tipText;

    private void Start()
    {
        Paused = false;

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
        if(LilithScript.stunned && !alreadyDummied)
        {
            alreadyDummied = true;
            StartCoroutine(lilithImpact());
        }

        if (scenename == "LashaiasScene" || scenename == "TenthLayerOfHell")
        {
            if (Input.GetKeyDown(KeyCode.Escape) && PlayerMovement.canPause)
            {
                if (!Paused)
                {
                    Scroll.SetActive(true);
                    Time.timeScale = 0f;
                    Paused = true;
                    AudioListener.pause = true;
                }
                else
                {
                    Scroll.SetActive(false);
                    Time.timeScale = 1f;
                    Paused = false;
                    AudioListener.pause = false;
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
        GlobalSettings globalsettings = SaveSystem.Load();
        if (!globalsettings.information.multiplierUnlocks.Equals(StyleManager.globalSettings.information.multiplierUnlocks)) SaveSystem.Save(StyleManager.globalSettings);
        Fadeout.SetActive(true);
        yield return new WaitForSecondsRealtime(0.95f);
        SceneManager.LoadScene(1);
    }

    public void YesMainMenu()
    {
        StartCoroutine(MainMenu());
    }

    private IEnumerator lilithImpact()
    {
        if (lilith == null) lilith = GameObject.Find("Lilith(Clone)");
        GameObject dummy = Instantiate(lilithDummy, lilith.transform.position, lilith.transform.rotation);
        dummy.transform.localScale = lilith.transform.localScale;
         
        Vector3 oldPosition = lilith.transform.position;
        lastPosition = lilith.transform.position;
        Quaternion oldRotation = lilith.transform.rotation;

        lilith.transform.position = OffShoot.position;

        yield return new WaitForSeconds(2);
        Destroy(dummy);
        lilith.SetActive(true);
        alreadyDummied = false;

        lilith.transform.position = oldPosition;
        lilith.transform.rotation = oldRotation;

        LilithScript.stunned = false;
    }

    public void No()
    {
        Scroll.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
        AudioListener.pause = false;
    }

    public void QuitToDesktop()
    {
        GlobalSettings globalsettings = SaveSystem.Load();
        if (!globalsettings.information.multiplierUnlocks.Equals(StyleManager.globalSettings.information.multiplierUnlocks)) SaveSystem.Save(StyleManager.globalSettings);
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
        kill = 0;
        dro = 0;
        dro = 0;
        SceneManager.LoadScene(1);
    }
}
