using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FSM_MovingState : FSM_BaseState
{
    float h = 2.0f;
    float r = 0.8f;
    bool debug = true;
    LayerMask mask = LayerMask.GetMask("Platform");
    Vector3 targetPosition;

    public override void OnEnter(Lumberjack l)
    {
        targetPosition = l.transform.position;
        l.SetSpriteColor(Color.blue);
    }

    public override void OnExit(Lumberjack l)
    {

    }

    public override void Update(Lumberjack l)
    {        
        RaycastHit2D hit = Physics2D.Raycast(l.transform.position + new Vector3(0, h, 0), Vector2.down, 3, mask);
        if (hit)
        {
            l.Move(hit.point);
            if (l.transform.parent != hit.transform)
            {
                l.transform.SetParent(hit.transform);
                Debug.Log("New parent : " + hit.collider.gameObject.name);
            }
        }
        l.transform.rotation = Quaternion.identity;

        if (SwipeManager.MoveLeft())
        {
            if (TryMove(l, -1.0f)) return;
        }
        else if (SwipeManager.MoveRight())
        {
            if (TryMove(l, 1.0f)) return;
        }
        else
        {
            targetPosition = l.transform.position;
        }

        l.Move(targetPosition);
    }

    bool TryMove(Lumberjack l, float hAxis)
    {
        // Simple step
        Vector2 start = l.transform.position + new Vector3(r * hAxis, h, 0);
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.down, 2*h, mask);
        if (hit)
        {
            if (debug)                
                Debug.DrawRay(start, hit.point - start, Color.yellow);

            if (Mathf.Abs(hit.point.y - l.transform.position.y) > .1f)
            {
                if (hit.transform != l.transform.parent)
                {
                    l.Jump(hit.point + Vector2.right * hAxis * .5f);
                    return true;
                }
            }
            else
            {
                l.Move(hit.point);
                targetPosition = hit.point;
                return true;
            }
        }
        return false;
    }
}
