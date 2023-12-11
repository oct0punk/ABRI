using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapBubble : MonoBehaviour
{
    public GameObject touchTuto;

    private void Awake()
    {
        touchTuto.SetActive(false);
    }
}
