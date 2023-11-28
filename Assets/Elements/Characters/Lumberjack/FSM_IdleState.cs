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
        l.SetSpriteColor(Color.white);
        l.animator.SetBool("isWalking", false);
        l.ThinkOf(false);

        time = 1.0f;
    }

    public override void Update(Lumberjack l)
    {        
        // Stabilize
        l.Stabilize();
        

        if (time > 0.0f)
        {
            time -= Time.deltaTime;
            if (time <= 0.0f)
            {
                l.ThinkOf(true);
            }
        }

        if (SwipeManager.MoveLeft() || SwipeManager.MoveRight())
            l.ChangeFSM(l.movingState);
    }

    public override void OnExit(Lumberjack l)
    {
    }
}
