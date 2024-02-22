using UnityEngine;

public class Clue : MonoBehaviour
{
    public void SendClueToLumberjack(string str)
    {
        Lumberjack.Instance.Message(str);
    }
}
