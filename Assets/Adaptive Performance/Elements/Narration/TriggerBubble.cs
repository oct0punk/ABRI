using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBubble : MonoBehaviour
{
    public GameObject content;
    public float time;
    public bool doOnce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            lum.Message(content, time);
            if (doOnce)
                Destroy(gameObject);
        }
    }
}
