using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    public float coolDown = 0.0f;
    GameObject content;
    bubbleAnchor anchor;

    private void Awake()
    {
        anchor = GetComponent<bubbleAnchor>();
    }

    IEnumerator CoolDown()
    {
        yield return new WaitUntil(() => anchor.dotsDisplayed);
        while (coolDown > 0)
        {
            coolDown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public Coroutine Message(GameObject obj, float time)
    {
        coolDown = time;
        StartCoroutine(CoolDown());
        return Message(obj, () => coolDown > 0.0f);
    }

    public Coroutine Message(GameObject obj, Func<bool> condition)
    {
        //if (messageRoutine != null) StopCoroutine(messageRoutine);
        if (content != null)
            Destroy(content);
        return StartCoroutine(MessageRoutine(obj, condition));
    }


    public IEnumerator MessageRoutine(GameObject obj, Func<bool> condition)
    {
        anchor.enabled = true;
        yield return anchor.DisplayDots();
        content = Instantiate(obj, transform);
        yield return new WaitWhile(() => condition.Invoke());
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        GetComponent<bubbleAnchor>().enabled = false;
    }
}
