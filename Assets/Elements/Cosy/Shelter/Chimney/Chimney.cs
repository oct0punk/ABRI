using System;
using UnityEngine;


public class Chimney : MonoBehaviour
{
    public bool reload;
    Combustible[] coms;

    private void Start()
    {
        coms = GetComponentsInChildren<Combustible>();
    }

    private void Update()
    {
        if (reload)
        {
            reload = false;
            Reload();
        }
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
}