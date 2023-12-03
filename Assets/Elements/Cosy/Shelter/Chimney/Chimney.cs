using JetBrains.Annotations;
using UnityEngine;

[SelectionBase]
public class Chimney : MonoBehaviour
{
    public ConsumeBubble bubble;
    Combustible[] coms;
    

    private void Start()
    {
        coms = GetComponentsInChildren<Combustible>();
        bubble.action = Reload;
        bubble.gameObject.SetActive(false);
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
        if (collision.GetComponentInParent<Lumberjack>())
        {
            bubble.gameObject.SetActive(true);
        }
    }
}