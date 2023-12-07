using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RepairBubble : DragDropBubble
{
    Piece p;

    protected override bool CanBeBuild(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 20.0f);
        p = hit.transform.GetComponent<Piece>();
        return p != null;
    }

    protected override void OnSuccess(PointerEventData eventData)
    {
        if (p != null)
        {
            p.Repair();
        }
        base.OnSuccess(eventData);
    }
}
