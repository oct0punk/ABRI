using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tree : MonoBehaviour
{
    [Range(.0f, 1.0f)] public float oscillationSpeed;
    public Vector2 angle;

    private void Update()
    {
        float zRot = Mathf.Lerp(angle.x, angle.y, Mathf.Sin(transform.position.x + Time.timeSinceLevelLoad * oscillationSpeed) / 2 + .5f);
        transform.rotation = Quaternion.Euler(0, 0, zRot);
    }
    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
