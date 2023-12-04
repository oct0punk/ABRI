using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Lumberjack : MonoBehaviour
{
    #region properties

    [Min(0)]
    public int speed = 3;
    public int force = 1;

    [HideInInspector] public bool indoor = false;
    public bool canCut = false;
    public Bird carryingBird;

    public CinemachineVirtualCamera cam;
    public Pickable pickingResource { get; private set; }
    LayerMask mask;
    float coolDown = 0.0f;

    [Space]
    [Header("Constructions")]
    public GameObject constructUI;
    public GameObject plans;
    public GameObject thinkObj;


    #region Owning Components
    public Animator animator { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public List<Pickable> canCutRes { get; private set; }
    public Storage storage { get; private set; }
    public ThinkBubble thinkBubble;
    #endregion

    #region FSM
    public FSM_BaseState fsm { get; private set; }
    public FSM_MovingState movingState { get; private set; }
    public FSM_IdleState idleState { get; private set; }
    public FSM_ClimbingState climbingState { get; private set; }
    public FSM_JumpingState jumpingState{ get; private set; }
    public FSM_WorkingState workingState { get; private set; }
    #endregion

    #endregion


    private void Awake()
    {
        // Component
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        canCutRes = new List<Pickable>();
        storage = GetComponent<Storage>();
        mask = LayerMask.GetMask("Platform");

        // FSM
        movingState = new FSM_MovingState();
        idleState = new FSM_IdleState();
        climbingState = new FSM_ClimbingState();
        jumpingState = new FSM_JumpingState();
        workingState = new FSM_WorkingState();
        fsm = idleState;
        fsm.OnEnter(this);

        constructUI.SetActive(true);
        ThinkOf(false);
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


    private void OnDisable()
    {
        if (fsm == idleState)
            idleState.OnEnter(this);
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
    public void ResetGlobalScale()
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);
    }

    #region Move
    public void Move(Vector3 targetPos)
    {
        Vector3 delta = targetPos - transform.position;
        delta = Vector3.ClampMagnitude(delta, Time.deltaTime * speed);
        transform.position += delta;
        if (delta.magnitude >= Time.deltaTime * speed)
        {
            bool res = (int)Mathf.Sign(delta.x) == -1;
            if (res != spriteRenderer.flipX)
            {
                // changeDir feedback
                if (delta.x > 0)
                    GameManager.instance.ui.MoveRight();
                else
                    GameManager.instance.ui.MoveLeft();
            }
            spriteRenderer.flipX = res;
        }
    }
    public void Jump(Vector3 landAt)
    {
        jumpingState.land = landAt;
        ChangeFSM(jumpingState);
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
        StartCoroutine(WaitEndOfLadder(ladder));
    }

    IEnumerator WaitEndOfLadder(Ladder ladder)
    {
        yield return new WaitWhile(() => fsm == climbingState);
        ladder.arrow.SetActive(true);
    }
    #endregion
    #endregion
    

    #region Res
    // Called when a resource ENTER the zone
    public void OnResEnter(Pickable res)
    {
        canCutRes.Add(res);
        if (res == canCutRes[0])
            res.CanCut(true, this);
        canCut = true;
        animator.SetBool("CanCut", true);
        // Trail emits
    }

    // Called when a resource EXIT the zone
    public void OnResExit(Pickable res)
    {
        res.CanCut(false, this);
        canCutRes.Remove(res);
        if (canCutRes.Count == 0)
        {
            animator.SetBool("CanCut", false);
            canCut = false;
            // Trail doesn't emit
        }
        else
        {
            canCutRes[0].CanCut(true, this);
        }
    }
    #endregion


    // Deploy plans in bubbles
    public void DisplayPlans()
    {
        thinkObj.SetActive(false);
        plans.SetActive(true);
    }

    // Deploy the bubble "..."
    public void ThinkOf(bool isActive)
    {
        thinkObj.SetActive(isActive);
        plans.SetActive(false);
        constructUI.GetComponent<RectTransform>().pivot = new Vector2(spriteRenderer.flipX ? 1.0f : .0f, .0f);
    }



    public Coroutine Message(GameObject obj, Func<bool> condition)
    {
        return thinkBubble.Message(obj, condition);
    }

    public Coroutine Message(GameObject obj, float time)
    {
        return thinkBubble.Message(obj, time);
    }
    IEnumerator CoolDown()
    {
        while (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }



    #region Work
    public void StartCutting()
    {
        pickingResource = canCutRes[0];
        if (pickingResource != null) 
        {
            if (!storage.CanFill(pickingResource.material))
            {
                Image image = new GameObject().AddComponent<Image>();
                image.sprite = pickingResource.material.icon;
                Message(image.gameObject, 2.0f);
                return;
            }

            workingState.state = WorkState.Cutting;
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
    {
        pickingResource.Resist(this);
    }


    public void Collect(Pickable res)
    {
        storage.Add(res.material, 1);
    }

    public void ConstructMode(bool active)
    {
        if (active)
        {
            DisplayPlans();
            workingState.state = WorkState.Building;
            ChangeFSM(workingState);
        }
        else
        {
            ThinkOf(false);
            ChangeFSM(idleState);
        }
    }

    public void WorkbenchMode(Workbench workbench)
    {
        workingState.workBench = workbench;
        workingState.state = WorkState.Crafting;
        ChangeFSM(workingState);
    }

    public void ExitCraftingMode()
    {
        workingState.canExit = true;
    }
    #endregion
}
