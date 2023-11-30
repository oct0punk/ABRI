
using UnityEngine;
using UnityEngine.EventSystems;

public class LadderBubble : DragDropBubble
{
    [SerializeField] GameObject ladderPrefab;
    Ladder ladder;
    Vector3 bottom;
    Vector3 top;


    void MoveLadder()
    {
        if (ladder == null)
            ladder = Instantiate(ladderPrefab).GetComponent<Ladder>();
        ladder.transform.SetPositionAndRotation(bottom + Vector3.down * .5f, Quaternion.identity);
        ladder.SetHeight(Vector2.Distance(bottom, top) + 2);
    }

    protected override bool CanBeBuild(PointerEventData eventData)
    {
        
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hitDown = Physics2D.Raycast(worldPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (!hitDown) return false;
        RaycastHit2D hitUp = Physics2D.Raycast(worldPos, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (!hitUp) return false;

        if (hitDown.collider == hitUp.collider) return false;

        bottom = hitDown.point;
        top = hitUp.point;
        bottom.z = top.z = 2;
        return true;
    }

    public override void OnDragCorrect(PointerEventData eventData)
    {
        MoveLadder();
        ladder.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, .4f);
        base.OnDragCorrect(eventData);
    }

    public override void OnDragUncorrect(PointerEventData eventData)
    {
        if (ladder != null)
            Destroy(ladder.gameObject);
        base.OnDragUncorrect(eventData);
    }

    protected override void OnSuccess(PointerEventData eventData)
    {
        MoveLadder();
        ladder.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        ladder = null;
        base.OnSuccess(eventData);
    }

    protected override void OnError(PointerEventData eventData)
    {
        base.OnError(eventData);
    }
}
