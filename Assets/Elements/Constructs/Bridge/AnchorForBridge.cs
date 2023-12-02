using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class AnchorForBridge : MonoBehaviour
{
    public bool buildOnStart;

    public Transform left;
    public Transform right;
    //[Space]
    //public Transform leftParent;
    //public Transform rightParent;
    [Space]
    public GameObject bridgePrefab;

    public bool isBuilt { get; private set; }

    private void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(left.transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Platform"));
        if (hit)
        {
            left.transform.SetParent(hit.transform);
        }
        hit = Physics2D.Raycast(right.transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Platform"));
        if (hit)
        {
            right.transform.SetParent(hit.transform);
        }

        if (buildOnStart)
            Build();
    }

    public void Build()
    {
        isBuilt = true;

        Bridge bridge = Instantiate(bridgePrefab).GetComponent<Bridge>();
        bridge.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        
        bridge.Build(left, right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            Tuto.canBuild = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Lumberjack>() != null)
        {
            Tuto.canBuild = false;
        }
    }
}
