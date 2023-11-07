using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FSM_MovingState : FSM_BaseState
{
    float h = 2.0f;
    float r = 0.5f;
    bool debug = true;
    LayerMask mask = LayerMask.GetMask("Platform");
    Vector3 targetPosition;

    public override void OnEnter(Lumberjack l)
    {
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

        if (SwipeManager.MoveLeft())
        {
            if (TryMove(l, -r))
            {
                l.Move(targetPosition);
                return;
            }
            else if (TryMove(l, -(r + 2)))   // TryJump
            {
                l.Jump(targetPosition);
                return;
            }
            
        }
        else if (SwipeManager.MoveRight())
        {
            if (TryMove(l, r))
            {
                l.Move(targetPosition);
                return;
            }
            else if (TryMove(l, r + 2))   // TryJump
            {
                l.Jump(targetPosition);
                return;
            }
        }
    }

    bool TryMove(Lumberjack l, float exp)
    {
        // Simple step
        Vector2 start = l.transform.position + new Vector3(exp, h, 0);
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.down, 2*h, mask);
        if (hit)
        {
            if (debug)
                Debug.DrawRay(start, hit.point - start, Color.yellow);

            targetPosition = hit.point;
            return true;
            
        }
        return false;
    }
}
