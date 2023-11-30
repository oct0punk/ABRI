using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapBubble : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
