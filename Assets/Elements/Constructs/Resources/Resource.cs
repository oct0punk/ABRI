using UnityEngine;

[SelectionBase]
public class Resource : MonoBehaviour
{
    public string type;
    public int maxResistance = 1;
    public bool alive = true;
    public int resistance { get; private set; }
    Collider2D collider;
    

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        Reset();
    }

    public void Resist(Lumberjack l)
    {
        resistance -= l.force;
        Debug.Log("Cut : " + resistance, this);
        if (resistance <= 0)
            OnDie(l);
    }

    void OnDie(Lumberjack l)
    {
        alive = false;
        l.OnResExit(this);
        l.Collect(this);
        collider.enabled = false;
        transform.localScale = Vector3.down;
    }

    private void Reset()
    {
        alive = true;
        collider.enabled = true;
        resistance = maxResistance;
        transform.localScale = Vector3.one;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alive) return;
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            lum.OnResEnter(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!alive) return;
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            lum.OnResExit(this);
        }
    }
}
