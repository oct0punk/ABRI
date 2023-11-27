using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    public GameObject bubble;
    public GameObject[] craftPlans;

    private void Start()
    {
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

    public void TransfrormWoodToPlanch(Animator anim)
    {
        anim.SetTrigger("NOPE");
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
