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
            AudioManager.Instance.Play("Build");
        }
        else
        {
            Lumberjack.Instance.Message("Faut plus de bois pour réparer ça.", 2.0f);
            ItemsManager.Instance.CollectWood(0);
        }
    }

    public virtual void Break()
    {
        foreach (GameObject item in taps)
            item.SetActive(true);
        build = false;
    }


    [ContextMenu("SetTMP")]
    protected void SetTextAmount()
    {
        foreach (GameObject item in taps)
        {
            TextMeshProUGUI tmp = item.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = required.ToString();
        }
    }
}


