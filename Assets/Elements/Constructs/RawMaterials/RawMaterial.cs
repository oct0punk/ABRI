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
}

[Serializable]
public struct CraftMaterials
{
    public RawMaterial rawMaterial;
    public int q;
}
