using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    public bool isMessaging = false;
    public float coolDown = 0.0f;


    IEnumerator CoolDown()
    {
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
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Instantiate(obj, transform);
        return StartCoroutine(MessageRoutine(condition));
    }


    public IEnumerator MessageRoutine(Func<bool> condition)
    {
        isMessaging = true;
        yield return new WaitWhile(() => condition.Invoke());
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        isMessaging = false;
    }
}
