using System;

public class FSM_WorkingState : FSM_BaseState
{
    Func<bool> condition;
    public bool canExit = false;
    Action<Lumberjack> updateFunc;

    public override void OnEnter(Lumberjack l)
    {
        canExit = false;
        condition = () => canExit == true;
        l.animator.SetBool("isWorking", true);
        
        updateFunc = CuttingUpdate;
        condition = () => !l.pickingResource.alive;
        AudioManager.Instance.Play("Whoosh");
    }


    public override void OnExit(Lumberjack l)
    {
        l.animator.SetBool("isWorking", false);
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
