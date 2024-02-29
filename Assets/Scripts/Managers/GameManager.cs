using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Menu,    
    Indoor,
    Explore,
    GameOver,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;                   // True if doing fade animation 
    bool isTransitionning;

    public bool pause { get; private set; }

    private void Awake()
    {
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }
    }
    private void Start()
    {
        EnterState(gameState);
    }


    #region GameState

    public void ChangeState(GameState newState)
    {
        if (gameState == newState) return;
        ExitState(gameState);
        gameState = newState;
        EnterState(gameState);
    }

    void ExitState(GameState state) {
        switch (state)
        {
            case GameState.GameOver:
                Time.timeScale = 1.0f;
                break;
            case GameState.End: 
                Time.timeScale = 1.0f; 
                break;
        }
    }

    void EnterState(GameState state) {
        switch (state)
        {
            case GameState.Indoor:
                if (isTransitionning) return;
                StartCoroutine(Transition(() => {
                    Shelter.instance.OnEnter();
                    AudioManager.Instance.Play("Indoor");
                    AudioManager.Instance.Stop("Outdoor");
                }));
                break;
            case GameState.Explore:
                if (isTransitionning) return;
                StartCoroutine(Transition(() => {
                    Shelter.instance.OnExit();
                    AudioManager.Instance.Play("Outdoor");
                    AudioManager.Instance.Stop("Indoor");
                }));
                break;
            case GameState.Menu:
                SceneManager.LoadScene(0);
                break;
            case GameState.End:
                SceneManager.LoadScene(2);
                break;
            case GameState.GameOver:
                GameUI.instance.GameOver();
                Time.timeScale = 0.0f;
                Lumberjack.Instance.enabled = false;
                break;
        }
    }

    #endregion

    IEnumerator Transition(Action func)
    {
        isTransitionning = true;
        yield return GameUI.instance.Transition(1);
        func();
        yield return GameUI.instance.Transition(0);
        isTransitionning = false;
    }


    public void Launch()
    {
        ItemsManager.Instance.Reset();
        SceneManager.LoadScene(1);
    }

    public void SetPause(bool pause)
    {
        this.pause = pause;
        Time.timeScale = pause ? 0.0f : 1.0f;
    }
}