using System.Collections;
using UnityEngine;

[SelectionBase]
public class Pickable : MonoBehaviour, IFix
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
    static bool Tuto = true;
    public bool flipped { get; private set; }

    private void Awake()
    {
        IFix fix = this;
        fix.Fix(transform, true);

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
        Tuto = false;
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

        flipped = (int)(Time.timeSinceLevelLoad + transform.position.x) % 2 != 0;

        SpriteRenderer sprRen = GetComponentInChildren<SpriteRenderer>();
        sprRen.sprite = sprites[Random.Range(0, sprites.Length)];
        transform.localScale = flipped ? Vector3.one : new Vector3(-1, 1, 1);
        sprRen.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!alive)
        {
            Bird.SendClueToPlayer(2, 5);
            return;
        }
        Lumberjack lum = collision.GetComponentInParent<Lumberjack>();
        if (lum != null)
        {
            if (Lumberjack.hasCaught) return;
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
            if (Lumberjack.hasCaught) return;
            lum.OnResExit(this);
        }
    }

    public void CanCut(bool canCut)
    {
        trail.SetActive(canCut);
        swipeTuto.SetActive(canCut);
        swipeTuto.SetActive(Tuto);
    }
}
