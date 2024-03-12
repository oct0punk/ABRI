using UnityEngine;
using Cinemachine;
using System;
using System.Collections.Generic;
using TMPro;

[SelectionBase]
public class Shelter : MonoBehaviour
{
    public static Shelter instance { get { if (Instance == null) Instance = FindObjectOfType<Shelter>(); return Instance; } }
    static Shelter Instance;
    [SerializeField] CinemachineVirtualCamera cam;
    [Space]
    [Header("Storm")]
    [Space]
    public Piece[] pieces;
    [HideInInspector] public bool restored = true;
    List<GameObject> tapsOutside = new();
    public Tap tap { get; private set; }


    void Awake()
    {
        tap = GetComponentInChildren<Tap>();
        pieces = GetComponentsInChildren<Piece>();        
        foreach (var tap in FindObjectsOfType<Tap>())
            if (tap != this.tap)
                tapsOutside.Add(tap.GetComponentInParent<Canvas>().gameObject);        
    }


    public void OnPieceUpdated(bool isGood)
    {
        if (!isGood)
        {
            if (Array.TrueForAll(pieces, p => !p.build))
            { // Abri détruit
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

        foreach (var piece in pieces)
            if (!piece.build)
                foreach (var t in piece.taps)
                    if (t.GetComponentInChildren<Canvas>() != null)
                        t.SetActive(true);

        foreach (var go in tapsOutside)
        { go.SetActive(false); }
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

        foreach (var piece in pieces)
            foreach (var t in piece.taps)
                if (t.GetComponentInChildren<Canvas>() != null)
                    t.SetActive(false);

        foreach (var cons in FindObjectsOfType<Construction>())
        {
            if (cons is not Piece)
            {
                if (!cons.build)
                    Array.ForEach(cons.taps, t => t.gameObject.SetActive(true));
            }
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

    public void End()
    {
        if (Lumberjack.Instance.hasCaught)
        {
            GameManager.instance.End();
        }
        else
        {
            tap.gameObject.SetActive(false);
            Lumberjack.Instance.Message("Il faut retrouver cet oiseau, il ne survivra pas à cette tempête infernale.");
        }
    }
}