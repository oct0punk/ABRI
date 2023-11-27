using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{
    Lumberjack l;
    SpriteRenderer sprite;
    BoxCollider2D box;

    public void SetHeight(float height)
    {
        sprite.transform.localScale = new Vector3(1, height, 1);
        sprite.transform.localPosition = new Vector3(0, height / 2, 0);
        box.size = new Vector2(1, height);
        box.offset = new Vector2(0, height / 2);
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    public float getHeight()
    {
        return sprite.bounds.size.y;
    }

    private void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.identity;

        if (l == null) return;
        if (l.fsm != l.movingState) return;

        if (SwipeManager.ClimbUp()) {
            if (isAtBottom(l))
                l.ClimbUp(this);            
        }
        else if (SwipeManager.ClimbDown()) {
            if (!isAtBottom(l))
                l.ClimbDown(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack p = collision.GetComponentInParent<Lumberjack>();
        if (p != null)
        {
            l = p;
            enabled = true;
            Tuto.canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lumberjack p = collision.GetComponentInParent<Lumberjack>();
        if (p != null)
        {
            l = null;
            enabled = false;
            Tuto.canClimb = false;
        }
    }

    bool isAtBottom(Lumberjack lum)
    {
        return  Vector3.Distance(lum.transform.position, transform.position) < 
                Vector3.Distance(lum.transform.position, transform.position + Vector3.up * getHeight());
    }
}
