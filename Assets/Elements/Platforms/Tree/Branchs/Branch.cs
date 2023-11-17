using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    public AnimationCurve curve;
    public Vector2 angle = Vector2.zero;
    public float speed = 1.0f;
    public GameObject rootBone;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float val = Mathf.Lerp(angle.x, angle.y, curve.Evaluate(Time.timeSinceLevelLoad * speed));
        rootBone.transform.localRotation = Quaternion.Euler(0, 0, val);
    }
}
