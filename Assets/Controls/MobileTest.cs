using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream:Assets/MobileTest.cs
        if (Input.touches.Length == 0) return;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
=======
        if (Input.touches.Length <= index) return;
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(index).position);
>>>>>>> Stashed changes:Assets/Controls/MobileTest.cs
        pos.z = 0;
        transform.position = pos;
    }
}
