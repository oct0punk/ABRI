using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresBubble : MonoBehaviour
{
    [HideInInspector] public RectTransform rectT;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI tmp;
    CraftMaterials mat;

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
    }

    public void Init(CraftMaterials craft)
    {
        mat = craft;
        icon.sprite = mat.rawMaterial.icon;
        
        if (mat.q > 1)    tmp.text = mat.q.ToString();
        else                tmp.gameObject.SetActive(false);

        UpdateMat();
    }


    public void UpdateMat()
    {
        int count = GameManager.instance.lumberjack.storage.Count(mat.rawMaterial);
        if (count >= mat.q)
            GetComponentInChildren<Image>().color = Color.green;
        else if (count > 0)
            GetComponentInChildren<Image>().color = Color.yellow;
        else
            GetComponentInChildren<Image>().color = Color.grey;
    }    
}
