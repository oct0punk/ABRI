using System;
using TMPro;
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
    Canvas[] UI_WorldArray;
    [Space]
    [SerializeField] TextMeshProUGUI woodCount;
    [SerializeField] SetTMPstring tutoText;
    public string currentTutoText { get; private set; } = "";
    [Space]
    [SerializeField] Toggle tog;
    public SMenuText[] sMenuTexts;

    private void Awake()
    {
        instance = this;
        UI_WorldArray = Array.FindAll(FindObjectsOfType<Canvas>(), can => can.renderMode == RenderMode.WorldSpace);
        tog.isOn = PlayerPrefs.GetInt("Locale") == 1;
        tog.onValueChanged.AddListener(ToggleFunc);
        Translate();
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
        // CameraManager.Instance.Blur(false);
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

        woodCount.text = ItemsManager.Instance.wood.ToString();
        // CameraManager.Instance.Blur(true);
    }
    public void NoHUD()
    {
        GamePanel.SetActive(false);
        PausePanel.SetActive(false);
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

    public void ToggleFunc(bool val)
    {
        DialogueManager.Translate(val);
        Translate();
    }
    void Translate()
    {
        foreach (var txt in sMenuTexts)
        {
            txt.tmp.text = DialogueManager.GetString(txt.registerName);
        }
    }

    public static void TutoText(string text)
    {
        instance.currentTutoText = text;
        instance.tutoText.transform.parent.gameObject.SetActive(true);
        instance.tutoText.tmp.SetText(DialogueManager.GetString(text));
    }

    public static void DisableTutoText()
    {
        instance.currentTutoText = "";
        instance.tutoText.transform.parent.gameObject.SetActive(false);
        instance.tutoText.tmp.SetText("");
    }
}
