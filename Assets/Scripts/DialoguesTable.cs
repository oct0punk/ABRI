using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Table
{
    public string name;
    public List<string> engTable; 
    public List<string> frTable;

    public Table(string name, List<string> eng, List<string> fr)
    {
        this.name = name;
        engTable = eng;
        frTable = fr;
    }
}

[CreateAssetMenu(fileName = "Dialogues", menuName = "Dialogues")]
public class DialoguesTable : ScriptableObject
{
    public List<Table> tables = new();
}
