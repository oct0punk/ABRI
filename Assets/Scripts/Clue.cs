using UnityEngine;

public interface IFix
{
    public void Fix(UnityEngine.Transform transform, bool paste = false)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);
            if (paste)
                transform.position = hit.point;
        }
    }
}

public class Clue : MonoBehaviour, IFix
{

    private void Awake()
    {
        IFix fix = this;
        fix.Fix(transform);
    }

    public void SendClueToLumberjack(string str)
    {
        Lumberjack.Instance.Message(str);
    }
}
