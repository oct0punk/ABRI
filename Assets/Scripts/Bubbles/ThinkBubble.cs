using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

struct MessageStruct
{
    public string text;
    public float time;
    public Action action;

    public MessageStruct(string text, float time, Action action)
    {
        this.text = text;
        this.time = time;
        this.action = action;
    }
}

public class ThinkBubble : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI Tmp;
    protected string content;
    public bool isRunning { get; private set; }
    readonly LinkedList<MessageStruct> standby = new();

    [SerializeField] protected float timeBtw = 0.02f;
    [SerializeField] protected float timeEnd = 1.3f;



    LinkedListNode<MessageStruct> FindNode(LinkedList<MessageStruct> list, Predicate<MessageStruct> match)
    {        
        if (list.Count == 0) return null;
        LinkedListNode<MessageStruct> node = list.First;
        while (!match(node.Value)) {
            if (node == list.Last) return null;
            node = node.Next;
        }
        return node;
    }

    public void Message(string text, float time, Action action = null, bool priority = false)
    {
        if (FindNode(standby, m => m.text == text) != null) return;
        if (isRunning)
        {
            if (content != text)
            {
                if (priority)
                    standby.AddFirst(new MessageStruct(text, time, action));
                else
                    standby.AddLast(new MessageStruct(text, time, action));
            }
        }    
        else
            StartCoroutine(MessageRoutine(text, time, action));
    }
    public IEnumerator MessageRoutine(string text, float time, Action action = null)
    {
        isRunning = true;
        action?.Invoke();
        yield return new WaitForSeconds(time);
        AudioManager.Instance.Play("Speak");
        transform.GetChild(0).gameObject.SetActive(true);
        content = text;
        text = "";
        for (int c = 0; c < content.Length; c++)
        {
            text += content[c];
            Tmp.text = text;
            yield return new WaitForSeconds(timeBtw);
        }
        yield return new WaitForSeconds(timeEnd);
        transform.GetChild(0).gameObject.SetActive(false);
        
        if (standby.Count > 0)
        {
            MessageStruct value = standby.First.Value;
            StartCoroutine(MessageRoutine(value.text, value.time, value.action));
            standby.RemoveFirst();
        }
        else
        {
            isRunning = false;
        }
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
            yield return new WaitForSeconds(timeBtw);
        }
        yield return new WaitWhile(() => condition.Invoke());
        transform.GetChild(0).gameObject.SetActive(false);
        isRunning = false;
    }
}
