using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthographicHandler : MonoBehaviour
{
    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localScale = Vector3.one * cam.orthographicSize;
    }
}
