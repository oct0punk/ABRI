using UnityEngine;

[SelectionBase]
public class Ladder : MonoBehaviour
{
    Lumberjack l;
    SpriteRenderer sprite;
    BoxCollider2D box;
    public GameObject arrow;
    public GameObject swipeTuto;
    
    public void SetHeight(float height)
    {
        sprite.transform.localScale = new Vector3(1, height, 1);
        sprite.transform.localPosition = new Vector3(0, height / 2, 0);
        box.size = new Vector2(1, height);
        box.offset = new Vector2(0, height / 2);
    }

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Platform"));
        if (hit)
            transform.SetParent(hit.transform);
    }

    public float getHeight()
    {
        return sprite.bounds.size.y;
    }

    private void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 3, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);

            //transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);            
        }
    }

    private void Update()
    {
        //transform.rotation = Quaternion.identity;

        if (l == null) return;
        if (l.fsm != l.movingState && l.fsm != l.idleState) return;

        if (SwipeManager.ClimbUp()) {
            if (isAtBottom(l))
            {
                l.ClimbUp(this);
                arrow.SetActive(false);
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.0f, getHeight() - .5f, 0.0f), Quaternion.identity);
                arrow.transform.transform.localScale = Vector3.one;
            }
        }
        else if (SwipeManager.ClimbDown()) {
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

            arrow.SetActive(GameManager.instance.gameState != GameState.Tuto);            
            if (isAtBottom(l))
            {
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.0f, .5f, 0.0f), Quaternion.Euler(0, 0, -90));
            } 
            else
            {
                arrow.transform.SetLocalPositionAndRotation(new Vector3(-1.0f, getHeight() - .5f, 0.0f), Quaternion.Euler(0, 0, 90));
            }
            swipeTuto.SetActive(Tuto.tutoClimb);

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
        return  Vector3.Distance(lum.transform.position, transform.position) < 
                Vector3.Distance(lum.transform.position, transform.position + Vector3.up * getHeight());
    }
}
