using UnityEngine;

public class Piece : Construction
{
    [Header("Piece")]
    public int solid = 1;
    public int life;
    Rigidbody2D rb;

    private new void Awake()
    {
        base.Awake();
        if (!buildOnStart) Break();
    }

    public void Resist(int windForce)
    {
        if (!build) return;
        if (Random.Range(1, solid) > windForce) return;

        life--;
        if (life <= 0)
        {
            Break();
        }
    }

    private void Start()
    {
        rb = gameObject.AddComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }
    private void Update()
    {
        rb.AddForce(Vector2.right * -1);
        rb.AddTorque(-Random.Range(0.0f, 2.0f));
    }

    public override void Break()
    {
        if (enabled) return;
        base.Break();
        if (GameManager.instance.gameState == GameState.Indoor)
            AudioManager.Instance.Play("OnPieceDie");
        Shelter.instance.OnPieceUpdated(false);
        Debug.Log("instance");
        GetComponent<SpriteRenderer>().enabled = false;
        Piece clone = Instantiate(this);
        clone.enabled = true;
    }

    public override void Build()
    {
        base.Build();
        if (!build) return;
        life = Random.Range(solid, solid + 4);
        Shelter.instance.OnPieceUpdated(true);

        GetComponent<SpriteRenderer>().enabled = true;
    }

    /*USP : 
Le vieil habitant des arbres y mettra tout son coeur pour protéger ses compagnons animaux. 

KSP : 
Faites face à la tempête qui pourrait vous coûter la vie
Les animaux apeurés de la forêt seront d’un grand réconfort dans votre foyer. 
Construisez pour progresser sans tomber!
*/

}
