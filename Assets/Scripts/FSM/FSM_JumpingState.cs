using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FSM_JumpingState : FSM_BaseState
{
    float t = 0.0f; 
    Vector3 start; 
    public Vector3 land;
    Vector3 localLand;
    Transform parent;

    public override void OnEnter(Lumberjack l)
    {
        parent = null;
        t = .0f;
        l.animator.SetBool("isJumping", true);
        start = l.transform.position;

        // Parents the land pos
        RaycastHit2D hit = Physics2D.Raycast(land + Vector3.up * 2, Vector2.down, 3, LayerMask.GetMask("Platform"));
        if (hit) {
            parent = hit.transform;
            land = hit.point;
            localLand = parent.InverseTransformPoint(land);
        }

        l.spriteRenderer.flipX = (int)Mathf.Sign((land - start).x) == -1;
    }

    public override void OnExit(Lumberjack l)
    {
        l.animator.SetBool("isJumping", false);
    }

    public override void Update(Lumberjack l)
    {
        t += Time.deltaTime / l.jumpDuration;
        
        if (parent)
        {
            land = parent.TransformPoint(localLand);
            Debug.DrawLine(land + Vector3.up, land, Color.red);
        }

        
        l.transform.position = Vector3.Lerp(start, land, l.jumpForwardCurve.Evaluate(t));
        l.transform.position += Vector3.up * l.jumpHeightCurve.Evaluate(t);

        if (t > 1.0f)
        {
            l.Move(land);
            l.ChangeFSM(l.movingState);
        }
    }
}
