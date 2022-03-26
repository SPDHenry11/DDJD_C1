using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    private sound[] sounds;
    struct sound
    {
        public string name;
        public AudioSource audio;
    }

    public static AudioController instance;
    [SerializeField] private AudioMixer mixer;

    private void Awake()
    {
        instance = this;
        AudioSource[] audios = GetComponentsInChildren<AudioSource>();
        sounds = new sound[audios.Length];
        for (int i = 0; i < audios.Length; i++)
        {
            sounds[i].audio = audios[i];
            sounds[i].name = audios[i].gameObject.name;
        }
        mixer.SetFloat("Volume", 0);
    }
    public void Play(string name)
    {
        if (!Array.Exists(sounds, sound => sound.name == name))
        {
            Debug.LogError("Missing Audio Source \"" + name + "\" in AudioController");
            return;
        }
        Array.Find(sounds, sound => sound.name == name).audio.Play();
    }
    public void Stop(string name)
    {
        if (!Array.Exists(sounds, sound => sound.name == name))
        {
            Debug.LogError("Missing Audio Source \"" + name + "\" in AudioController");
            return;
        }
        Array.Find(sounds, sound => sound.name == name).audio.Stop();
    }
    private bool audioExists(string name)
    {
        return true;
    }

    public IEnumerator FadeOut()
    {
        float volume = 1;
        while (volume >= 0)
        {
            volume -= 0.5f * Time.deltaTime;
            mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
            yield return null;
        }
        mixer.SetFloat("Volume", -80);
    }

    public IEnumerator FadeIn()
    {
        float volume = 0;
        while (volume < 1)
        {
            volume += 0.5f * Time.deltaTime;
            mixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
            yield return null;
        }
        mixer.SetFloat("Volume", 0);
    }

}
