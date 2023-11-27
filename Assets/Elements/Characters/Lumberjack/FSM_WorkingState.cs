using System;
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
    public bool canExit = false;
    Action<Lumberjack> updateFunc;
    internal Workbench workBench;

    public override void OnEnter(Lumberjack l)
    {
        canExit = false;
        l.SetSpriteColor(Color.red);
        l.animator.SetBool("isWorking", true);
        switch (state)
        {
            case WorkState.Building:
                condition = () => SwipeManager.ConstructMode();
                updateFunc = BuildingUpdate;
                l.constructUI.SetActive(true);
                break;
            case WorkState.Crafting:
                updateFunc = CraftUpdate;
                condition = () => canExit == true;
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
        switch (state)
        {
            case WorkState.Building:
                l.constructUI.SetActive(false);
                break;
            case WorkState.Crafting:
                workBench.HidePlans();
                break;
            case WorkState.Cutting:
                break;
        }
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

    }

    void CraftUpdate(Lumberjack l)
    {

    }
}
