using JetBrains.Annotations;
using UnityEngine;

[SelectionBase]
public class Chimney : MonoBehaviour
{
    public bool canReload;
    public Canvas canvas;
    Combustible[] coms;
    

    private void Start()
    {
        coms = GetComponentsInChildren<Combustible>();
        canvas.gameObject.SetActive(false);
    }

    public void Reload()
    {
        if (!canReload)
        {
            canvas.GetComponent<Animator>().SetTrigger("NOPE");
            return;
        }

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
        canvas.GetComponent<Animator>().SetTrigger("OK");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>())
        {
            canvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        if (collision.GetComponentInParent<Lumberjack>())
        {
            canvas.gameObject.SetActive(true);
        }
    }
}