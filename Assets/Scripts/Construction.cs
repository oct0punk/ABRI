using TMPro;
using UnityEngine;

public class Construction : MonoBehaviour
{
    [Header("Construction")]
    public int required;
    [HideInInspector] public bool build = false;
    public bool buildOnStart;
    public GameObject[] taps;
    [SerializeField] protected ParticleSystem buildFX;

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

    public virtual void BuildWithFX()
    {
        if (ItemsManager.Instance.Build(this))
        {
            buildFX.Play();
            AudioManager.Instance.Play("BeginBuild");
            Invoke(nameof(Build), buildFX.main.duration);

        }
        else
        {
            Lumberjack.Instance.Message("NeedMoreWood");
        }
    }
    public virtual void Build()
    {
        foreach (GameObject item in taps)
        {
            item.SetActive(false);
        }
        build = true;
        AudioManager.Instance.Play("Build");
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


