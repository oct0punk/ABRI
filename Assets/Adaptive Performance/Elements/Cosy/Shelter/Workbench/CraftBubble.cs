using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftBubble : ConsumeBubble
{
    public CraftableMaterial material;
    public TapBubble craftButton;
    ScrollLayout layout;
    PresBubble[] presentation;
    public PresBubble presPrefab;

    private void Awake()
    {
        layout = GetComponentInChildren<ScrollLayout>();
        materials = material.craftMaterials;
        action = () => GameManager.instance.lumberjack.storage.Add(material);
        InitPres();
    }

    void InitPres()
    {
        presentation = new PresBubble[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            presentation[i] = Instantiate(presPrefab, layout.transform);
            presentation[i].Init(materials[i]);
        }
    }

    public void SetVisibility(bool visible)
    {
        gameObject.SetActive(visible);
    }

    private void OnDisable()
    {
        layout.Close();
    }

    public void Roll()
    {
        layout.AutoRoll();
        if (layout.isOpen)
        {
            UpdatePres();
            
            foreach (CraftBubble bubble in transform.parent.GetComponentsInChildren<CraftBubble>())
            {
                if (bubble != this)
                {
                    bubble.layout.Close();
                }
            }
        }
    }

    public void UpdatePres()
    {
        Array.ForEach(presentation, pres => pres.UpdateMat());
    }

    protected override void OnConsumeFeedback()
    {
        UpdatePres();
    }
    protected override void OnFailed(List<RawMaterial> missed)
    {
        foreach (RawMaterial material in missed)
        {
            PresBubble pres = Array.Find(presentation, p => p.cMat.rawMaterial == material);
            pres.GetComponent<Animator>().SetTrigger("OK");
        }
        StartCoroutine(EnumerateMissedMaterials(GameManager.instance.lumberjack, missed));
    }

}
