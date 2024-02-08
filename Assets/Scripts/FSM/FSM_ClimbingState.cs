using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_ClimbingState : FSM_BaseState
{
    public Vector3 targetPos;
    public override void OnEnter(Lumberjack l)
    {
        l.animator.SetBool("isClimbing", true);
        Tuto.tutoClimb = false;
    }

    public override void OnExit(Lumberjack l)
    {
        l.animator.SetBool("isClimbing", false);
    }

    public override void Update(Lumberjack l)
    {
        l.Move(targetPos);
        if (Vector3.Distance(targetPos, l.transform.position) < .1f)
        {
            l.ChangeFSM(l.idleState);
        }
    }
}
