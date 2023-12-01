using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresBubble : MonoBehaviour
{
    [HideInInspector] public RectTransform rectT;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI tmp;

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
    }

    public void Init(CraftMaterials craft)
    {
        icon.sprite = craft.rawMaterial.icon;
        
        if (craft.q > 1)    tmp.text = craft.q.ToString();
        else                tmp.gameObject.SetActive(false);
    }
}
