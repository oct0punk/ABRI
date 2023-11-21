using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameUI ui;

    private void Awake()
    {
        instance = this;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        ui.Resume();
    }

    public void Pause()
    {        
        Time.timeScale = .0f;
        ui.Pause();
    }

    public void GameOver()
    {
        ui.GameOver();
    }

    public void Outro()
    {
        Time.timeScale = .0f;
        ui.End();
    }
}
