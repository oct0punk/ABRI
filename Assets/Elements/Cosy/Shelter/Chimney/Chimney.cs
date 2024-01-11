using Cinemachine;
using System;
using UnityEngine;

[SelectionBase]
public class Chimney : MonoBehaviour
{
    public ConsumeBubble bubble;
    public CinemachineVirtualCamera cam;
    Combustible[] coms;

    private void Start()
    {
        coms = GetComponentsInChildren<Combustible>();
        bubble.gameObject.SetActive(false);
        bubble.action = () => { Reload(); };
    }

    public int GetPower()
    {
        return Array.FindAll(coms, c => c.active).Length;
    }

    public void Reload()
    {
        float maxCom = coms[0].consuming;
        int idx = 0;
        for (int i = 1; i < coms.Length; i++) 
        {
            if (coms[i].consuming > maxCom)
            {
                maxCom = coms[i].consuming;
                idx = i;
            }
        }
        coms[idx].Activate();
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>())
        {
            bubble.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            if (lum.storage.Count(RawMatManager.instance.GetRawMatByName("Log")) > 0)
                bubble.gameObject.SetActive(true);
        }
    }
}