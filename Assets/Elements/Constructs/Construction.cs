using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
    public CraftMaterials[] materials;

    public virtual void Build(Lumberjack lum)
    {
        if (!lum.storage.CanCraft(materials))
        {

            return;
        }
    }
}
