using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpolateLocalPos : MonoBehaviour
{
    public Vector3 posA;
    public Vector3 posB;
    [Min(1)] public int speed;
    [Range(0.0f, 1.0f)] public float alpha = 0;
    float target;

    public void MoveToA()
    {
        target = 0.0f;
        enabled = true;
    }

    public void MoveToB()
    {
        target = 1.0f;
        enabled = true;
    }

    private void Update()
    {
        alpha += Mathf.Clamp(target, alpha - Time.deltaTime * speed, alpha + Time.deltaTime * speed);
        transform.localPosition = Vector3.Lerp(posA, posB, alpha);
        if (alpha == target) enabled = false;
    }
    
    private void Awake()
    {
        target = alpha;
        Update();
    }
}
