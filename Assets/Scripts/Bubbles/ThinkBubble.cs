using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Tmp;
    protected string content;
    protected float coolDown = 0.0f;
    protected Coroutine routine;


    IEnumerator CoolDown()
    {
        while (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public Coroutine Message(string text, float time)
    {
        coolDown = time;
        StartCoroutine(CoolDown());
        return Message(text, () => coolDown > 0.0f);
    }

    public Coroutine Message(string text, Func<bool> condition)
    {
        if (routine != null) StopCoroutine(routine);
        return StartCoroutine(MessageRoutine(text, condition));
    }


    public virtual IEnumerator MessageRoutine(string text, Func<bool> condition)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        content = text;
        text = "";
        for (int c = 0; c < content.Length; c++)
        {
            text += content[c];
            Tmp.text = text;
            yield return new WaitForSeconds(.03f);
        }
        yield return new WaitForSeconds(content.Length * .03f);
        yield return new WaitWhile(() => condition.Invoke());
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
