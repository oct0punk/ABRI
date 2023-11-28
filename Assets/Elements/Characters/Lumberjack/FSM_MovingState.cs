

using UnityEngine;


public class FSM_MovingState : FSM_IdleState
{
    LayerMask mask = LayerMask.GetMask("Platform");
    Vector3 targetPosition;

    public override void OnEnter(Lumberjack l)
    {
        l.SetSpriteColor(Color.blue);
        l.ThinkOf(false);
    }

    public override void OnExit(Lumberjack l)
    {
        l.animator.SetBool("isWalking", false);
    }

    public override void Update(Lumberjack l)
    {
        // Stabilize
        RaycastHit2D hit = Physics2D.Raycast(l.transform.position + new Vector3(0, l.idleState.h, 0), Vector2.down, 3, mask);
        if (hit)
        {
            l.Move(hit.point);
            if (l.transform.parent != hit.transform)
            {
                if (hit.transform.GetComponentInParent<Bridge>())
                    l.transform.SetParent(hit.transform.GetComponentInParent<Bridge>().transform);
                else
                    l.transform.SetParent(hit.transform);
            }
        }

        if (SwipeManager.Cut())
        {
            if (l.canCutRes.Count > 0)
            {
                l.StartCutting();
                return;
            }
        }


        if (Input.touches.Length > 1) return;

        // Move left or right
        if (SwipeManager.MoveLeft())
        {
            if (TryMove(l, -r, 2.0f, 0.0f))
            {
                l.Move(targetPosition);
                return;
            }
            else if (TryMove(l, -(r + 2), 1.5f, -h/2))   // TryJump
            {
                l.Jump(targetPosition);
                return;
            }
            
        }
        else if (SwipeManager.MoveRight())
        {
            if (TryMove(l, r, 2.0f, 0.0f))
            {
                l.Move(targetPosition);
                return;
            }
            else if (TryMove(l, r + 2, 1.5f, -h/2))   // TryJump
            {
                l.Jump(targetPosition);
                return;
            }
        }

        l.ChangeFSM(l.idleState);
        
    }

    bool TryMove(Lumberjack l, float far, float depth, float drop)
    {
        // Simple step
        Vector2 start = l.transform.position + new Vector3(far, h - drop, 0);
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.down, depth * h, mask);
        
        if (hit) {
            Debug.DrawLine(start, hit.point, Color.yellow);
            targetPosition = hit.point;
            l.animator.SetBool("isWalking", true);
            return true;            
        }
        else {
            Debug.DrawRay(start, Vector2.down * depth * h, Color.white);
        }
        
        return false;
    }
}
