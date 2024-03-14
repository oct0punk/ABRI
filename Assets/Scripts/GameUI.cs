using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    [Header("Inputs")]
    [SerializeField] Image moveLeft;
    [SerializeField] Image moveRight;

    [Header("Panels")]
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject PausePanel;
    [SerializeField] GameObject EndPanel;
    Canvas[] UI_WorldArray;
    [Space]
    public Image fade;
    [SerializeField][Min(1)] int fadeSpeed;

    private void Awake()
    {
        instance = this;
        UI_WorldArray = Array.FindAll(FindObjectsOfType<Canvas>(), can => can.renderMode == RenderMode.WorldSpace);
    }


    #region Layers
    public void Game()
    {
        NoHUD(); GamePanel.gameObject.SetActive(true);
        GameManager.instance.SetPause(false);

        foreach (Canvas canvas in UI_WorldArray)
        {
            canvas.gameObject.SetActive(true);
        }
    }
    public void Pause()
    {
        NoHUD(); PausePanel.SetActive(true);
        GameManager.instance.SetPause(true);

        foreach (Canvas canvas in UI_WorldArray)
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                canvas.gameObject.SetActive(false);
            }
        }
    }
    public void NoHUD()
    {
        GamePanel.SetActive(false);
        PausePanel.SetActive(false);
        EndPanel.SetActive(false);
    }
    #endregion


    public void BackToMenu()
    {
        GameManager.instance.ChangeState(GameState.Menu);
    }
    public void Reload()
    {
        GameManager.instance.Launch();
        Time.timeScale = 1.0f;
        Game();
    }


    #region Inputs
    public void MoveLeft()
    {
        moveRight.color = Color.clear;
        moveLeft.color = new Color(1, 1, 1, .1f);
    }
    public void MoveRight()
    {
        moveLeft.color = Color.clear;
        moveRight.color = new Color(1, 1, 1, .1f);
    }
    public void NoMove()
    {
        moveLeft.color = moveRight.color = Color.clear;
    }
    public void BothMove()
    {
        moveLeft.color = moveRight.color = new Color(1, 1, 1, .1f);
    }
    #endregion
}
