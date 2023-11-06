using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_MovingState : FSM_BaseState
{
    public override void OnEnter(Lumberjack l)
    {
        l.SetSpriteColor(Color.blue);
    }

    public override void OnExit(Lumberjack l)
    {
        throw new System.NotImplementedException();
    }

    public override void Update(Lumberjack l)
    {
        // Debug.DrawRay(l.transform.position + new Vector3(0, 2, 0), Vector3.down * 3);
        LayerMask mask = LayerMask.GetMask("Platform");
        
        RaycastHit2D hit = Physics2D.Raycast(l.transform.position + new Vector3(0, 2, 0), Vector2.down, 3, mask);
        if (hit)
        {
            l.Move(hit.point);
            if (l.transform.parent != hit.transform)
            {
                l.transform.SetParent(hit.transform);
            }
        }
        l.transform.rotation = Quaternion.identity;

        if (SwipeManager.MoveLeft())
        {
            RaycastHit2D hitL = Physics2D.Raycast(l.transform.position + new Vector3(-.7f, 2, 0), Vector2.down, 3, mask);
            if (hitL)
                l.Move(hitL.point);
        }
        
        if (SwipeManager.MoveRight())
        {
            RaycastHit2D hitR = Physics2D.Raycast(l.transform.position + new Vector3(.7f, 2, 0), Vector2.down, 3, mask);
            if (hitR)
                l.Move(hitR.point);
        }
    }
}
