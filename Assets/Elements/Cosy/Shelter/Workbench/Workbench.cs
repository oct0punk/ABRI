using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftState
{
    RawMat,
    Piece,
    Nest
}

public class Workbench : MonoBehaviour
{
    Lumberjack lumberjack { get { return GameManager.instance.lumberjack; } }
    Shelter shelter { get { return GameManager.instance.shelter; } }
    public TapBubble openBubble;
    public TapBubble swapBubble;
    public TapBubble closeBubble;
    public GameObject plans;
    public CraftBubble[] craftPlans;
    public CraftState state;

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
        state = CraftState.RawMat;
        shelter.DisplayPieceBubble(false);
        Array.ForEach(craftPlans, plan => plan.SetVisibility(true));
    }

    public void ModePieces()
    {
        state = CraftState.Piece;
        Array.ForEach(craftPlans, plan => plan.gameObject.SetActive(false));
        shelter.DisplayPieceBubble(true);
    }

    public void ModeNests()
    {
        state = CraftState.Nest;
        Array.ForEach(craftPlans, plan => plan.gameObject.SetActive(false));
        shelter.DisplayNestsBubble(true);
    }

    public void SwapMode()
    {
        switch (state)
        { 
            case CraftState.RawMat:
                ModePieces(); break;
            case CraftState.Piece:
                ModeNests(); break;
            case CraftState.Nest:
                ModeCraft(); break;
        }
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
