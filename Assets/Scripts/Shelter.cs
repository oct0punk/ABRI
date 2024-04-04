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
    public Piece[] pieces;
    [HideInInspector] public bool restored = true;
    List<Tap> tapsOutside = new();
    public Tap tap;


    void Awake()
    {
        pieces = GetComponentsInChildren<Piece>();        
        foreach (var tap in FindObjectsOfType<Tap>())
        {
            if (tap == this.tap) continue;
            if (tap.GetComponentInParent<Piece>() != null) continue;
            tapsOutside.Add(tap);        
        }
    }

    [ContextMenu("InitPieces")]
    void InitPieces()
    {
        pieces = GetComponentsInChildren<Piece>();
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
                Lumberjack.Instance.Message("HolesInShelter", 3.0f);
            }
        }

        if (!Lumberjack.hasCaught)
        {
            foreach (var piece in pieces)
                if (!piece.build)
                    foreach (var t in piece.taps)
                        if (t.GetComponentInChildren<Canvas>() != null)
                            t.SetActive(true);
        }

        foreach (var go in tapsOutside)
        { go.gameObject.SetActive(false); }
    }

    public void OnExit()
    {
        cam.Priority = -1;

        if (!restored)
        {
            if (Array.TrueForAll(pieces, p => p.build))
            {
                Lumberjack.Instance.Message("ShelterIsFine");
                restored = true;
            }
            else
                Lumberjack.Instance.Message("NeedMoreWoodForShelter");
        }

        foreach (var piece in pieces)
            foreach (var t in piece.taps)
                if (t.GetComponentInChildren<Canvas>() != null)
                    t.SetActive(false);

        foreach (var tap in tapsOutside)
        {
            if (tap.cons != null)
                tap.gameObject.SetActive(!tap.cons.build);
            else
                tap.gameObject.SetActive(true);
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
        if (Lumberjack.hasCaught)
        {
            GameManager.instance.End();
        }
        else
        {
            tap.gameObject.SetActive(false);
            Lumberjack.Instance.Message("Motivation");
        }
    }
}