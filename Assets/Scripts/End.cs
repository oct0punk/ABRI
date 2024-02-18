using UnityEngine;

public class End : MonoBehaviour
{
    public void Skip()
    {
        GameManager.instance.ChangeState(GameState.Menu);
    }
}
