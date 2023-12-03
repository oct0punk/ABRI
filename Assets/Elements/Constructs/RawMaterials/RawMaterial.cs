using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new rawMat", menuName = "Raw Material")]
public class RawMaterial : ScriptableObject
{
    public int resistance;
    public bool craftable { get { return craftMaterials.Length > 0; } }
    public CraftMaterials[] craftMaterials;
    
    public Sprite icon;
    public GameObject bubbleContent;    
    public bool revivable { get { return timeBeforeRevive != Vector2.zero; } }
    public Vector2 timeBeforeRevive;
}

[Serializable]
public struct CraftMaterials
{
    public RawMaterial rawMaterial;
    public int q;

    public CraftMaterials(RawMaterial rawMaterial, int q)
    {
        this.rawMaterial = rawMaterial;
        this.q = q;
    }
}
