using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private sound[] sounds;
    struct sound
    {
        public string name;
        public AudioSource audio;
    }

    public static AudioController instance;

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
    }
    public void Play(string name)
    {
        if (!Array.Exists(sounds, sound => sound.audio.clip.name == name))
        {
            Debug.LogError("Missing Audio Source \"" + name + "\" in AudioController");
            return;
        }
        Array.Find(sounds, sound => sound.audio.clip.name == name).audio.Play();
    }
    private bool audioExists(string name)
    {
        return true;
    }
}
