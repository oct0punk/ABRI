using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool alive = true;
    public GameObject button;
    public ConsumeBubble bubble;
    public int solid = 1;
    public int life;

    private void Start()
    {
        SetBubbleVisible(false);
        button.SetActive(false);
        bubble.action = Repair;
        if (!alive) Break();
    }

    public void SetBubbleVisible(bool isActive)
    {
        bubble.gameObject.SetActive(isActive);
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
        GetComponent<SpriteRenderer>().color = Color.black;
        button.SetActive(true);
        Shelter.UpdateSpeed(-1);
    }

    public void Repair()
    {
        alive = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        button.SetActive(false);
        life = Random.Range(solid, solid + 4);
        Shelter.UpdateSpeed(1);
    }

    /*USP : 
Le vieil habitant des arbres y mettra tout son coeur pour prot�ger ses compagnons animaux. 

KSP : 
Faites face � la temp�te qui pourrait vous co�ter la vie
Les animaux apeur�s de la for�t seront d�un grand r�confort dans votre foyer. 
Construisez pour progresser sans tomber!
*/
}
