using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [Range(1, 5)]
    public int wind;
    
    // S[SerializeField]
    float windMultiplier;
    
    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
}
