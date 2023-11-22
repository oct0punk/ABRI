using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        else
        {
            Debug.Log("Mat " + type + " not found.");
            return null;
        }
    }

    public void Add(string type, int count = 1)
    {
        MatType mat = GetMatInContent(type);
        if (mat != null)
        {
            mat.q += count;
        }
        else
            Debug.LogWarning(type + " not found");
    }

    public bool CanFill(string type, int count = 1)
    {
        MatType mat = GetMatInContent(type);
        if (mat.maxQ == -1) return true;
        if (mat != null)        
            return mat.q + count <= mat.maxQ;
        
        else        
            return false;
    }

    public int Count(string type)
    {
        MatType mat = GetMatInContent(type);
        if (mat != null)        
            return mat.q;
        return -1;
        
    }
}
