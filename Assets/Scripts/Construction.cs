using TMPro;
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
        SetTextAmount();
        if (buildOnStart)
        {
            required = 0;
            Build();
        }
    }

    public virtual void Build()
    {
        if (ItemsManager.Instance.Build(this))
        {
            foreach (GameObject item in taps)
            {
                item.SetActive(false);
            }
            build = true;
        }
        else
        {
            Lumberjack.Instance.Message("Faut plus de bois pour réparer ça.", 2.0f);
        }
    }

    public virtual void Break()
    {
        build = false;
    }


    [ContextMenu("SetTMP")]
    protected void SetTextAmount()
    {
        foreach (GameObject item in taps)
            item.GetComponentInChildren<TextMeshProUGUI>().text = required.ToString();
    }
}


