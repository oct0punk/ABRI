using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance { get { return GameManager.instance.GetComponent<ItemsManager>(); } }
    public int wood { get;private set; }
    StorageUI Pack;
    StorageUI pack { get { return Pack != null ? Pack : FindObjectOfType<StorageUI>(); } }

    public void Reset()
    {
        wood = 0;
    }
    public void CollectWood(int amount)
    {
        wood += amount;
        pack.Add(wood);
    }

    public void ConsumeWood(int amount)
    {
        wood -= amount;
        pack.Remove(wood);
    }
    
    public bool Build(Construction construction)
    {
        if (construction.required == 0) return true;
        if (wood >= construction.required)
        {            
            ConsumeWood(construction.required);
            return true;
        }
        else
        {
            pack.Required(wood);
            return false;
        }

    }
}
