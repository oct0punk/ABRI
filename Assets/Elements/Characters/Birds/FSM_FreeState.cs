using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_FreeState : Bird_BaseState
{
    public override void OnEnter(Bird bird)
    {
        bird.GetComponent<SpriteRenderer>().color = Color.white;
        bird.transform.position = bird.perch.transform.position;
    }

    public override void OnExit(Bird bird)
    {

    }

    public override void OnTriggerEnter2D(Bird bird, Collider2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerExit2D(Bird bird, Collider2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerStay2D(Bird bird, Collider2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(Bird bird)
    {

    }
}
