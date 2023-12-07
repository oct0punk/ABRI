using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Workbench : MonoBehaviour
{
    Lumberjack lumberjack { get { return GameManager.instance.lumberjack; } }
    Shelter shelter { get { return GameManager.instance.shelter; } }
    public TapBubble openBubble;
    public TapBubble closeBubble;
    public GameObject plans;

    private void Start()
    {
        openBubble.gameObject.SetActive(false);
        plans.SetActive(false);
    }

    public void Open()
    {
        GameManager.instance.ChangeState(GameState.Craft);
    }
    public void DisplayPlans()
    {
        openBubble.gameObject.SetActive(false);
        plans.SetActive(true);
    }

    public void Close()
    {
        GameManager.instance.ChangeState(GameState.Indoor);
    }
    public void HidePlans()
    {
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
            plans.SetActive(false);
        }
    }
}
