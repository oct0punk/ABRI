using UnityEngine;
using UnityEngine.EventSystems;

public class BridgeBubble : DragDropBubble
{
    AnchorForBridge anchor;
    AnchorForBridge anchorForPreview;

    protected GameObject preview;
    public GameObject previewPrefab;

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

    public override void OnDragCorrect(PointerEventData eventData)
    {
        base.OnDragCorrect(eventData);

        if (anchor == null) return;
        if (anchorForPreview == anchor) return;
        anchorForPreview = anchor;
        
        if (preview == null)
            preview = Instantiate(previewPrefab);

        preview.name = "Preview";
        preview.transform.position = Vector3.zero;
        preview.GetComponent<Bridge>().Preview(anchorForPreview.left, anchorForPreview.right);
        Debug.Log("Preview");
    }

    public override void OnDragUncorrect(PointerEventData eventData)
    {
        base.OnDragUncorrect(eventData);
        if (preview != null)
        {
            anchorForPreview = null;
            Destroy(preview);
            preview = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (preview != null)
        {
            anchorForPreview = null;
            Destroy(preview.gameObject);
            Debug.Log("Destroy");
            preview = null;
        }
        base.OnEndDrag(eventData);
    }

    protected override void OnSuccess(PointerEventData eventData)
    {
        anchor.Build();
        base.OnSuccess(eventData);
    }
}
