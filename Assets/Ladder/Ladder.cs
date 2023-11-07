using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    Lumberjack l;
    public float getHeight()
    {
        return GetComponentInChildren<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        if (l == null) return;
        if (l.fsm != l.movingState) return;

        if (SwipeManager.ClimbUp())
        {
            l.ClimbUp(this);
        }
        else if (SwipeManager.ClimbDown())
        {
            l.ClimbDown(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack p = collision.GetComponentInParent<Lumberjack>();
        if (p != null)
        {
            l = p;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lumberjack p = collision.GetComponentInParent<Lumberjack>();
        if (p != null)
        {
            l = null;
        }
    }
}
