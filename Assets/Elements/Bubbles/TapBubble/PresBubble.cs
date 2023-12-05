using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresBubble : MonoBehaviour
{
    [HideInInspector] public RectTransform rectT;
    [HideInInspector] public Image im;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI tmp;
    public CraftMaterials cMat { get; private set; }

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
        im = GetComponent<Image>();
    }

    public void Init(CraftMaterials craft)
    {
        cMat = craft;
        icon.sprite = cMat.rawMaterial.icon;
        
        if (cMat.q > 1)    tmp.text = cMat.q.ToString();
        else                tmp.gameObject.SetActive(false);

        UpdateMat();
    }
    public void UpdateMat()
    {
        int count = GameManager.instance.lumberjack.storage.Count(cMat.rawMaterial);
        if (count >= cMat.q)
            im.color = Color.green;
        else if (count > 0)
            im.color = Color.yellow;
        else
            im.color = Color.red;
    }    
    public void Hide()
    {
        Destroy(gameObject);
    }
}
