using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public ConsumeBubble bubble;
    public SpriteRenderer shelter;
    public Sprite sprite;
    public RawMaterial material;
    bool isBuild = false;

    private void Start()
    {
        bubble.action = Build;
        bubble.gameObject.SetActive(false);
    }

    public void Build()
    {
        shelter.sprite = sprite;
        FindObjectOfType<Lumberjack>().storage.Add(material);
        bubble.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        isBuild = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBuild) return;
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            bubble.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isBuild) return;
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            bubble.gameObject.SetActive(false);
        }
    }
}
