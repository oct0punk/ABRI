using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;

public class LadderBubble : DragDropBubble
{
    [SerializeField] GameObject ladderPrefab;
    Vector3 bottom;
    Vector3 top;
    private void Awake()
    {
        base.Awake();
        condition = canBeBuild;
    }

    bool canBeBuild(PointerEventData eventData)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D hitDown = Physics2D.Raycast(worldPos, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (!hitDown) return false;
        RaycastHit2D hitUp = Physics2D.Raycast(worldPos, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (!hitUp) return false;


        bottom = hitDown.point;
        top = hitUp.point;
        bottom.z = top.z = 2;
        return true;
    }

    protected override void OnSuccess(PointerEventData eventData)
    {
        Ladder lad = Instantiate(ladderPrefab, bottom + Vector3.down * .5f, Quaternion.identity).GetComponent<Ladder>();
        lad.SetHeight(Vector2.Distance(bottom, top) + 2);

        base.OnSuccess(eventData);
    }

    protected override void OnError(PointerEventData eventData)
    {
        base.OnError(eventData);
    }
}
