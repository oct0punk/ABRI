using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_CarriedState : Bird_BaseState
{
    public override void OnEnter(Bird bird)
    {
        bird.GetComponent<SpriteRenderer>().color = Color.blue;
        bird.GetComponent<Collider2D>().enabled = false;
    }

    public override void OnExit(Bird bird)
    {

    }

    public override void OnTriggerEnter2D(Bird bird, Collider2D collision)
    {

    }

    public override void OnTriggerExit2D(Bird bird, Collider2D collision)
    {

    }

    public override void OnTriggerStay2D(Bird bird, Collider2D collision)
    {

    }

    public override void Update(Bird bird)
    {

    }
}
