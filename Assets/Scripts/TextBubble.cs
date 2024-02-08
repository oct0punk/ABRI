using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextBubble : MonoBehaviour
{
    public TextMeshProUGUI text;
    public string content;
    float t = 0.0f;
    int c = 0;
    [Range(0.01f, 0.1f)] public float cooldown = 0.066f;


    private void OnEnable()
    {
        c = 0;
        text.text = "";
    }

    private void Update()
    {
        t -= Time.deltaTime;
        while(t < 0.0f)
        {
            t += cooldown;
            text.text += content[c++];
            if (c >= content.Length)
            {
                enabled = false;
                return;
            }
        }
    }
}
