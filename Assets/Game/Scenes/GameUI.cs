using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;
    public GameObject GamePanel;
    public GameObject GameOverPanel;
    public GameObject EndPanel;
    public VerticalLayoutGroup PausePanel;

    private void Start()
    {
        instance = this;
        GameOverPanel.SetActive(false);
        EndPanel.SetActive(false);
        Resume();
        
    }

    public void Pause()
    {
        GamePanel.SetActive(false);
        PausePanel.gameObject.SetActive(true);
    }

    public void Resume()
    {
        GamePanel.gameObject.SetActive(true);
        PausePanel.gameObject.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void End()
    {
        GamePanel.SetActive(false);
        EndPanel.SetActive(true);
    }

    public void GameOver()
    {
        GamePanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(1);
    }
}
