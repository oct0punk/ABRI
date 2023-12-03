using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new rawMat", menuName = "Raw Material")]
public class RawMaterial : ScriptableObject
{
    public int resistance;
    public CraftMaterials[] craftMaterials;
    public bool craftable { get { return craftMaterials.Length > 0; } }
    public Sprite icon;
    public Vector2 timeBeforeRevive;
    public bool revivable { get { return timeBeforeRevive != Vector2.zero; } }
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
