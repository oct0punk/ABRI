using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageUI : ThinkBubble
{
    public void Display(int amount,  float time)
    {
        Message(amount.ToString(), time);
    }

    public void Display(int amount)
    {
        Tmp.text = amount.ToString();
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Required(int amount)
    {

    }
}
