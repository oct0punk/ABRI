using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [SerializeField] Sound sound1;
    [SerializeField] Sound sound2;
    [SerializeField] [Range(0.0f, 1.0f)] float fadeAlpha;
    /*[HideInInspector]*/ public float target;
    [SerializeField] AnimationCurve fadeCurve;

    private void Awake()
    {
        target = fadeAlpha;
        sound1.source = gameObject.AddComponent<AudioSource>();
        sound2.source = gameObject.AddComponent<AudioSource>();
        sound1.source.loop = true;
        sound2.source.loop = true;
        sound1.source.clip = sound1.clip;
        sound2.source.clip = sound2.clip;
        Play();
    }

    private void Update()
    {
        fadeAlpha = Mathf.Clamp(target, fadeAlpha - Time.deltaTime, fadeAlpha + Time.deltaTime);
        sound1.source.volume = sound1.volume * fadeCurve.Evaluate(fadeAlpha);
        sound2.source.volume = sound2.volume * fadeCurve.Evaluate(1 - fadeAlpha);
    }

    public void Play()
    {
        sound1.source.Play();
        sound2.source.Play();
    }

    public void Stop()
    {
        sound1.source.Stop();
        sound2.source.Stop();
    }
}
