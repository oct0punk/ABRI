using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public Lumberjack lumberjack;
    public GameUI ui;

    private void Awake()
    {
        instance = this;
        lumberjack = FindObjectOfType<Lumberjack>();
    }

    private void Start()
    {
        Time.timeScale = 0.0f;
        lumberjack.enabled = false;
        ui.PausePanel.gameObject.SetActive(false);
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
        ui.Resume();
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
        ui.End();
    }
}
