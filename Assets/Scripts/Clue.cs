using UnityEngine;

public interface IFix
{
    public void Fix(Transform transform, bool paste = false)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, LayerMask.GetMask("Platform"));
        if (hit)
        {
            transform.SetParent(hit.transform);
            if (paste)
            {
                float z = transform.position.z;
                transform.position = new Vector3(hit.point.x, hit.point.y, z);
            }
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
