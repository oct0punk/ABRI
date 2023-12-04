using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    Lumberjack lumberjack { get { return GameManager.instance.lumberjack; } }
    Shelter shelter { get { return GameManager.instance.shelter; } }
    public GameObject openBubble;
    public GameObject plans;
    public CraftBubble[] craftPlans;

    private void Start()
    {
        openBubble.gameObject.SetActive(false);
        foreach (CraftBubble plan in craftPlans)
        {
            plan.gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        GameManager.instance.ChangeState(GameState.Craft);
    }
    public void DisplayPlans()
    {
        openBubble.gameObject.SetActive(false);
        plans.SetActive(true);
        ModeCraft();
    }

    public void Close()
    {
        GameManager.instance.ChangeState(GameState.Indoor);
    }
    public void HidePlans()
    {
        Array.ForEach(craftPlans, plan => plan.SetVisibility(false));
        plans.SetActive(false);
        openBubble.gameObject.SetActive(true);
    }

    public void Craft(CraftBubble bubble)
    {
        RawMaterial craftable = bubble.material;
        foreach (var material in craftable.craftMaterials)
        {
            if (lumberjack.storage.Count(material.rawMaterial) < material.q)
            {
                bubble.animator.SetTrigger("NOPE");
                return;
            }
        }
        if (lumberjack.storage.CanFill(craftable))
        {
            bubble.animator.SetTrigger("OK");
            lumberjack.storage.Craft(craftable);
        }
        else
        {
            if (craftable == RawMatManager.instance.GetRawMatByName("WoodPlanch"))
            {
                lumberjack.storage.Consume(craftable.craftMaterials);
                shelter.storage.Add(craftable);
                bubble.animator.SetTrigger("OK");
            }
            else
                bubble.animator.SetTrigger("NOPE");
        }
    }


    public void ModeCraft()
    {
        shelter.DisplayPieceBubble(false);
        Array.ForEach(craftPlans, plan => plan.SetVisibility(true));
    }

    public void ModePieces()
    {
        Array.ForEach(craftPlans, plan => plan.gameObject.SetActive(false));
        shelter.DisplayPieceBubble(true);
    }

    public void ModeNests()
    {
        Array.ForEach(craftPlans, plan => plan.gameObject.SetActive(false));
        shelter.DisplayNestsBubble(true);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            openBubble.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            openBubble.gameObject.SetActive(false);
            foreach (var bubble in craftPlans)
                bubble.gameObject.SetActive(false);
        }
    }
}
