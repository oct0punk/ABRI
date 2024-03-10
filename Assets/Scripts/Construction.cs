using TMPro;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [Header("Construction")]
    public int required;
    [HideInInspector] public bool build = false;
    public bool buildOnStart;
    public GameObject[] taps;
    [SerializeField] protected ParticleSystem fx;

    protected void Awake()
    {
        SetTextAmount();
        if (buildOnStart)
        {
            int temp = required;
            required = 0;
            Build();
            required = temp;
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
            fx.Play();
            AudioManager.Instance.Play("Build");
        }
        else
        {
            Lumberjack.Instance.Message("Faut plus de bois pour réparer ça.");
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


