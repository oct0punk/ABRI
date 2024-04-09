using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Range(0, 1)] public int locale;
    static DialogueManager instance;
    static Dictionary<string, string> register = new();
    PlayerPrefs prefs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    public static void Translate(bool french)
    {
        PlayerPrefs.SetInt("Locale", french ? 1 : 0);
        Debug.Log(PlayerPrefs.GetInt("Locale"));
        register.Clear();
        var fileData = File.ReadAllText(Application.dataPath + "/Prefabs/dialogues.csv");
        var lines = fileData.Split('\n');
        foreach (var line in lines)
        {
            string[] rows = line.Split("¤ ");
            register.Add(rows[0], french ? rows[2] : rows[1]);
        }
    }

    public static string GetString(string name)
    {
        if (register == null) Translate(PlayerPrefs.GetInt("Locale") == 1);
        if (register.ContainsKey(name))
        {
            string[] sentences = register[name].Split('*');
            return sentences[UnityEngine.Random.Range(0, sentences.Length)];
        }
        else
        {
            Debug.LogWarning(name + " not found");
            return string.Empty;
        }
    }
}
