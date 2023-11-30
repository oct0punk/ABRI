using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    Lumberjack lumberjack;
    public GameObject bubble;
    public GameObject[] craftPlans;

    private void Start()
    {
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
        foreach (var plan in craftPlans)
            plan.SetActive(true);
    }

    public void HidePlans()
    {
        bubble.SetActive(true);
        foreach (var plan in craftPlans)
            plan.SetActive(false);
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
        }
    }
}
