using System;
using System.IO;
using UnityEditor.SceneTemplate;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Range(0, 1)] public int locale;
    static DialogueManager instance;

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

    public static string GetString(string name)
    {
        var fileData = File.ReadAllText(Application.dataPath + "/Prefabs/dialogues.csv");
        var lines = fileData.Split('\n');
        foreach (var line in lines)
        {
            Debug.Log(line);
            string[] rows = line.Split("¤ ");
            foreach (var row in rows)
            {
                Debug.Log(row);
            }
            if (rows[0] == name)
            {
                Debug.Log("return " + rows[1 + instance.locale]);
                return rows[1 + instance.locale];
            }
        }
        Debug.LogWarning(name + " not found");
        return string.Empty;
    }
}
