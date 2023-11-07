using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FSM_JumpingState : FSM_BaseState
{
    float t = 0.0f; 
    Vector3 start; 
    public Vector3 land;

    public override void OnEnter(Lumberjack l)
    {
        t = .0f;
        l.SetSpriteColor(Color.yellow);
        start = l.transform.position;
    }

    public override void OnExit(Lumberjack l)
    {

    }

    public override void Update(Lumberjack l)
    {
        t += Time.deltaTime;
        l.transform.position = Vector3.Lerp(start, land, t);
        l.transform.position += Vector3.up * Mathf.Sin(t * Mathf.PI);
        if (t > 1.0f) {
            l.Move(land);
            l.ChangeFSM(l.movingState);
        }
    }
}
