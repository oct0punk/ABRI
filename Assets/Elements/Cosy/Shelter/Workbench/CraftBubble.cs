using System;
using System.Collections.Generic;
using UnityEngine;

public class CraftBubble : ConsumeBubble
{
    public RawMaterial material;
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
        Debug.Log(layout);
        layout.Close();
    }

    public void Roll()
    {
        layout.AutoRoll();
        if (layout.isOpen)
            Array.ForEach(presentation, p => p.UpdateMat());
    }
    void Open()
    {
        layout.gameObject.SetActive(true);
        Array.ForEach(presentation, p => p.UpdateMat());

    }

    void UpdatePres()
    {
        Array.ForEach(presentation, pres => pres.UpdateMat());
    }
    void LaunchCraftFeedback()
    {
        GameUI.instance.InstantiateCraftFeedback(this);
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
    }

}
