using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
public class AnchorForBridge : MonoBehaviour
{
    public Transform left;
    public Transform right;
    [Space]
    public Transform leftParent;
    public Transform rightParent;
    [Space]
    public GameObject bridgePrefab;

    public bool isBuilt { get; private set; }

    private void Start()
    {
        if (leftParent != null)
            left.transform.SetParent(leftParent);
        if (rightParent != null)
            right.transform.SetParent(rightParent);
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
