using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[SelectionBase]
public class Lumberjack : MonoBehaviour
{
    public static Lumberjack Instance;

    [Min(1)]
    public int speed = 3;
    public int force = 1;

    [HideInInspector] public bool isAutoMoving = false;
    public bool canCut { get; private set; }
    public CinemachineVirtualCamera cam;
    public Pickable pickingResource { get; private set; }
    [Space]
    public ThinkBubble thinkBubble;

    #region Owning Components
    public Animator animator { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public List<Pickable> canCutRes { get; private set; }
    #endregion

    #region FSM
    public FSM_BaseState fsm { get; private set; }
    public FSM_IdleState idleState { get; private set; }
    public FSM_MovingState movingState { get; private set; }
    public FSM_AutoMove autoMoveState { get; private set; }
    public FSM_ClimbingState climbingState { get; private set; }
    public FSM_JumpingState jumpingState { get; private set; }
    public FSM_WorkingState workingState { get; private set; }
    #endregion
    LayerMask mask;
    public bool hasCaught { get; private set; }


    private void Awake()
    {
        Instance = this;
        // Component
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canCutRes = new List<Pickable>();

        mask = LayerMask.GetMask("Platform");

        // FSM
        idleState = new FSM_IdleState();
        movingState = new FSM_MovingState();
        autoMoveState = new FSM_AutoMove();
        climbingState = new FSM_ClimbingState();
        jumpingState = new FSM_JumpingState();
        workingState = new FSM_WorkingState();
        fsm = idleState;
    }
    private void Start()
    {
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

    public void OnCatch()
    {
        hasCaught = true;
        animator.SetBool("Catch", true);
        
        foreach (var construction in FindObjectsOfType<Construction>())
        {
            foreach (var ui in GetComponentsInChildren<Canvas>())
                ui.gameObject.SetActive(false);
        }
    }

    public void ChangeFSM(FSM_BaseState newState)
    {
        fsm.OnExit(this);
        fsm = newState;
        fsm.OnEnter(this);
    }
    public void ResetGlobalScale()
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);
    }

    #region Move
    public void Move(Vector2 targetPos)
    {
        Vector2 delta = targetPos - (Vector2)transform.position;
        delta = Vector2.ClampMagnitude(delta, Time.deltaTime * speed);
        transform.position += (Vector3)delta;
        if (delta.magnitude >= Time.deltaTime * speed)
        {
            bool res = (int)Mathf.Sign(delta.x) == -1;
            if (res != spriteRenderer.flipX)
            {
                // changeDir feedback
                if (delta.x > 0)
                    GameUI.instance.MoveRight();
                else
                    GameUI.instance.MoveLeft();
            }
            spriteRenderer.flipX = res;
        }
    }
    public void Jump(Vector3 landAt)
    {
        jumpingState.land = landAt;
        ChangeFSM(jumpingState);
    }
    public void AutoMoveTo(Vector3 pos, Action action = null)
    {
        pos.z = transform.position.z;
        autoMoveState.targetPos = pos;
        autoMoveState.action = action;
        ChangeFSM(autoMoveState);
    }

    public void Stabilize()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, idleState.h, 0), Vector2.down, 3, mask);
        if (hit)
        {
            Move(hit.point);
            if (transform.parent != hit.transform)
            {
                if (hit.transform.GetComponentInParent<Bridge>())
                    transform.SetParent(hit.transform.GetComponentInParent<Bridge>().transform);
                else
                    transform.SetParent(hit.transform);
            }
        }
    }

    #region Climb
    public void ClimbUp(Ladder ladder)
    {
        Climb(ladder, false);
    }
    public void ClimbDown(Ladder ladder)
    {
        Climb(ladder, true);
    }
    void Climb(Ladder ladder, bool climbDown)
    {
        climbingState.ladder = ladder;
        climbingState.climbDown = climbDown;
        ChangeFSM(climbingState);

        Bird.SendClueToPlayer(2, 3);
    }
    #endregion
    #endregion


    #region Message
    public Coroutine Message(string text, Func<bool> whileCondition)
    {
        thinkBubble.gameObject.SetActive(true);
        return thinkBubble.Message(text, whileCondition);
    }
    public void Message(string text, float time = 0.0f, Action action = null, bool priority = false)
    {
        thinkBubble.gameObject.SetActive(true);
        thinkBubble.Message(text, time, action, priority);
    }
    #endregion


    #region Cutting

    public void OnResEnter(Pickable res)
    {
        canCutRes.Add(res);
        if (res == canCutRes[0])
            res.CanCut(true);
        canCut = true;
        animator.SetBool("CanCut", true);
        // Trail emits
    }
    public void OnResExit(Pickable res)
    {
        res.CanCut(false);
        canCutRes.Remove(res);
        if (canCutRes.Count == 0)
        {
            animator.SetBool("CanCut", false);
            canCut = false;
            // Trail doesn't emit
        }
        else
        {
            canCutRes[0].CanCut(true);
        }
    }

    public void StartCutting()
    {
        pickingResource = canCutRes[0];
        if (pickingResource != null)
        {
            ChangeFSM(workingState);
            Cut();
        }
    }
    public void Cut()
    {
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "CutAnim") return;
        animator.SetTrigger("Cut");
    }
    public void ResistRes()
    { // event for animation
        pickingResource.Resist(this);
    }
    public void Collect(Pickable pickable)
    {
        Move(pickable.transform.position);
        ItemsManager.Instance.CollectWood(pickable.amount);
    }
    #endregion

    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine(Intro());
    }

    IEnumerator Intro()
    {
        enabled = false;
        yield return Message("Cette tempête est féroce.", () => !Input.anyKey);
        yield return Message("je dois à tout prix trouver l'oiseau en voie d'extinction qui vit dans cette forêt", () => !Input.anyKey);
        enabled = true;
    }
}
