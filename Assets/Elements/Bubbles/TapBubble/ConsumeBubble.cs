using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ConsumeBubble : TapBubble
{
    public Action action;
    public CraftMaterials[] materials;

    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Consume()
    {
        Lumberjack lum = FindAnyObjectByType<Lumberjack>();
        if (!lum.storage.CanCraft(materials))
        {
            OnFailed(lum.storage.GetMissedMats(materials));
            return;
        }
        foreach (var material in materials)
        {
            lum.storage.Add(material.rawMaterial, -material.q);
        }
        OnConsumeFeedback();
        action();
    }
    protected virtual void OnConsumeFeedback()
    {
        animator.SetTrigger("OK");
    }

    protected virtual void OnFailed(List<RawMaterial> missed)
    {
        animator.SetTrigger("NOPE");
    }
}
