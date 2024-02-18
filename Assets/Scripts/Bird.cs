using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] GameObject tap;

    private void Awake()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
            tap.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
            tap.SetActive(false);
    }


    public void Catch()
    {
        GameManager.instance.ChangeState(GameState.End);
    }
}
