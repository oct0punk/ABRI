using System;
using UnityEngine;

public class FSM_AutoMove : FSM_MovingState
{
    public Vector3 targetPos;
    public Action action;
    bool disableOnExit = false;

    public override void OnEnter(Lumberjack l)
    {
        disableOnExit = !l.enabled;
        l.enabled = true;
        if (!l.isAutoMoving)
        {
            l.isAutoMoving = true;
            AudioManager.Instance.Play("AutoMove");
        }
        GameUI.instance.NoMove();
    }

    public override void OnExit(Lumberjack l)
    {
    }

    public override void Update(Lumberjack l)
    {
        if (Mathf.Abs((targetPos - l.transform.position).x) < .1f)
        {
            l.isAutoMoving = false;
            if (disableOnExit)  l.enabled = false;
            l.ChangeFSM(l.idleState);
            if (action != null) action();
            return;
        }

        float sign = Mathf.Sign((targetPos - l.transform.position).x);
        if (TryMove(l, sign * r, 2.0f, 0.0f))
        {
            l.Move(targetPosition);
            return;
        }
        else if (TryMove(l,  sign * (r + 2), 3.0f, -h / 2))   // TryJump
        {
            l.Jump(targetPosition);
            return;
        }
    }
}
