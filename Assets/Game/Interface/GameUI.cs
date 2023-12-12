using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public struct bubbleImage
{
    public string name;
    public GameObject prefab;
}

public struct feedbackValue
{
    public RawMaterial mat;
    public bool add;

    public feedbackValue( RawMaterial mat, bool add )
    {
        this.mat = mat;
        this.add = add;
    }
}

public class GameUI : MonoBehaviour
{
    [SerializeField] bubbleImage[] images;

    [Header("Game")]
    [SerializeField] GameObject GamePanel;
    [SerializeField] Image moveLeft;
    [SerializeField] Image moveRight;
    [Space]
    [Header("Inventory")]
    public Inventory inventory;
    public RectTransform collectFeedbackParent;
    [SerializeField] CollectFeedback OnCollectPrefab;
    List<feedbackValue> onCollectValues = new List<feedbackValue>();
    [Space]
    [SerializeField] VerticalLayoutGroup PausePanel;
    [SerializeField] GameObject GameOverPanel;
    [Header("Narration")]
    [SerializeField] Cinematic cineIntro;
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

    public GameObject GetMissBubbleByMat(RawMaterial mat)
    {
        GameObject res = Array.Find(images, im => im.name == "miss").prefab;
        res.GetComponentsInChildren<Image>()[1].sprite = RawMatManager.instance.GetRawMatByName(mat.name).icon;
        res.GetComponentsInChildren<Image>()[1].SetNativeSize();
        return res;
    }



    public void FeedbackOnCollect(RawMaterial mat, bool add)
    {
        onCollectValues.Add(new feedbackValue(mat, add));
        if (onCollectValues.Count == 1)
        {
            StartCoroutine(StorageFeedback());
        }
    }

    IEnumerator StorageFeedback()
    {
        CollectFeedback cfb = Instantiate(OnCollectPrefab, collectFeedbackParent);
        cfb.Init(onCollectValues[0].mat, onCollectValues[0].add);
        yield return new WaitForSeconds(.3f);
        onCollectValues.RemoveAt(0);
        if (onCollectValues.Count > 0)
            StartCoroutine(StorageFeedback());
    }

}
