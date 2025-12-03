using Steamworks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Functions : MonoBehaviour
{
    public GameObject FadeOut;
    public Animator settingsanim;
    public bool hasrolledin;
    public AudioMixer audioMixer;
    public UnityEngine.UI.Slider sfxSlider, musicSlider;
    public AudioClip rollout, rollin;
    public int alreadybeatthegame = 0;
    public Button startGame, discord, quit, settings, tenthbutton;
    public GameObject tenth;
    public GameObject eventsystem, glow;

    void Start()
    {
        SaveSystem.Load();
        AchivementScript.instance.UnlockAchivement("HELLBOUND_ENTRY");
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loadAudioSettings();
        loadsaved();
    }

    public void loadsaved()
    {
        GlobalSettings globalsettings = SaveSystem.Load();

        alreadybeatthegame = globalsettings.information.hasbeatthegame;

        RectTransform rectTransform = startGame.GetComponent<RectTransform>();
        if (alreadybeatthegame != 0)
        {
            rectTransform.anchoredPosition = new Vector2(0, -5f);
            tenth.SetActive(true);
        }
        else
        {
            rectTransform.anchoredPosition = new Vector2(0, -72f);
        }
    }

    public void Update()
    {
        if (hasrolledin)
        {
            if(Input.GetKeyDown(KeyCode.Escape)) SettingsRollout();
        }
    }

    public void Play()
    {
        EventSystem.current.SetSelectedGameObject(null);
        int scene = 3;

        /*
        string path = System.IO.Path.Combine(Application.persistentDataPath, "Information.json");

        if (!File.Exists(path))
        {
            Information info = new Information()
            {
                hasbeatthegame = 0,
            };
            File.WriteAllText(path, JsonUtility.ToJson(info));
        }

        Information information = JsonUtility.FromJson<Information>(System.IO.File.ReadAllText(path));
        */

        //alreadybeatthegame = information.hasbeatthegame;

        GlobalSettings globalsettings = SaveSystem.Load();

        if (globalsettings.information.doneTutorial > 0) scene = 4;
        StartCoroutine(StartGame(scene));
    }

    public void PlayTenth()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(StartGame(6));
    }

    public void Quit()
    {
       Application.Quit();
    }

    private IEnumerator StartGame(int scene)
    {
        //EventSystem.current.SetSelectedGameObject(null);
        FadeOut.SetActive(true);
        UnInteract();
        yield return new WaitForSeconds(0.95f);
        loadScene.SceneToLoad = scene;
        SceneManager.LoadScene(2);
    }

    void UnInteract()
    {
        Destroy(eventsystem);
        Destroy(glow);
        startGame.interactable = false;
        quit.interactable = false;
        settings.interactable = false;
        tenthbutton.interactable = false;
        discord.interactable = false;
    }

    public void SettingsRollin()
    {
        if (hasrolledin) return;
        StartCoroutine(settingsrollinwaiter());
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SettingsRollout()
    {
        if (!hasrolledin) return;
        StartCoroutine(agarshemidzliadamacadeyleo());
        settingsanim.Play("SettingsPanelRollout");
        audioManager.instance.playAudio(rollin, 1, 1, transform, audioManager.instance.sfx);
    }

    public IEnumerator agarshemidzliadamacadeyleo()
    {
        yield return new WaitForSeconds(1.2f);
        hasrolledin = false;
    }

    public IEnumerator settingsrollinwaiter()
    {
        settingsanim.Play("SettingsPanelRollIn");
        audioManager.instance.playAudio(rollout, 1, 1, transform, audioManager.instance.sfx);
        yield return new WaitForSeconds(1.5f);
        hasrolledin = true;
    }

    public void discordloader()
    {
        Application.OpenURL("https://discord.gg/Kn7P2Rkx3b");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void loadAudioSettings()
    {
        GlobalSettings globalsettings = SaveSystem.Load();

        musicSlider.value = globalsettings.audio.MUSICVolume;
        sfxSlider.value = globalsettings.audio.SFXVolume;

        setMusicVolume();
        setSFXVolume();
    }

    public void saveAudioSettings(float volume, int value)
    {
        GlobalSettings globalsettings = SaveSystem.Load();
        if (value > 0) globalsettings.audio.MUSICVolume = volume;
        else globalsettings.audio.SFXVolume = volume;

        SaveSystem.Save(globalsettings);
    }

    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        saveAudioSettings(volume, 1);
    }

    public void setSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        saveAudioSettings(volume, 0);
    }
}
