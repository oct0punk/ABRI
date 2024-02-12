using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Instance { get { return GameManager.instance.GetComponent<ItemsManager>(); } }
    public int wood { get;private set; }


    public void CollectWood(int amount)
    {
        wood += amount;
    }
    
    public void Build(Construction construction)
    {
        wood -= construction.required;
    }
}
