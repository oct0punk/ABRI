using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public enum WorkState
{
    Cutting,
    Building,
    Crafting,
}

public class FSM_WorkingState : FSM_BaseState
{
    public WorkState state;
    Func<bool> condition;
    bool forceExit = false;
    Action<Lumberjack> updateFunc;

    public override void OnEnter(Lumberjack l)
    {
        l.SetSpriteColor(Color.red);
        l.animator.SetBool("isWorking", true);
        switch (state)
        {
            case WorkState.Building:
                condition = () => forceExit == true;
                updateFunc = BuildingUpdate;
                break;
            case WorkState.Crafting: 
                
                break;
            case WorkState.Cutting:
                updateFunc = CuttingUpdate;
                condition = () => l.pickingResource.resistance <= 0;
                break;
        }
    }


    public override void OnExit(Lumberjack l)
    {
        l.animator.SetBool("isWorking", false);
        Debug.Log("WorkEnd");
    }

    public override void Update(Lumberjack l)
    {
        if (condition.Invoke())
        {
            l.ChangeFSM(l.movingState);
            return;
        }
        updateFunc(l);
    }

    void CuttingUpdate(Lumberjack l)
    {
        if (SwipeManager.Cut())
        {
            if (l.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "CutAnim")
                l.Cut();
        }
    }

    void BuildingUpdate(Lumberjack l)
    {
        if (SwipeManager.ConstructMode())
        {
            l.ChangeFSM(l.movingState);
        }
    }
}
