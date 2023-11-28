using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MobileTest : MonoBehaviour
{
    public int index;



    // Update is called once per frame
    void Update()
    {

        if (Input.touches.Length <= index) return;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(index).position);
        pos.z = 0;
        transform.position = pos;
    }
}
