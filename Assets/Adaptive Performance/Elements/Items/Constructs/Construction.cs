using UnityEngine;

public class Construction : MonoBehaviour
{
    [Header("Construction")]
    public int required;
    [HideInInspector] public bool build = false;
    public bool buildOnStart;
    [SerializeField] GameObject[] taps;

    protected void Awake()
    {
        if (buildOnStart)
        {
            required = 0;
            Build();
        }
    }

    public virtual void Build()
    {
        build = true;
        ItemsManager.Instance.Build(this);
        foreach (GameObject item in taps)
        {
            item.SetActive(false);
        }
    }
}


