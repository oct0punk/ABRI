using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeBubble : TapBubble
{
    public Action action;
    public CraftMaterials[] materials;
    PresBubble[] presentation;
    public float offset = 100.0f;
    public PresBubble presPrefab;
    public bool isPresenting = false;

    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        isPresenting = materials.Length == 0;
    }

    protected void Pres()
    {
        if (presentation == null)
        {
            presentation = new PresBubble[materials.Length];
            for (int i = 0; i < presentation.Length; i++)
            {
                presentation[i] = Instantiate(presPrefab, transform);
                presentation[i].rectT.anchoredPosition = Vector3.up * (i + 1) * offset;
                presentation[i].Init(materials[i]);
            }
        }
        else
        {
            foreach (var pres in presentation)
                pres.gameObject.SetActive(true);
        }
        isPresenting = true;
    }

    protected static void hideAll()
    {
        foreach (var bubble in FindObjectsOfType<ConsumeBubble>())
            bubble.hidePres();
    }

    protected void hidePres()
    {
        if (presentation != null)
            foreach (var pres in presentation)
                pres.gameObject.SetActive(false);

        isPresenting = false;
    }

    public void Consume()
    {
        if (!isPresenting)
        {
            hideAll();
            Pres();
            return;
        }
        Lumberjack lum = FindAnyObjectByType<Lumberjack>();
        if (!lum.storage.CanCraft(materials))
        {
            animator.SetTrigger("NOPE");
            return;
        }
        foreach (var material in materials)
        {
            lum.storage.Add(material.rawMaterial, -material.q);
        }
        animator.SetTrigger("OK");
        action();
    }
}
