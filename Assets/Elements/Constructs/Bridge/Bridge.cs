using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] Transform left;
    [SerializeField] Transform right;

    public Transform tLeft;
    public Transform tRight;

    private void Start()
    {
        Build(tLeft, tRight);
    }

    public void Build(Transform rightParent, Transform leftParent)
    {
        left.SetParent(leftParent);
        left.localPosition = Vector3.zero;
        right.SetParent(rightParent);
        right.localPosition = Vector3.zero;

        EdgeCollider2D[] edges = GetComponentsInChildren<EdgeCollider2D>();        
        float distance  = Vector3.Distance(left.position, right.position) + 1;
        distance = distance / edges.Length;
        foreach (EdgeCollider2D edge in edges)
        {
            edge.points = new Vector2[2] { new Vector2(-distance / 2, 0), new Vector2(distance / 2, 0)};
            Debug.Log(edge.points[0]);
            Debug.Log(edge.points[1]);
            Debug.Log("------------");
        }
    }
}
