using UnityEngine;

public class Piece : Construction
{
    [Header("Piece")]
    public int solid = 1;
    public int life;

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

    public override void Break()
    {
        if (enabled) return;
        base.Break();
        Shelter.instance.OnPieceUpdated(false);

        // Feedbacks
        if (GameManager.instance.gameState == GameState.Indoor)
            AudioManager.Instance.Play("OnPieceDie");

        GetComponent<SpriteRenderer>().enabled = false;
        
        GameObject clone = Instantiate(new GameObject(), transform.position, transform.rotation);
        clone.transform.localScale = transform.lossyScale;
        clone.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        clone.AddComponent<Rigidbody2D>().angularVelocity = Random.Range(-10.0f, -100.0f);
        Destroy(clone, 5.0f);
    }

    public override void Build()
    {
        base.Build();
        if (!build) return;
        life = Random.Range(solid, solid + 3);
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
