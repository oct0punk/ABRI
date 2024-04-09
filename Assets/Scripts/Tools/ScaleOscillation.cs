using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOscillation : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = -0.1f;

    RectTransform rectTransform;
    Vector3 scale;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        scale = rectTransform.localScale;
    }

    private void Update()
    {
        rectTransform.localScale = scale + Vector3.one * Mathf.Sin(Time.timeSinceLevelLoad * speed) * amplitude;
    }
}
