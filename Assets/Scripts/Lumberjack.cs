using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Lumberjack : MonoBehaviour
{
    public static Lumberjack Instance;

    [Min(1)]
    public int speed = 3;
    public int force = 1;
    [SerializeField] ParticleSystem jump_fx;
    [Space]
    public AnimationCurve jumpForwardCurve;
    public AnimationCurve jumpHeightCurve;
    [Range(0.1f, 1.0f)] 
    public float jumpDuration;
    [Space]
    public GameObject autoTarget;
    public CinemachineVirtualCamera cam;
    [HideInInspector] public bool isAutoMoving = false;
    public bool canCut { get; private set; }
    public Pickable pickingResource { get; private set; }
    [Space]
    public HorizontalLayoutGroup hBox;
    public ThinkBubble thinkBubble;
    public Image[] imgs;
    public TextMeshProUGUI[] txts;

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
    public static bool hasCaught;


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

    public void OnPause(bool pause)
    {
        foreach (var t in txts)
            t.enabled = !pause;
        foreach (var i in imgs)
            i.enabled = !pause;
    }

    public void OnCatch()
    {
        hasCaught = true;
        animator.SetBool("Catch", true);
        Message("OnCatch");
        Shelter.instance.tap.gameObject.SetActive(true);
    }

    public void ChangeFSM(FSM_BaseState newState)
    {
        // Debug.Log("FSM : Exit " + fsm.ToString() + ", Enter " + newState.ToString());
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
        if (fsm == climbingState) return;
        if (delta.magnitude >= Time.deltaTime * speed)
        {
            bool res = (int)Mathf.Sign(delta.x) == -1;
            if (res != spriteRenderer.flipX)
            {
                hBox.reverseArrangement = delta.x < 0;
                // changeDir feedback
                if (delta.x > 0)
                    GameUI.instance.MoveRight();
                else
                    GameUI.instance.MoveLeft();
            }
            spriteRenderer.flipX = res;
        }
    }
    public void Step()
    {
        AudioManager.Instance.Play("Step");
    }
    public void JumpFX()
    {
        jump_fx.Play();
    }

    public void Jump(Vector3 landAt)
    {
        AudioManager.Instance.Play("Jump");
        jumpingState.land = landAt;
        ChangeFSM(jumpingState);
    }
    public void AutoMoveTo(Vector3 pos, Action action = null, Func<Vector3> autoUpdateTarget = null)
    {
        pos.z = transform.position.z;
        autoMoveState.targetPos = pos;
        autoMoveState.action = action;
        autoMoveState.updateTarget = autoUpdateTarget;
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
    public void Climb(Ladder ladder, bool climbDown)
    {
        if (Ladder.Tuto == true)
        {
            GameUI.DisableTutoText();
            Ladder.Tuto = false;
        }
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
        // Trail emits
    }
    public void OnResExit(Pickable res)
    {
        res.CanCut(false);
        canCutRes.Remove(res);
        if (canCutRes.Count == 0)
        {
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
            AutoMoveTo(pickingResource.transform.position, () => {
                spriteRenderer.flipX = !pickingResource.flipped;
                ChangeFSM(workingState); 
                Cut(); }, 
                () => pickingResource.transform.position);
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
        Shelter.instance.tap.gameObject.SetActive(false);
        enabled = false;
        yield return Message("intro1", () => !Input.anyKey);
        yield return Message("intro2", () => !Input.anyKey);
        enabled = true;
        Shelter.instance.tap.gameObject.SetActive(true);
    }
}
