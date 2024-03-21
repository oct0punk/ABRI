using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Bridge : Construction, IFix
{
    [Header("Bridge")]
    public Transform left;
    public Transform right;


    public GameObject fill;
    [Min(.1f)] public float distBetweenEachPlanchs;

    List<HingeJoint2D> joints;

    new void Awake()
    {
        IFix fix = this;
        fix.Fix(left.transform, true);
        fix.Fix(right.transform, true);

        base.Awake();
    }

    public override void Build()
    {
        base.Build();
        if (!build) return;
        Build(left, right);
        enabled = true;
    }

    void Build(Transform left, Transform right)
    {
        // Init
        this.left = left;
        this.right = right;

        Vector2 leftToRight = left.position - right.position;
        buildFX.transform.position = (left.position + right.position) / 2;
        buildFX.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(leftToRight.y, leftToRight.x) * Mathf.Rad2Deg);
        var sh = buildFX.shape;
        sh.scale = new Vector3(Vector3.Distance(left.position, right.position) + 1, 3, 1);

        float dist = Vector3.Distance(this.left.transform.position, this.right.transform.position);
        Vector3 vec = this.right.transform.position - this.left.transform.position;

        joints = new List<HingeJoint2D>();
        // Fill
        for (float t = distBetweenEachPlanchs / 2; t < dist; t += distBetweenEachPlanchs)
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
        joints[0].connectedAnchor = new Vector2(fill.transform.localScale.x / 2, 0);
        for (int i = 1; i < joints.Count; i++)
        {
            joints[i].connectedBody = joints[i - 1].GetComponent<Rigidbody2D>();
        }
        joints[0].transform.rotation = Quaternion.identity;
        joints[joints.Count - 2].transform.rotation = Quaternion.identity;
    }



    private void Update()
    {
        for (int i = 0; i<joints.Count - 1; i++) {
            Vector2 vec = joints[i+1].transform.position - joints[i].attachedRigidbody.transform.position;
            joints[i].transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
        }

    }

}

    //public void Preview(Transform left, Transform right)
    //{
    //    enabled = false;
    //    float dist = Vector3.Distance(left.transform.position, right.transform.position);
    //    Vector3 vec = right.transform.position - left.transform.position;

    //    for (float t = distBetweenEachPlanchs / 2; t < dist; t += distBetweenEachPlanchs)
    //    {
    //        Vector3 pos = Vector3.Lerp(left.transform.position, right.transform.position, t / dist);
    //        Quaternion rot = Quaternion.Euler(0, 0, Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
    //        GameObject go = Instantiate(fill, transform);
    //        go.transform.SetLocalPositionAndRotation(pos, rot);
    //        go.name = t.ToString();
    //        go.GetComponent<Rigidbody2D>().simulated = false;
    //        go.GetComponent<HingeJoint2D>().enabled = false;
    //        go.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .4f);
    //    }
    //}