using System.Collections;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public ThinkBubble bubble;
    public Animator fade;

    private void Awake()
    {
        bubble.Message("L'abri est détruit, tout est perdu...", () => true);
    }

    public void Exit()
    {
        StartCoroutine(GameReset());        
    }

    IEnumerator GameReset()
    {
        fade.SetTrigger("Fade");
        yield return new WaitForSeconds(1);
        GameManager.instance.Launch();
    }
}
