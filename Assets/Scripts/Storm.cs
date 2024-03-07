using System;
using UnityEngine;

public class Storm : MonoBehaviour
{
    [Range(1, 5)]
    public int wind;

    [SerializeField]
    float timeBeforeNextGust = 1.0f;
    Shelter shelter;

    private void Awake()
    {
        shelter = GetComponent<Shelter>();
    }

    private void Update()
    {
        timeBeforeNextGust -= Time.deltaTime;
        if (timeBeforeNextGust < 0.0f)
        {
            enabled = false;
            if (GameManager.instance.gameState == GameState.Explore)
            {
                Lumberjack.Instance.Message("Ouf, quelle rafale! J'espère que mon abri l'a encaissé...", 1.0f, Guts);
            }
            else if (GameManager.instance.gameState == GameState.Indoor)
            {
                Lumberjack.Instance.Message("ça souffle bien dehors.", .5f, Guts);
            }
        }
    }

    public void Guts()
    {
        if (GameManager.instance.gameState == GameState.End) return;

        Bird.SendClueToPlayer(5);
        timeBeforeNextGust = UnityEngine.Random.Range(40.0f, 100.0f);
        enabled = true;

        foreach (Piece p in shelter.pieces)
        {
            p.Resist(wind);
        }


        if (GameManager.instance.gameState == GameState.Explore)
        {
            if (Array.TrueForAll(shelter.pieces, p => p.life < 2))
            {
                if (Array.TrueForAll(shelter.pieces, p => p.life == 1))
                    shelter.pieces[0].life = 3;
                else
                    Lumberjack.Instance.Message("J'ai un mauvais pressentiment. Mon abri a sûrement besoin d'être rafistolé!");
            }
        }
        else if (GameManager.instance.gameState == GameState.Indoor)
        {
            if (Array.TrueForAll(shelter.pieces, p => p.build))
            {
                shelter.restored = true;
            }
            else
            {
                Lumberjack.Instance.Message("Wouaaahh !! La tempête a fait un trou dans le mur !");
                shelter.restored = false;
            }
        }

    }
}
