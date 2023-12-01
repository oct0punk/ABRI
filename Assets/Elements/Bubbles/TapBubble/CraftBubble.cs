using UnityEngine;

public class CraftBubble : ConsumeBubble
{
    public RawMaterial material;

    private void Start()
    {
        materials = material.craftMaterials;
        action = () => FindObjectOfType<Lumberjack>().storage.Add(material);
    }
}
