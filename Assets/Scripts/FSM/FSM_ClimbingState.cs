using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class FSM_ClimbingState : FSM_BaseState
{
    public Ladder ladder;
    public bool climbDown;
    Vector2 targetPos;

    public override void OnEnter(Lumberjack l)
    {
        AudioManager.Instance.Play("Climb");
        l.transform.SetParent(ladder.transform);
        l.animator.SetBool("isClimbing", true);
    }

    public override void OnExit(Lumberjack l)
    {
        AudioManager.Instance.Stop("Climb");
        l.spriteRenderer.flipX = false;
        l.animator.SetBool("isClimbing", false);
        ladder.ActivateArrow(l);
    }

    public override void Update(Lumberjack l)
    {
        targetPos = climbDown ? ladder.transform.position : ladder.Top();
        l.Move(targetPos);
        if (Vector3.Distance(targetPos, l.transform.position) < .1f)
        {
            l.ChangeFSM(l.idleState);
        }
    }
}
