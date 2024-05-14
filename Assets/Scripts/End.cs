using UnityEngine;

public class End : MonoBehaviour
{
    [SerializeField] ThinkBubble bubble;
    private void Awake()
    {
        bubble.Message(DialogueManager.GetString("OnBackHome"));
    }
    public void Skip()
    {
        GameManager.instance.ChangeState(GameState.Menu);
    }
}
