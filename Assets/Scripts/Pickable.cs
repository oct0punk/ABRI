using System.Collections;
using UnityEngine;

[SelectionBase]
public class Pickable : MonoBehaviour
{
    public int maxResistance = 1;
    public int amount = 1;
    public bool alive = true;
    [Space]
    public GameObject trail;
    public GameObject swipeTuto;
    public int resistance { get; private set; }
    [SerializeField] Sprite[] sprites;
    static bool doOnce = true;
    [SerializeField] ParticleSystem[] fx;

    private void Awake()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.position = hit.point;
            transform.SetParent(hit.transform);
        }

        if (alive)
            Reset();
        else        
            OnDie();            
        
    }

    public void Resist(Lumberjack l)
    {
        resistance -= l.force;
        AudioManager.Instance.Play("Cut");
        if (resistance <= 0)
            OnDie(l);
    }

    void OnDie()
    {
        alive = false;
        AudioManager.Instance.Play("Collect");
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        StartCoroutine(Revive());
        foreach (var f in fx)
            f.Play();
        
        if (doOnce)
        {
            doOnce = false;
            FindObjectOfType<Storm>().enabled = true;
            Lumberjack.Instance.Message("Une p'tite branche pour construire le chemin");
        }
    }
    void OnDie(Lumberjack l)
    {
        OnDie();
        l.OnResExit(this);
        l.Collect(this);
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(Random.Range(30, 60));
        Reset();
    }

    [ContextMenu("Reset")]
    private void Reset()
    {
        alive = true;
        resistance = maxResistance;

        bool rand = (int)(Time.timeSinceLevelLoad + transform.position.x) % 2 == 0;

        SpriteRenderer sprRen = GetComponentInChildren<SpriteRenderer>();
        sprRen.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.localScale = rand ? Vector3.one : new Vector3(-1, 1, 1);
        sprRen.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alive) {
            Bird.SendClueToPlayer(2, 5);
            return;
        }
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            if (lum.hasCaught) return;
            swipeTuto.SetActive(true);
            lum.OnResEnter(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!alive) return;
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            if (lum.hasCaught) return;
            lum.OnResExit(this);
        }
    }

    public void CanCut(bool canCut)
    {
        trail.SetActive(canCut);
        swipeTuto.SetActive(canCut);
    }
}
