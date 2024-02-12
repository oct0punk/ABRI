using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum GameState
{
    Explore,
    Indoor,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState gameState;

    [HideInInspector] public Lumberjack lumberjack;
    [HideInInspector] public Shelter shelter;
    public GameUI ui;
    public Volume volume;
    public bool pause { get; private set; }

    private void Awake()
    {
        instance = this;
        lumberjack = FindObjectOfType<Lumberjack>();
        shelter = FindObjectOfType<Shelter>();
        DepthOfField dof;
        volume.profile.TryGet<DepthOfField>(out dof);
        if (dof != null)
            dof.focalLength.Override(Screen.dpi / 4);
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
            case GameState.Indoor:  break;
            case GameState.Explore: break;
        }
    }

    void EnterState(GameState state) {
        switch (state)
        {
            case GameState.Indoor:
                lumberjack.indoor = true;
                CameraManager.Possess(shelter.cam);
                // AudioManager.Instance.GetFadeByName("Wind").target = 0.0f;
                break;
            case GameState.Explore:
                lumberjack.indoor = false;
                CameraManager.Possess(lumberjack.cam);
                // AudioManager.Instance.GetFadeByName("Wind").target = 1.0f;
                break;
        }
    }


    public void Resume()
    {
        pause = false;
        Time.timeScale = 1.0f;
        ui.Game();
        lumberjack.enabled = true;
    }

    public void Pause()
    {
        pause = true;
        lumberjack.enabled = false;
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
    }
}
