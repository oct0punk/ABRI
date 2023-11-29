using UnityEngine;

public class RawMatManager : MonoBehaviour
{
    public static RawMatManager instance;
    public RawMaterial[] rawMaterials;

    private void Awake()
    {
        instance = this;
    }

    public RawMaterial GetRawMatByName(string name)
    {
        return System.Array.Find(rawMaterials, rawMat => rawMat.name == name);
    }
}
