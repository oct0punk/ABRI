using UnityEngine;
using Cinemachine;
using System;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    public static Shelter instance { get { if (Instance == null) Instance = FindObjectOfType<Shelter>(); return Instance; } }
    static Shelter Instance;
    [SerializeField] CinemachineVirtualCamera cam;
    [Space]
    [Header("Storm")]
    [SerializeField] float timeBeforeNextGust = 1.0f;
    Storm storm;
    [Space]
    public Piece[] pieces;
    bool restored = true;


    void Awake()
    {
        storm = GetComponent<Storm>();
        pieces = GetComponentsInChildren<Piece>();
    }



    private void Update()
    {
        timeBeforeNextGust -= Time.deltaTime;
        if (timeBeforeNextGust < 0.0f)
        {
            Bird.SendClueToPlayer(5);
            timeBeforeNextGust = UnityEngine.Random.Range(40.0f, 100.0f);
            foreach (Piece p in pieces)
            {
                p.Resist(storm.wind);
            }

            if (GameManager.instance.gameState == GameState.Explore)
            {
                if (Array.TrueForAll(pieces, p => p.life < 2))
                {
                    if (Array.TrueForAll(pieces, p => p.life == 1))
                        pieces[0].life = 3;
                    else
                        Lumberjack.Instance.Message("J'ai un mauvais pressentiment. Mon abri a sûrement besoin d'être réfistolé!");
                }
                else
                {
                    Lumberjack.Instance.Message("Ouf, quelle rafale! J'espère que mon abri l'a encaissé...");
                }
            }
            else
            {
                if (Array.TrueForAll(pieces, p => p.build))
                {
                    Lumberjack.Instance.Message("ça souffle bien dehors.");
                    restored = true;
                }
                else
                {
                    Lumberjack.Instance.Message("Wouaaahh !! La tempête a fait un trou dans le mur !");
                    restored = false;
                }
            }
        }
    }

    public void OnPieceUpdated(bool isGood)
    {
        if (!isGood)
        {
            if (Array.TrueForAll(pieces, p => !p.build))
            { // Abri détruit
                enabled = false;
                GameManager.instance.ChangeState(GameState.GameOver);
            }
        }
    }


    public void OnEnter()
    {
        cam.Priority = 1;

        if (restored)
        {
            if (!Array.TrueForAll(pieces, p => p.build))
            {
                restored = false;
                Lumberjack.Instance.Message("Il y a des trous dans mon abri. Si ça continue, je ne pourrai pas sauver l'oiseau...", 3.0f);
            }
        }
    }

    public void OnExit()
    {
        cam.Priority = -1;

        if (!restored)
        {
            if (Array.TrueForAll(pieces, p => p.build))
            {
                Lumberjack.Instance.Message("Tout est en ordre, je peux reprendre les recherches.");
                restored = true;
            }
            else
                Lumberjack.Instance.Message("Il me faut du bois pour réparer mon abri.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
            GameManager.instance.ChangeState(GameState.Indoor);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
            GameManager.instance.ChangeState(GameState.Explore);
    }
}