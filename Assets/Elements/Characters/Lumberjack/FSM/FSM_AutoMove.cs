

using TMPro;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class FSM_AutoMove : FSM_MovingState
{
    public bool isAutoMoving = false;
    public Vector3 targetPos;
    public bool flip;

    public override void OnEnter(Lumberjack l)
    {
        isAutoMoving = true;
        GameManager.instance.ui.NoMove();
    }

    public override void OnExit(Lumberjack l)
    {
    }

    public override void Update(Lumberjack l)
    {
        if (Mathf.Abs((targetPos - l.transform.position).x) < .1f)
        {
            isAutoMoving = false;
            l.spriteRenderer.flipX = flip;
            l.ChangeFSM(l.idleState);
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
