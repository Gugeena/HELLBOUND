using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loadAudioSettings();
        loadsaved();
    }

    public void loadsaved()
    {
        alreadybeatthegame = PlayerPrefs.GetInt("alreadybeatthegame", 0);
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
        StartCoroutine(StartGame(3));
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
        EventSystem.current.SetSelectedGameObject(null);
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(0.95f);
        loadScene.SceneToLoad = scene;
        SceneManager.LoadScene(2);
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
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);

        setMusicVolume();
        setSFXVolume();
    }
    public void saveAudioSettings(float volume, int value)
    {
        if(value == 0) PlayerPrefs.SetFloat("sfxVolume", volume);
        else PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void loadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        setMusicVolume();
    }

    public void loadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);
        setSFXVolume();
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
