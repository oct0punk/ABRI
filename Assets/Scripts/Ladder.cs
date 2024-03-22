using UnityEngine;

[SelectionBase]
public class Ladder : Construction, IFix
{
    [Header("Ladder")]
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] SpriteRenderer spriteUp;
    [SerializeField] SpriteRenderer spriteDown;
    [Space]
    public static bool Tuto = true;
    public GameObject arrow;
    public GameObject swipeTuto;
    
    float height;
    BoxCollider2D box;
    Lumberjack l;

    new void Awake()
    {
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
        base.Awake();
    }

    public void SetHeight(float height)
    {
        this.height = height;
        sprite.size = new Vector2(sprite.size.x, height);
        box.size = new Vector2(3, height);
        box.offset = new Vector2(0, height / 2);

        spriteDown.transform.localPosition = Vector3.zero;
        spriteUp.transform.localPosition = new Vector3(0, height - 1.0f, 0);

        buildFX.transform.localPosition = new Vector3(0, height / 2, -5);
        var shape = buildFX.shape;
        shape.scale = new Vector3(1, height, 1);
    }

    public override void Build()
    {
        base.Build();
        if (!build) return;
        sprite.enabled = true;
        box.enabled = true;
    }

    public Vector3 Top()
    {
        return transform.position + transform.up * height;
    }


    private void Update()
    {
        //transform.rotation = Quaternion.identity;

        if (l == null) return;
        if (l.fsm == l.climbingState) return;
        if (l.fsm == l.workingState) return;

        if (SwipeManager.ClimbUp())
        {
            if (isAtBottom(l))
            {
                l.AutoMoveTo(transform.position, () => l.Climb(this, false), () => transform.position);
                arrow.SetActive(false);
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.76f, height - .5f, 0.0f), Quaternion.identity);
                arrow.transform.transform.localScale = Vector3.one;
            }
        }
        else if (SwipeManager.ClimbDown())
        {
            if (!isAtBottom(l))
            {
                l.AutoMoveTo(Top(), () => l.Climb(this, true), () => Top());
                arrow.SetActive(false);
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.76f, .5f, 0.0f), Quaternion.identity);
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
                Vector3.Distance(lum.transform.position, transform.position + Vector3.up * height);
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
            arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.5f, height, 0.0f), Quaternion.Euler(0, 0, 90));
        }
        swipeTuto.SetActive(Tuto);
    }
}
