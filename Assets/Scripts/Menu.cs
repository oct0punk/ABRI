using System.Collections;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public ThinkBubble bubble;
    public string[] texts;
    [Tooltip("How long is the interval between each text")] public int waitFor;
    int lastIndex = -1;
    

    private void Awake()
    {
        StartCoroutine(Bubble());
    }

    IEnumerator Bubble()
    {
        WaitForSeconds wait = new WaitForSeconds(waitFor);
        while (true)
        {
            int idx;
            do { idx = Random.Range(0, texts.Length); } while (idx == lastIndex);
            lastIndex = idx;
            yield return bubble.MessageRoutine(texts[idx], 0);
            yield return wait;
        }
    }

    public void NewGame()
    {
        GameManager.instance.Launch();
    }

    public void Options()
    {

    }

    public void Credits()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
