using System;
using UnityEngine;

[Serializable]
public struct DictionaryRawmatBubble
{
    public string name;
    public GameObject bubble;
}

public class RawMatManager : MonoBehaviour
{
    public static RawMatManager instance;
    public RawMaterial[] rawMaterials;
    public DictionaryRawmatBubble[] rawBubbles;
   Workbench workbench { get { return GameManager.instance.shelter.workbench; } }

    private void Awake()
    {
        instance = this;
    }

    public RawMaterial GetRawMatByName(string name)
    {
        RawMaterial res = System.Array.Find(rawMaterials, rawMat => rawMat.name == name);
        if (res == null)
        {
            Debug.LogWarning("RawMat '" + name + " ' not set");
            return null;
        }
        return res;
    }

    public void AddBubbleToWorkbench(string matStr)
    {
        Array.Find(rawBubbles, mat => mat.name == matStr).bubble.SetActive(true);
    }
}
