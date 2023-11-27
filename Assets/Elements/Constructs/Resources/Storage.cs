using System;
using UnityEngine;

[Serializable]
class MatType {
    public string type;
    public int q;
    public int maxQ;
}

public class Storage : MonoBehaviour
{
    [SerializeField] MatType[] content;


    MatType GetMatInContent(string type)
    {
        MatType mat = Array.Find(content, Mat => Mat.type == type);
        if (mat != null)
            return mat;
        else {
            Debug.LogWarning("Mat '" + type + "' not found.");
            return null;
        }
    }


    public void Add(string type, int count = 1)
    {
        MatType mat = GetMatInContent(type);
        if (mat != null)
        {
            Debug.Log("Add " + count + " to ", this);
            mat.q += count;
        }
    }

    // true if you can fill 'count' of 'type'
    public bool CanFill(string type, int count = 1)
    {
        MatType mat = GetMatInContent(type);
        if (mat.maxQ == -1) return true;
        if (mat != null)        
            return mat.q + count <= mat.maxQ;
        
        else        
            return false;
    }

    // return the max you can fill
    public int CountEmpty(string type)
    {
        MatType mat = GetMatInContent(type);
        if (mat == null)
            return -1;
        return mat.maxQ - mat.q;
    }

    // count an element in storage
    public int Count(string type)
    {
        MatType mat = GetMatInContent(type);
        if (mat != null)        
            return mat.q;
        return -1;
        
    }
}
