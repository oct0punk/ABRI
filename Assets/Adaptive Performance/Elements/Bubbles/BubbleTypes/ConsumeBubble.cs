using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class ConsumeBubble : TapBubble
{
    public Action action;
    public CraftRes[] materials;

    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Consume()
    {
        Lumberjack lum = GameManager.instance.lumberjack;
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
        StartCoroutine(EnumerateMissedMaterials(GameManager.instance.lumberjack, missed));
    }

    protected IEnumerator EnumerateMissedMaterials(Lumberjack lum, List<RawMaterial> missed)
    {
        GameUI ui = GameManager.instance.ui;
        foreach(RawMaterial miss in missed)
        {
            yield return lum.Message(ui.GetMissBubbleByMat(miss), 0.7f);
            yield return new WaitForSeconds(.1f);
        }
    }
}
