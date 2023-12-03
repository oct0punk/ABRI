using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    [SerializeField] GameObject GamePanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject EndPanel;
    [SerializeField] Cinematic cineIntro;
    [SerializeField] VerticalLayoutGroup PausePanel;

    private void Awake()
    {
        instance = this;        
    }

    #region Layers
    public void Game()
    {
        NoHUD();
        GamePanel.gameObject.SetActive(true);
    }
    public void Pause()
    {
        NoHUD();
        PausePanel.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        NoHUD();
        GameOverPanel.SetActive(true);
    }
    public void NoHUD()
    {
        GamePanel.SetActive(false);
        PausePanel.gameObject.SetActive(false);
        EndPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        cineIntro.gameObject.SetActive(false);
    }
    #endregion


    public void Intro()
    {
        NoHUD();
        cineIntro.gameObject.SetActive(true);
        cineIntro.Play();
    }
    public void Outro()
    {
        NoHUD();
        EndPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Reload()
    {
        SceneManager.LoadScene(1);
    }
}
