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
    public static AudioManager Instance { get { return GameManager.instance.GetComponent<AudioManager>(); } }
    public Fade[] fades;
    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].source != null)
            {
                Debug.LogWarning("Sound " + sounds[i].name + " destroy on loading a new scene");
                continue;
            }
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

    public AudioSource Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound.source != null)
            sound.source.Play();
        else
            Debug.LogWarning("Sound " + name + " not found.");
        return sound.source;
    }

    public AudioFade GetFadeByName(string name)
    {
        AudioFade fade = Array.Find(fades, f => f.name == name).audio;
        if (fade != null)
            return fade;
        else
            Debug.LogWarning("Fade " + name + " not found.");
        return null;
    }
}
