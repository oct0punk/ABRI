using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ScrollLayout : MonoBehaviour
{
    RectTransform rect;
    public bool isOpen { get; private set; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
    }

    public void AutoRoll()
    {
        if (isOpen) Close();
        else Open();
    }
    public void Open()
    {
        float widthOpen = (rect.childCount) * 100.0f + 50.0f;
        rect.sizeDelta = new Vector2(widthOpen, rect.sizeDelta.y);
        isOpen = true;

    }
    public void Close()
    {
        rect.sizeDelta = Vector2.zero;
        isOpen = false;
    }
}
