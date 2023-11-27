using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public bool forceExit = false;
    Action<Lumberjack> updateFunc;
    internal Workbench workBench;

    public override void OnEnter(Lumberjack l)
    {
        forceExit = false;
        l.SetSpriteColor(Color.red);
        l.animator.SetBool("isWorking", true);
        switch (state)
        {
            case WorkState.Building:
                condition = () => forceExit == true;
                updateFunc = BuildingUpdate;
                l.constructUI.SetActive(true);
                break;
            case WorkState.Crafting:
                updateFunc = CraftUpdate;
                condition = () => forceExit == true;
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
        if (SwipeManager.ConstructMode())
        {
            l.constructUI.SetActive(false);
            l.ChangeFSM(l.movingState);
        }
    }

    void CraftUpdate(Lumberjack l)
    {

    }
}
