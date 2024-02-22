using UnityEngine;

public class Piece : Construction
{
    [Header("Piece")]
    public int solid = 1;
    public int life;


    private void Start()
    {
        if (!build) Break();
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
        base.Break();
        GetComponent<SpriteRenderer>().color = Color.black;
        Shelter.instance.UpdateSpeed(-1);
    }

    public override void Build()
    {
        base.Build();
        if (!build) return;
        build = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        life = Random.Range(solid, solid + 4);
        Shelter.instance.UpdateSpeed(1);
    }

    /*USP : 
Le vieil habitant des arbres y mettra tout son coeur pour prot�ger ses compagnons animaux. 

KSP : 
Faites face � la temp�te qui pourrait vous co�ter la vie
Les animaux apeur�s de la for�t seront d�un grand r�confort dans votre foyer. 
Construisez pour progresser sans tomber!
*/
}
