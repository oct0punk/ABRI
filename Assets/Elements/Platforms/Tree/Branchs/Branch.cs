using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour
{
    [Space]
    public AnimationCurve curve;
    public float speed = 1.0f;
    [Space]
    public GameObject IKtarget;
    public GameObject targetPole1;
    public GameObject targetPole2;

    // Update is called once per frame
    void Update()
    {
        Vector3 val = Vector3.Lerp(targetPole1.transform.position, targetPole2.transform.position, curve.Evaluate(Time.timeSinceLevelLoad * speed));
        IKtarget.transform.position = val;
    }
}
