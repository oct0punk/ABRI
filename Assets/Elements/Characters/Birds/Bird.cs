using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Bird : MonoBehaviour
{
    public GameObject bubble;
    public NestBox perch;


    public Bird_BaseState fsm { get; private set; }
    public FSM_StandyState standbyState { get; private set; }
    public FSM_CarriedState carriedState { get; private set; }
    public FSM_FreeState freeState { get; private set; }

    private void Awake()
    {
        standbyState = new FSM_StandyState();
        carriedState = new FSM_CarriedState();
        freeState = new FSM_FreeState();
        fsm = standbyState;
        fsm.OnEnter(this);
    }

    public void ChangeState(Bird_BaseState newState)
    {
        fsm.OnExit(this);
        fsm = newState;
        fsm.OnEnter(this);
    }

    private void Update()
    {
        fsm.Update(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        fsm.OnTriggerEnter2D(this, collision);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        fsm.OnTriggerStay2D(this, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        fsm.OnTriggerExit2D(this, collision);
    }

    public void CarryBy(Lumberjack lum)
    {
        lum.carryingBird = this;
        transform.SetParent(lum.transform);
        ChangeState(carriedState);
        
    }

    public void Free()
    {
        Debug.Log("Free");
        transform.parent = null;
        ChangeState(freeState);
    }
}
