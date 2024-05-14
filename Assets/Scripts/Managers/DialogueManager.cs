using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    static DialogueManager instance;
    [SerializeField] DialoguesTable _dialoguesTable;
    static DialoguesTable _DialoguesTable;
    static bool localeIsFrench = false;

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
                return;
            }
        }

        _DialoguesTable = _dialoguesTable;
        if (!PlayerPrefs.HasKey("Locale")) PlayerPrefs.SetInt("Locale", 0);
        Translate(PlayerPrefs.GetInt("Locale") == 1);
    }

    //[ContextMenu("CreateDialogues")]
    //void CreateDialogueFile()
    //{
    //    var fileData = File.ReadAllText(Application.dataPath + "/Prefabs/dialogues.csv");
    //    var lines = fileData.Split('\n');
    //    List<Table> objContent = new();
    //    foreach (var line in lines)
    //    {
    //        string[] rows = line.Split("¤ ");
    //        string[] engTab = rows[1].Split("*");
    //        string[] frTab = rows[2].Split("*");
    //        objContent.Add(new Table (rows[0], engTab.ToList(), frTab.ToList()));
    //    }
    //    DialoguesTable dTab = ScriptableObject.CreateInstance<DialoguesTable>();
    //    dTab.tables = objContent;
    //    AssetDatabase.CreateAsset(dTab, $"Assets/Prefabs/Dialogues/dialoguesTable.asset");
    //    AssetDatabase.SaveAssets();
    //}

    public static void Translate(bool french)
    {        
        PlayerPrefs.SetInt("Locale", french ? 1 : 0);
        localeIsFrench = !french;
    }

    public static string GetString(string name)
    {
        Table tab = _DialoguesTable.tables.Find(t => t.name == name);
        if (localeIsFrench) return tab.frTable[UnityEngine.Random.Range(0, tab.frTable.Count)];
        else                return tab.engTable[UnityEngine.Random.Range(0, tab.frTable.Count)];
    }
}
