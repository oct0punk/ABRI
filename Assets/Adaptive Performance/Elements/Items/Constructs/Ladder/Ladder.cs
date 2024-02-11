using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[SelectionBase]
public class Ladder : MonoBehaviour
{
    Lumberjack l;
    SpriteRenderer sprite;
    BoxCollider2D box;
    public GameObject arrow;
    public GameObject swipeTuto;


    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);
            transform.position = new Vector3(hit.point.x, hit.point.y, 3);
        }
        hit = Physics2D.Raycast(transform.position + Vector3.up , Vector2.up, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            hit = Physics2D.Raycast(hit.point + Vector2.up * 2, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
            if (hit)
                SetHeight(Vector3.Distance(transform.position, hit.point));
        }
    }

    public float getHeight()
    {
        return sprite.bounds.size.y;
    }
    public void SetHeight(float height)
    {
        sprite.transform.localScale = new Vector3(1, height, 1);
        sprite.transform.localPosition = new Vector3(0, height / 2, 0);
        box.size = new Vector2(1, height);
        box.offset = new Vector2(0, height / 2);
    }
    public Vector3 Top()
    {
        return transform.position + transform.up * getHeight();
    }


    private void Update()
    {
        //transform.rotation = Quaternion.identity;

        if (l == null) return;
        if (l.fsm != l.movingState && l.fsm != l.idleState) return;

        if (SwipeManager.ClimbUp())
        {
            if (isAtBottom(l))
            {
                l.ClimbUp(this);
                arrow.SetActive(false);
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.0f, getHeight() - .5f, 0.0f), Quaternion.identity);
                arrow.transform.transform.localScale = Vector3.one;
            }
        }
        else if (SwipeManager.ClimbDown())
        {
            if (!isAtBottom(l))
            {
                l.ClimbDown(this);
                arrow.SetActive(false);
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.0f, .5f, 0.0f), Quaternion.identity);
                arrow.transform.transform.localScale = new Vector3(1, -1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lumberjack p = collision.GetComponentInParent<Lumberjack>();
        if (p != null)
        {
            l = p;
            enabled = true;

            ActivateArrow(l);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Lumberjack p = collision.GetComponentInParent<Lumberjack>();
        if (p != null)
        {
            l = null;
            enabled = false;
            arrow.SetActive(false);
        }
    }


    bool isAtBottom(Lumberjack lum)
    {
        return Vector3.Distance(lum.transform.position, transform.position) <
                Vector3.Distance(lum.transform.position, transform.position + Vector3.up * getHeight());
    }
    public void ActivateArrow(Lumberjack l)
    {
        arrow.SetActive(true);
        if (isAtBottom(l))
        {
            arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.5f, .5f, 0.0f), Quaternion.Euler(0, 0, -90));
        }
        else
        {
            arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.5f, getHeight() - .5f, 0.0f), Quaternion.Euler(0, 0, 90));
        }
        swipeTuto.SetActive(true);
    }
}
