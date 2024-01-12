using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

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
    public Volume volume;
    public Tuto tuto { get; private set; }
    public bool pause { get; private set; }

    private void Awake()
    {
        instance = this;
        tuto = GetComponent<Tuto>();
        lumberjack = FindObjectOfType<Lumberjack>();
        shelter = FindObjectOfType<Shelter>();
        ui.inventory.gameObject.SetActive(false);
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
            case GameState.Intro:
                Time.timeScale = 1.0f;
                ui.Game();                
                break;

            case GameState.Craft:
                lumberjack.enabled = true;
                lumberjack.ChangeFSM(lumberjack.idleState);
                shelter.workbench.HidePlans();
                ui.BothMove();
                ui.inventory.gameObject.SetActive(false);
                CameraManager.Possess(shelter.cam);
                break;

            case GameState.Build:   
                ui.inventory.gameObject.SetActive(false);
                ui.BothMove();
                lumberjack.enabled = true;
                shelter.workbench.canvas.gameObject.SetActive(true);
                break;

            case GameState.Indoor:  break;
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
                AudioManager.Instance.windFade.target = 0.0f;
                break;

            case GameState.Craft:
                CameraManager.Possess(shelter.workbench.cam);
                lumberjack.AutoMoveTo(shelter.workbench.transform.position, () => { lumberjack.CraftMode(shelter.workbench); });
                break;
            
            case GameState.Build:
                lumberjack.enabled = false;
                shelter.workbench.canvas.gameObject.SetActive(false);
                // ui.inventory.gameObject.SetActive(true);
                // Hide ...
                // Display plans
                break;
            
            
            case GameState.Explore:
                lumberjack.indoor = false;
                CameraManager.Possess(lumberjack.cam);
                AudioManager.Instance.windFade.target = 1.0f;
                break;

            case GameState.Outro: break;
        }
    }


    public void Resume()
    {
        if (gameState != GameState.Build &&  gameState != GameState.Craft)
            ui.inventory.gameObject.SetActive(false);
        pause = false;
        Time.timeScale = 1.0f;
        ui.Game();
        lumberjack.enabled = true;
    }

    public void Pause()
    {
        pause = true;
        lumberjack.enabled = false;
        ui.inventory.gameObject.SetActive(true);
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
