using System.Collections;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public ThinkBubble bubble;
    public Animator fade;

    private void Awake()
    {
        bubble.Message("L'abri est détruit. Je n'ai nulle part pour abriter l'oiseau... \r\nC foutu", () => true);
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
