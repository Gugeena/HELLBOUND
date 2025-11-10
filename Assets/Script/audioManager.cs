using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class audioManager : MonoBehaviour
{
    public static audioManager instance;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioSource mainMusic, lillithMusic, endMusic, tutorialMusic;
    public AudioMixerGroup sfx, music;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void playAudio(AudioClip a, float volume, float pitch, Transform pos, AudioMixerGroup channel)
    {
        AudioSource audioSource = Instantiate(source, pos.position, Quaternion.identity);

        audioSource.clip = a;
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.outputAudioMixerGroup = channel;

        audioSource.Play();
        float len = audioSource.clip.length;

        Destroy(audioSource.gameObject, len);
    }

    public void playRandomAudio(AudioClip[] a, float volume, float pitch, Transform pos, AudioMixerGroup channel)
    {
        AudioSource audioSource = Instantiate(source, pos.position, Quaternion.identity);

        audioSource.clip = a[Random.Range(0, a.Length)];
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.outputAudioMixerGroup = channel;

        audioSource.Play();
        float len = audioSource.clip.length;

        Destroy(audioSource.gameObject, len);
    }

    public void stopMusic() 
    { 
        mainMusic.Stop();
        lillithMusic.Stop();
        endMusic.Stop();
    }

    public void startLillith()
    {
        lillithMusic.Play();
    }

    public void stopLillith()
    {
        lillithMusic.Stop();
    }

    public void startMusic()
    {
        mainMusic.Play();
    }

    public void startEnd()
    {
        endMusic.Play();
    }

    public void stopEnd()
    { 
        endMusic.Stop();
    }

    public void startTutorial()
    {
        tutorialMusic.Play();
    }

    public void stopTutorial()
    {
        tutorialMusic.Stop();
    }
}
