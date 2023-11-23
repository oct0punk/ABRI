using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Lumberjack : MonoBehaviour
{
    [Min(0)]
    public int speed = 3;
    public int force = 1;

    [HideInInspector] public int x = 1;

    #region External datas
    public CinemachineVirtualCamera cam;
    public Ladder ladderPrefab;
    public Resource pickingResource { get; private set; }
    #endregion

    #region Owning Components
    public Animator animator { get; private set; }
    public List<Resource> canCutRes { get; private set; }
    private Storage storage;
    #endregion

    #region FSM
    public FSM_BaseState fsm { get; private set; }
    public FSM_MovingState movingState { get; private set; }
    public FSM_ClimbingState climbingState { get; private set; }
    public FSM_JumpingState jumpingState{ get; private set; }
    public FSM_WorkingState workingState { get; private set; }
    #endregion


    private void Start()
    {
        // Component
        animator = GetComponent<Animator>();
        canCutRes = new List<Resource>();
        storage = GetComponent<Storage>();

        // FSM
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
    private void LateUpdate()
    {
        ResetGlobalScale();
        transform.rotation = Quaternion.identity;
    }

    public void ChangeFSM(FSM_BaseState newState)
    {
        fsm.OnExit(this);        
        fsm = newState;
        fsm.OnEnter(this);
    }

    public void Move(Vector3 targetPos)
    {
        Vector3 delta = targetPos - transform.position;
        delta = Vector3.ClampMagnitude(delta, Time.deltaTime * speed);
        transform.position += delta;
        if (delta.magnitude >= Time.deltaTime * speed)
            x = (int)Mathf.Sign(delta.x);
    }
    public void Jump(Vector3 landAt)
    {
        jumpingState.land = landAt;
        ChangeFSM(jumpingState);
    }

    public void ClimbUp(Ladder ladder)
    {
        Vector3 pos = ladder.transform.position + Vector3.up * ladder.getHeight();
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 2, LayerMask.GetMask("Platform"));
        if (hit)
            pos = hit.point;

        Climb(ladder, pos);
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
    
    public void SetSpriteColor(Color c)
    {
        GetComponentInChildren<SpriteRenderer>().color = c;
    }
    public void ResetGlobalScale()
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(x / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);
    }

    public void BuildLadder()
    {
        if (transform.parent.tag != "Platform") return;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (!hit) return;
        Ladder lad = Instantiate(ladderPrefab, transform.position + Vector3.down * .5f + Vector3.forward * 2, Quaternion.identity).GetComponent<Ladder>();
        lad.SetHeight(Vector2.Distance(transform.position, hit.point) + 2);
    }

    public void OnResEnter(Resource res)
    {
        canCutRes.Add(res);
        animator.SetBool("CanCut", true);
    }

    public void OnResExit(Resource res)
    {
        canCutRes.Remove(res);
        if (canCutRes.Count == 0)
            animator.SetBool("CanCut", false);
    }

    public void StartCutting()
    {
        pickingResource = canCutRes[0];
        if (pickingResource == null) return;

        Debug.Log("StartCutting", pickingResource);
        workingState.state = WorkState.Cutting;
        ChangeFSM(workingState);
    }
    public void Cut()
    {
        animator.SetTrigger("Cut");
        pickingResource.Resist(this);
    }

    public void Collect(Resource res)
    {
        storage.Add(res.type, 1);
    }
}
