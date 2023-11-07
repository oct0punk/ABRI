using UnityEngine;

public class Lumberjack : MonoBehaviour
{
    [Min(0)]
    public int speed = 3;


    FSM_BaseState fsm;
    public FSM_MovingState movingState { get; private set; }
    public FSM_ClimbingState climbingState { get; private set; }
    public FSM_JumpingState jumpingState{ get; private set; }
    public FSM_WorkingState workingState { get; private set; }

    private void Start()
    {
        movingState = new FSM_MovingState();
        climbingState = new FSM_ClimbingState();
        jumpingState = new FSM_JumpingState();
        workingState = new FSM_WorkingState();
        fsm = movingState;
        fsm.OnEnter(this);
    }

    private void Update()
    {
        fsm.Update(this);
    }

    public void ChangeFSM(FSM_BaseState newState)
    {
        fsm.OnExit(this);        
        fsm = newState;
        fsm.OnEnter(this);
    }

    public void SetSpriteColor(Color c)
    {
        GetComponentInChildren<SpriteRenderer>().color = c;
    }

    public void Move(Vector3 targetPos)
    {
        Vector3 delta = targetPos - transform.position;
        delta = Vector3.ClampMagnitude(delta, Time.deltaTime * speed);
        transform.position += delta;
    }

    public void Jump(Vector3 landAt)
    {
        jumpingState.land = landAt;
        ChangeFSM(jumpingState);
    }

    public void ClimbUp(Ladder ladder)
    {
        Climb(ladder, ladder.transform.position + Vector3.up * ladder.getHeight());
    }
    public void ClimbDown(Ladder ladder)
    {
        Climb(ladder, ladder.transform.position);
    }
    void Climb(Ladder ladder, Vector3 pos)
    {
        climbingState.targetPos = pos;
        transform.SetParent(ladder.transform);
        ChangeFSM(climbingState);
    }
}
