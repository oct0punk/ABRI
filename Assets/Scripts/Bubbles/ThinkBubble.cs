using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Tmp;
    protected string content;
    bool isRunning;
    LinkedList<string> standby = new LinkedList<string>();


    public void Message(string text, float time)
    {
        if (standby.Contains(text)) return;
        if (isRunning)
        {
            if (content != text)
                standby.AddLast(text);        
        }
        else
            StartCoroutine(MessageRoutine(text, time));
    }
    public Coroutine Message(string text, Func<bool> condition)
    {
        return StartCoroutine(MessageRoutine(text, condition));
    }

    public IEnumerator MessageRoutine(string text, Func<bool> condition)
    {
        isRunning = true;
        AudioManager.Instance.Play("Speak");
        transform.GetChild(0).gameObject.SetActive(true);
        content = text;
        text = "";
        for (int c = 0; c < content.Length; c++)
        {
            text += content[c];
            Tmp.text = text;
            yield return new WaitForSeconds(.03f);
        }
        yield return new WaitWhile(() => condition.Invoke());
        transform.GetChild(0).gameObject.SetActive(false);
        isRunning = false;
    }
    public IEnumerator MessageRoutine(string text, float time)
    {
        isRunning = true;
        AudioManager.Instance.Play("Speak");
        transform.GetChild(0).gameObject.SetActive(true);
        content = text;
        text = "";
        for (int c = 0; c < content.Length; c++)
        {
            text += content[c];
            Tmp.text = text;
            yield return new WaitForSeconds(.03f);
        }
        yield return new WaitForSeconds(time);
        transform.GetChild(0).gameObject.SetActive(false);
        
        if (standby.Count > 0)
        {
            StartCoroutine(MessageRoutine(standby.First.Value, 2.0f));
            standby.RemoveFirst();
        }
        else
        {
            isRunning = false;
        }
    }
}
