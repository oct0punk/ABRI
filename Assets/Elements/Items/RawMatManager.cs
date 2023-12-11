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
        RawMaterial res = System.Array.Find(rawMaterials, rawMat => rawMat.name == name);
        if (res == null)
        {
            Debug.LogWarning("RawMat '" + name + " ' not set");
            return null;
        }
        return res;
    }
}
