using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] Image moveLeft;
    [SerializeField] Image moveRight;
    
    [Header("Panels")]
    [SerializeField] GameObject GamePanel;
    [SerializeField] VerticalLayoutGroup PausePanel;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject EndPanel;


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
    }
    #endregion


    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Reload()
    {
        SceneManager.LoadScene(1);
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
