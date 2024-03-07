using UnityEngine;
using Cinemachine;
using System;
using System.Collections.Generic;

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
    List<GameObject> tapsInside = new();
    List<GameObject> tapsOutside = new();


    void Awake()
    {
        pieces = GetComponentsInChildren<Piece>();        
        foreach (var tap in FindObjectsOfType<Tap>())
        {
            if (tap.GetComponentInParent<Shelter>())
                tapsInside.Add(tap.GetComponentInParent<Canvas>().gameObject);
            else
                tapsOutside.Add(tap.GetComponentInParent<Canvas>().gameObject);
        }
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

        foreach (var go in tapsInside)
        { go.SetActive(true); }

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

        foreach (var go in tapsInside)
        { go.SetActive(false); }

        foreach (var go in tapsOutside)
        { go.SetActive(true); }
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
            GameManager.instance.ChangeState(GameState.End);
        }
        else
        {
            GetComponentInChildren<Tap>().gameObject.SetActive(false);
            Lumberjack.Instance.Message("Il faut retrouver cet oiseau, il ne survivra pas à cette tempête infernale.");
        }
    }
}