using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sonidos;

    Dictionary<string, Sound> dictionarySounds = new Dictionary<string, Sound>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (var s in sonidos)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            if (s.playOnAwake)
                s.source.Play();

            dictionarySounds.Add(s.nombre, s);
        }
    }

    public void Play(string nombre)
    {
        if (dictionarySounds.ContainsKey(nombre))
            dictionarySounds[nombre].source.Play();
    }

    public void ToggleMusic(string nombre)
    {
        if (dictionarySounds.ContainsKey(nombre))
            dictionarySounds[nombre].source.mute = !dictionarySounds[nombre].source.mute;
    }

    public void Pause(string nombre)
    {
        if (dictionarySounds.ContainsKey(nombre))
            dictionarySounds[nombre].source.Pause();
    }

    public void Resume(string nombre)
    {
        if (dictionarySounds.ContainsKey(nombre))
            dictionarySounds[nombre].source.UnPause();
    }
}

