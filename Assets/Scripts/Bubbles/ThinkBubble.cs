using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Tmp;
    protected string content;
    protected float coolDown = 0.0f;
    protected Coroutine routine;

    private void Update()
    {
        coolDown -= Time.deltaTime;
        if (coolDown < 0.0f)
            enabled = false;
    }

    public Coroutine Message(string text, float time)
    {
        coolDown = time;
        return Message(text, () => coolDown > 0.0f);
    }

    public Coroutine Message(string text, Func<bool> condition)
    {
        if (routine != null) StopCoroutine(routine);
        return StartCoroutine(MessageRoutine(text, condition));
    }


    public IEnumerator MessageRoutine(string text, Func<bool> condition)
    {
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
        enabled = true;
        yield return new WaitWhile(() => condition.Invoke());
        enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
