using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SwipeManager : MonoBehaviour
{
    public bool test;

    [SerializeField] private Swipe[] swipes;

    void Update()
    {
        foreach (var swipe in swipes) swipe.Update();

        test = swipes[0].SwipeDown(0.0f) && swipes[1].SwipeDown(1.0f)
            || swipes[0].SwipeDown(1.0f) && swipes[1].SwipeDown(0.0f);

        if (test)
            Debug.Log("YEAH");

    }
}
