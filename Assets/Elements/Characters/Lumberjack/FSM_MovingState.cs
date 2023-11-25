

using UnityEngine;


public class FSM_MovingState : FSM_BaseState
{
    float h = 1.0f;
    float r = 0.5f;
    LayerMask mask = LayerMask.GetMask("Platform");
    Vector3 targetPosition;

    public override void OnEnter(Lumberjack l)
    {
        l.SetSpriteColor(Color.white);
        l.animator.SetBool("isWalking", false);
    }

    public override void OnExit(Lumberjack l)
    {
        l.animator.SetBool("isWalking", false);
    }

    public override void Update(Lumberjack l)
    {        
        // Stabilize
        RaycastHit2D hit = Physics2D.Raycast(l.transform.position + new Vector3(0, h, 0), Vector2.down, 3, mask);
        if (hit)
        {
            l.Move(hit.point);
            if (l.transform.parent != hit.transform)
            {
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

        if (SwipeManager.ConstructMode())
        {
            l.ConstructMode();
            return;
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
            else if (TryMove(l, -(r + 2), 1.5f, h))   // TryJump
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
            else if (TryMove(l, r + 2, 1.5f, h))   // TryJump
            {
                l.Jump(targetPosition);
                return;
            }
        }

        // Idle
        l.animator.SetBool("isWalking", false);
        
    }

    bool TryMove(Lumberjack l, float exp, float hExp, float hDiff)
    {
        // Simple step
        Vector2 start = l.transform.position + new Vector3(exp, h - hDiff, 0);
        RaycastHit2D hit = Physics2D.Raycast(start, Vector2.down, hExp * h, mask);
        
        if (hit) {
            Debug.DrawLine(start, hit.point, Color.yellow);
            targetPosition = hit.point;
            l.animator.SetBool("isWalking", true);
            return true;            
        }
        else {
            Debug.DrawRay(start, Vector2.down * hExp * h, Color.white);
        }
        
        return false;
    }
}
