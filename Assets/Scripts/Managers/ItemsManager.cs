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
        pack.Display(wood, 1.0f);
    }
    
    public bool Build(Construction construction)
    {
        if (wood >= construction.required)
        {            
            CollectWood(-construction.required);
            return true;
        }
        else
        {
            pack.Required(construction.required);
            return false;
        }

    }
}
