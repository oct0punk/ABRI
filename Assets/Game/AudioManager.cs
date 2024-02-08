using System;
using UnityEngine;

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    [HideInInspector] public AudioSource source;
    [Range(0f,1f)] public float volume;
    public bool loop;
    public bool playOnAwake;
}

[Serializable]
public struct Fade
{
    public string name;
    public AudioFade audio;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Fade[] fades;
    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].source != null) continue;
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sounds[i].clip; 
            source.volume = sounds[i].volume;
            source.loop = sounds[i].loop;
            if (sounds[i].playOnAwake) {
                source.playOnAwake = true;
                source.Play();
            }
            sounds[i].source = source;
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound.source != null)
            sound.source.Play();
        else
            Debug.LogWarning("Sound " + name + " not found.");
    }

    public AudioFade GetFadeByName(string name)
    {
        return Array.Find(fades, f => f.name == name).audio;
    }
}
