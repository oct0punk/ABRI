using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_StandyState : Bird_BaseState
{
    public override void OnEnter(Bird bird)
    {
        bird.GetComponent<SpriteRenderer>().color = Color.white;
        bird.GetComponent<Collider2D>().enabled = true;
        bird.bubble.gameObject.SetActive(false);
    }

    public override void OnExit(Bird bird)
    {
        bird.bubble.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter2D(Bird bird, Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null && lum.carryingBird == null)
            bird.bubble.SetActive(true);
    }

    public override void OnTriggerExit2D(Bird bird, Collider2D collision)
    {
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null && lum.carryingBird == null)
            bird.bubble.gameObject.SetActive(false);
    }

    public override void OnTriggerStay2D(Bird bird, Collider2D collision)
    {

    }

    public override void Update(Bird bird)
    {

    }
}
