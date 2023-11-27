using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    public AnchorForBridge left;
    public AnchorForBridge right;

    public GameObject fill;
    [Min(.1f)]
    public float l;

    List<HingeJoint2D> joints;

    public void Build(AnchorForBridge left, AnchorForBridge right)
    {
        Building(left, right);
        StartCoroutine(ComputeJoints(joints));
    }

    public void Building(AnchorForBridge left, AnchorForBridge right)
    {
        // Init
        this.left = left;
        this.right = right;
        joints = new List<HingeJoint2D>();
        float dist = Vector3.Distance(this.left.transform.position, this.right.transform.position);
        Vector3 vec = this.right.transform.position - this.left.transform.position;


        // fill between
        for (float t = l / 2; t < dist; t += l)
        {
            Vector3 pos = Vector3.Lerp(left.transform.position, right.transform.position, t / dist);
            Quaternion rot = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
            GameObject go = Instantiate(fill, transform);
            go.transform.SetLocalPositionAndRotation(pos, rot);
            go.name = t.ToString();
            joints.Add(go.GetComponent<HingeJoint2D>());            
        }
        joints.Add(right.GetComponent<HingeJoint2D>());
        
        // Setup joints
        joints[0].connectedBody = left.GetComponent<Rigidbody2D>();
        for (int i = 1; i < joints.Count; i++)
        {
            joints[i].connectedBody = joints[i-1].GetComponent<Rigidbody2D>();            
        }
        

        joints[0].transform.rotation = Quaternion.identity;
        joints[joints.Count - 2].transform.rotation = Quaternion.identity;
    }

    IEnumerator ComputeJoints(List<HingeJoint2D> arr)
    {
        yield return new WaitForSeconds(.1f);
        foreach (var joint in arr)
        {
            joint.autoConfigureConnectedAnchor = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i<joints.Count - 1; i++) {
            Vector2 vec = joints[i+1].transform.position - joints[i].attachedRigidbody.transform.position;
            joints[i].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
        }

    }
}
