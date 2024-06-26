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
        ExitState(gameState);
        gameState = newState;
        EnterState(gameState);
    }

    void ExitState(GameState state) {
        switch (state)
        {
            case GameState.Indoor:
                if (Shelter.instance != null)
                Shelter.instance.OnExit();
                break;
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
                Shelter.instance.OnEnter();
                break;
            case GameState.Menu:
                SceneManager.LoadScene(0);
                Time.timeScale = 1.0f;
                break;
            case GameState.End:
                Time.timeScale = 1.0f;
                foreach (var c in FindObjectsOfType<Construction>()) {
                    foreach (var t in c.taps)
                        if (t.GetComponentInChildren<Canvas>() != null)
                            t.SetActive(false);
                }
                break;
            case GameState.GameOver:
                Time.timeScale = 1.0f;
                SceneManager.LoadScene(3);
                break;
        }
    }

    #endregion


    public void Launch()
    {
        Reset();
        SceneManager.LoadScene(1);
    }

    private void Reset()
    {
        Pickable.doOnce = true;
        Time.timeScale = 1.0f;
        ItemsManager.Instance.Reset();
        Lumberjack.hasCaught = false;
    }

    public void SetPause(bool pause)
    {
        this.pause = pause;
        Time.timeScale = pause ? 0.0f : 1.0f;
        Lumberjack.Instance.OnPause(pause);
        if (pause)
        {
            foreach (var tap in FindObjectsOfType<Tap>())
                tap.gameObject.SetActive(false);
        }
        else {
            if (gameState == GameState.Indoor) Shelter.instance.OnEnter();
            else Shelter.instance.OnExit();
        }
    }

    public void End()
    {
        SceneManager.LoadScene(2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { End(); Debug.Log("ex"); }
    }
}