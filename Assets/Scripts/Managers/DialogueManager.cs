using System;
using System.IO;
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
            string[] rows = line.Split("¤ ");
            if (rows[0] == name)
            {
                string[] sentences = rows[1 + instance.locale].Split('*');
                return sentences[UnityEngine.Random.Range(0, sentences.Length)];
            }
        }
        Debug.LogWarning(name + " not found");
        return string.Empty;
    }
}
