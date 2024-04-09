using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct SMenuText
{
    public TextMeshProUGUI tmp;
    public string registerName;
}

public class Menu : MonoBehaviour
{
    public ThinkBubble bubble;
    public Toggle tog;
    public SMenuText[] sMenuTexts;
    bool creditRoutine;
    [TextArea]
    public string[] texts;
    [Tooltip("How long is the interval between each text")] public int waitFor;
    

    private void Awake()
    {
        StartCoroutine(Bubble());
        tog.onValueChanged.AddListener(ToggleFunc);
        
    }

    IEnumerator Bubble()
    {
        creditRoutine = false;
        WaitForSeconds wait = new WaitForSeconds(waitFor);
        while (true)
        {
            for (int idx = 0; idx < 3; idx++)
            {
                yield return bubble.MessageRoutine($"Menu{idx}");
                yield return wait;
            }
            
        }
    }

    public void NewGame()
    {
        GameManager.instance.Launch();
    }

    public void ToggleFunc(bool val)
    {
        DialogueManager.Translate(val);
        foreach (var txt in sMenuTexts) {
            txt.tmp.text = DialogueManager.GetString(txt.registerName);
        }
    }

    public void Credits()
    {
        if (creditRoutine) return;
        creditRoutine = true;
        StopAllCoroutines();
        StartCoroutine(CreditRoutine());
    }

    IEnumerator CreditRoutine()
    {
        yield return bubble.Message("Credits", () => Input.touchCount == 0);
        StartCoroutine(Bubble());
    }

    public void Exit()
    {
        Application.Quit();
    }
}
