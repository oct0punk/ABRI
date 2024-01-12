using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct Sound
{
    public string name;
    public AudioClip clip;
    public AudioSource source;
    public bool loop;
    public bool playOnAwake;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioFade windFade;
    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].source != null) continue;
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sounds[i].clip; 
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
}
