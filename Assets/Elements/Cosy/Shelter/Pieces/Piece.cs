using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool alive = true;
    public GameObject bubble;
    public int solid = 1;
    public int life;
       
    public void SetBubbleVisible(bool isActive)
    {
        bubble.transform.parent.gameObject.SetActive(isActive);
    }

    public void Resist(int windForce)
    {
        if (!alive) return;
        if (Random.Range(1, solid) > windForce) return;

        life--;
        if (life <= 0)
        {
            Break();
        }
    }

    public void Break()
    {
        alive = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        bubble.SetActive(true);
        Shelter.UpdateSpeed(-1);
    }

    public void Repair()
    {
        alive = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        bubble.SetActive(false);
        life = Random.Range(solid, solid + 4);
        Shelter.UpdateSpeed(1);
    }

    /*USP : 
Le vieil habitant des arbres y mettra tout son coeur pour protéger ses compagnons animaux. 

KSP : 
Faites face à la tempête qui pourrait vous coûter la vie
Les animaux apeurés de la forêt seront d’un grand réconfort dans votre foyer. 
Construisez pour progresser sans tomber!
*/
}
