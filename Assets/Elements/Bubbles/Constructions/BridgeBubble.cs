using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BridgeBubble : DragDropBubble
{
    [SerializeField] GameObject bridgePrefab;

    AnchorForBridge anchor;

    protected override bool CanBeBuild(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("Anchor"));
        if (hit) 
        {
            anchor = hit.collider.GetComponentInParent<AnchorForBridge>();
            return  anchor != null && !anchor.isBuilt;
        }        
        return false;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    protected override void OnSuccess(PointerEventData eventData)
    {
        Bridge bridge = Instantiate(bridgePrefab).GetComponent<Bridge>();
        bridge.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (anchor.other == null) {
            Debug.Log("Other null", anchor);
            return;
        }

        if (anchor.type == AnchorType.left)
            bridge.Build(anchor, anchor.other);
        else
            bridge.Build(anchor.other, anchor);

        base.OnSuccess(eventData);
    }
}
