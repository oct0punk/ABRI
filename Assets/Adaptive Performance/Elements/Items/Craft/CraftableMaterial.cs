using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Craftable", menuName = "Craftable")]
public class CraftableMaterial : RawMaterial
{
    public bool craftable { get { return craftMaterials.Length > 0; } }
    public CraftRes[] craftMaterials;
}



[Serializable]
public struct CraftRes
{
    public RawMaterial rawMaterial;
    public int q;

    public CraftRes(RawMaterial rawMaterial, int q)
    {
        this.rawMaterial = rawMaterial;
        this.q = q;
    }
}

