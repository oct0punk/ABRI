using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    Tuto,
    Explore,
    Indoor,
    Craft,
    Build,
    Intro,
    Outro
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;

    [HideInInspector] public Lumberjack lumberjack;
    [HideInInspector] public Shelter shelter;
    public GameUI ui;
    Tuto tuto;

    private void Awake()
    {
        instance = this;
        tuto = GetComponent<Tuto>();
        lumberjack = FindObjectOfType<Lumberjack>();
        shelter = FindObjectOfType<Shelter>();
    }

    private void Start()
    {
        EnterState(gameState);
    }

    public void ChangeState(GameState newState)
    {
        ExitState(gameState);
        gameState = newState;
        EnterState(gameState);
    }

    void ExitState(GameState state) { 
        switch (state)
        {
            case GameState.Intro:
                Time.timeScale = 1.0f;
                ui.Game();                
                break;

            case GameState.Craft:
                lumberjack.ChangeFSM(lumberjack.idleState);
                shelter.DisplayPieceBubble(false);
                shelter.workbench.HidePlans();
                break;


            case GameState.Indoor:  break;
            case GameState.Build:   break;
            case GameState.Explore: break;
            case GameState.Outro:   break;            
            case GameState.Tuto:    break;
        }
    }

    void EnterState(GameState state) {
        switch (state)
        {
            case GameState.Intro:
                ui.Intro();
                Time.timeScale = 0.0f;
                lumberjack.enabled = false;
                break;

            case GameState.Tuto:
                Time.timeScale = 1.0f;
                lumberjack.enabled = false;
                ui.Game();
                tuto.Launch(lumberjack);
                break;

            case GameState.Indoor:
                lumberjack.indoor = true;
                CameraManager.Possess(shelter.cam);
                break;

            case GameState.Craft:
                shelter.workbench.DisplayPlans();
                lumberjack.CraftMode();
                break;
            
            case GameState.Build: 
                // Hide ...
                // Display plans
                break;
            
            
            case GameState.Explore:
                lumberjack.indoor = false;
                CameraManager.Possess(lumberjack.cam); 
                break;

            case GameState.Outro: break;
        }
    }

    public void OnNestBuilt()
    {
        foreach (NestBox nest in FindObjectsOfType(typeof(NestBox)))
        {
            if (!nest.isBuilt) return;
        }

        Outro();
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        ui.Game();
        lumberjack.enabled = true;
    }

    public void Pause()
    {        
        Time.timeScale = .0f;
        ui.Pause();
    }

    public void GameOver()
    {
        ui.GameOver();
        lumberjack.enabled = false;
    }

    public void Outro()
    {
        lumberjack.enabled = false;
        Time.timeScale = .0f;
        ui.Outro();
    }
}
