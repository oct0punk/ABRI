using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileTest : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length == 0) return;
        Vector3 pos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
        pos.z = 0;
        transform.position = pos;
    }
}
