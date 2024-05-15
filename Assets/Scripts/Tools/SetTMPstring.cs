using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SetTMPstring : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI tmp;
    public string string_name;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        tmp.SetText(DialogueManager.GetString(string_name));
    }
}
