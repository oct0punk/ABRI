
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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
    public GameState gameState;

    public bool pause { get; private set; }

    private void Awake()
    {
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.D)) {

        }
    }


    #region GameState

    public void ChangeState(GameState newState)
    {
        Debug.Log("From " + gameState.ToString() + " to " + newState.ToString());
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
                if (CameraManager.Instance != null)
                    CameraManager.Possess(Shelter.instance.cam);
                break;
            case GameState.Explore:
                CameraManager.Possess(Lumberjack.Instance.cam);
                break;
            case GameState.Menu:
                SceneManager.LoadScene(0);
                break;
            case GameState.End:
                Debug.Log("End, VICTORY");
                ChangeState(GameState.Menu);
                break;
            case GameState.GameOver:
                GameUI.instance.GameOver();
                Time.timeScale = 0.0f;
                Lumberjack.Instance.enabled = false;
                break;
        }
    }

    #endregion


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