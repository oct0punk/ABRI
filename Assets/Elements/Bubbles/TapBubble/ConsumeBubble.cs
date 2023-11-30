using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeBubble : MonoBehaviour
{
    public Action action;
    public CraftMaterials[] materials;


    public void Consume()
    {
        Lumberjack lum = FindAnyObjectByType<Lumberjack>();
        if (!lum.storage.CanCraft(materials))
        {
            GetComponent<Animator>().SetTrigger("NOPE");
            return;
        }
        foreach (var material in materials)
        {
            lum.storage.Add(material.rawMaterial, -material.q);
        }
        GetComponent<Animator>().SetTrigger("OK");
        action();
    }
}
