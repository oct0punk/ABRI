using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FSM_IdleState : FSM_BaseState
{
    public float h = 1.0f;
    public float r = 0.5f;
    float time = 1.0f;
    

    public override void OnEnter(Lumberjack l)
    {
        if (l.isAutoMoving) l.ChangeFSM(l.autoMoveState);
        GameManager.instance.ui.NoMove();
        l.animator.SetBool("isWalking", false);

        time = 1.0f;
    }

    public override void Update(Lumberjack l)
    {        
        BaseUpdate(l);

        if (SwipeManager.MoveLeft() || SwipeManager.MoveRight()) {
            l.ChangeFSM(l.movingState);
            return;
        }
    }

    protected void BaseUpdate(Lumberjack l)
    {
        l.Stabilize();

        
        if (l.canCut)
        {
            if (SwipeManager.Cut())
            {
                l.StartCutting();
            }
        }


    }

    public override void OnExit(Lumberjack l)
    {

    }
}
