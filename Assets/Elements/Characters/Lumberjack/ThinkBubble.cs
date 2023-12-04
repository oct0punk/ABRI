using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThinkBubble : MonoBehaviour
{
    public bool isMessaging = false;
    public float coolDown = 0.0f;
    private Coroutine messageRoutine;


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
        Debug.Log("Message : " + obj.name);
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        isMessaging = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Instantiate(obj, transform);
        return messageRoutine = StartCoroutine(MessageRoutine(condition));
    }


    public IEnumerator MessageRoutine(Func<bool> condition)
    {
        Debug.Log(coolDown, this);
        yield return new WaitWhile(() => condition.Invoke());
        Debug.Log(coolDown, this);

        Debug.Log("EndMessage");
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        isMessaging = false;
    }
}
