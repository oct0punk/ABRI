using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFade : MonoBehaviour
{
    [SerializeField] AudioSource source1;
    [SerializeField] AudioSource source2;
    [SerializeField] [Range(0.0f, 1.0f)] float fadeAlpha;
    [HideInInspector] public float target;
    [SerializeField] AnimationCurve fadeCurve;

    private void Awake()
    {
        target = fadeAlpha;
    }

    private void Update()
    {
        fadeAlpha = Mathf.Clamp(target, fadeAlpha - Time.deltaTime, fadeAlpha + Time.deltaTime);
        source1.volume = fadeCurve.Evaluate(fadeAlpha);
        source2.volume = fadeCurve.Evaluate(1 - fadeAlpha);
    }
}
