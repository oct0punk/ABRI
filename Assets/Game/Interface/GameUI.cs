using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct bubbleImage
{
    public string name;
    public GameObject prefab;
}

public class GameUI : MonoBehaviour
{
    public static GameUI instance { get { return GameManager.instance.ui; } }
    [SerializeField] bubbleImage[] images;

    [Header("Game")]
    [SerializeField] GameObject GamePanel;
    [SerializeField] Image moveLeft;
    [SerializeField] Image moveRight;
    public Inventory inventory;
    [Space]
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject EndPanel;
    [SerializeField] Cinematic cineIntro;
    [SerializeField] VerticalLayoutGroup PausePanel;
    [SerializeField] CraftFeedback craftPrefab;


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

    public GameObject GetBubbleContentByName(string name)
    {
        GameObject res = Array.Find(images, im => im.name == name).prefab;
        if (res == null)
            Debug.LogWarning(name + " not found", this);
        return res;
    }
    public void InstantiateCraftFeedback(CraftBubble craftBubble)
    {
        CraftFeedback craftFeedback = Instantiate(craftPrefab, transform as RectTransform);
        //craftFeedback.target = 
    }
}
