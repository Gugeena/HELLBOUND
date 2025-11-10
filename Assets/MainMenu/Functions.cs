using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Functions : MonoBehaviour
{
    public GameObject FadeOut;
    public Animator settingsanim;
    public bool hasrolledin;
    public AudioMixer audioMixer;
    public UnityEngine.UI.Slider sfxSlider, musicSlider;
    public AudioClip rollout, rollin;

    void Start()
    {
       Cursor.lockState = CursorLockMode.None;
       Cursor.visible = true;
       loadAudioSettings();
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
        StartCoroutine(StartGame());
    }

    public void Quit()
    {
       Application.Quit();
    }

    private IEnumerator StartGame()
    {
        FadeOut.SetActive(true);
        yield return new WaitForSeconds(0.95f);
        loadScene.SceneToLoad = 3;
        SceneManager.LoadScene(2);
    }

    public void SettingsRollin()
    {
        if (hasrolledin) return;
        StartCoroutine(settingsrollinwaiter());
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
        Application.OpenURL("https://discord.gg/BuAXWgqr");
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void loadAudioSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

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
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        setMusicVolume();
    }

    public void loadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
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
