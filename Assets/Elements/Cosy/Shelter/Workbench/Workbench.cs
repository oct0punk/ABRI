using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    Lumberjack lumberjack;
    Shelter shelter;
    public GameObject bubble;
    public GameObject[] craftPlans;

    private void Start()
    {
        shelter = GetComponentInParent<Shelter>();
        lumberjack = FindObjectOfType<Lumberjack>();
        bubble.SetActive(false);
        foreach (GameObject plan in craftPlans)
        {
            plan.SetActive(false);
        }
    }

    public void DisplayPlans()
    {
        bubble.SetActive(false);
        Array.ForEach(craftPlans, plan => plan.SetActive(true));
        Array.ForEach(shelter.pieces, p => p.SetBubbleVisible(true));
        Array.ForEach(shelter.perchs, p => p.bubble.gameObject.SetActive(true));
    }

    public void HidePlans()
    {
        bubble.SetActive(true);
        Array.ForEach(craftPlans, plan => plan.SetActive(false));
        Array.ForEach(shelter.pieces, p => p.SetBubbleVisible(false));
        Array.ForEach(shelter.perchs, p => p.bubble.gameObject.SetActive(false));
    }

    public void Craft(CraftBubble bubble)
    {
        RawMaterial craftable = bubble.material;
        if (!lumberjack.storage.CanFill(craftable))
        {
            bubble.animator.SetTrigger("NOPE");
            return;
        }
        foreach (var material in craftable.craftMaterials)
        {
            if (lumberjack.storage.Count(material.rawMaterial) < material.q)
            {
                bubble.animator.SetTrigger("NOPE");
                return;
            }
        }
        bubble.animator.SetTrigger("OK");
        lumberjack.storage.Craft(craftable);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            bubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            bubble.SetActive(false);
            foreach (var bubble in craftPlans)
                bubble.gameObject.SetActive(false);
        }
    }
}
