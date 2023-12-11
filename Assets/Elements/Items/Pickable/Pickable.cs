using System.Collections;
using UnityEngine;

[SelectionBase]
public class Pickable : MonoBehaviour
{
    public PickableMaterial material;
    public int maxResistance = 1;
    public bool alive = true;
    public int resistance { get; private set; }
    new Collider2D collider;
    public GameObject trail;
    public Canvas canvas;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        Reset();
        trail.SetActive(false);
        canvas = GetComponentInChildren<Canvas>();
    }

    public void Resist(Lumberjack l)
    {
        resistance -= l.force;
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
        if (material.revivable)
        {
            StartCoroutine(Revive());
        }
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(Random.Range(material.timeBeforeRevive.x, material.timeBeforeRevive.y));
        Reset();
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

    public void CanCut(bool canCut, Lumberjack lum)
    {
        trail.SetActive(canCut);
    }
}
