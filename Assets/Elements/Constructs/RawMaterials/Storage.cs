using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class StockRawMat {
    public RawMaterial material;
    [Min(0)] public int q;
    [Min(-1)] public int maxQ;

    public StockRawMat(RawMaterial material, int q = 0, int maxQ = 4)
    {
        this.material = material;
        this.q = q;
        this.maxQ = maxQ;
    }
}

public class Storage : MonoBehaviour
{
    [SerializeField] StockRawMat[] content;
    public PresBubble pres;
    public Canvas canvas;

    private void Start()
    {
        for (int i = 0; i < content.Length - 1; i++)
        {
            for (int j = i + 1; j < content.Length; j++)
            {
                if (content[i].material == null)
                    Debug.LogWarning("Null material in storage " + content[i].material.name, this);
                
                else if (content[j].material == null)
                    Debug.LogWarning("Null material in storage " + content[j].material.name, this);

                else if (content[i].material.name == content[j].material.name)
                    Debug.LogWarning("Same material in storage " + content[i].material.name, this);
            }
        }
    }

    StockRawMat GetMatInContent(RawMaterial material)
    {
        StockRawMat mat = Array.Find(content, Mat => Mat.material.name == material.name);
        if (mat != null)
            return mat;
        else { 

            // Create a new slot for the material
            StockRawMat[] copy = new StockRawMat[content.Length + 1];
            content.CopyTo(copy, 0);
            copy[content.Length] = new StockRawMat(material);
            content = copy;
            // GameUI.instance.inventory.UpdateItem(content[content.Length - 1]);
            return content[content.Length - 1];
        }
    }


    public void Add(RawMaterial material, int count = 1)
    {
        StockRawMat stock = GetMatInContent(material);
        if (stock != null)
        {
            stock.q += count;
            GameUI.instance.inventory.UpdateItem(stock);
            if (stock.q > stock.maxQ) Debug.LogWarning("Q out of range : " + stock.material.name, this);
        }
    }

    // true if you can fill 'count' of 'type'
    public bool CanFill(RawMaterial material, int count = 1)
    {
        StockRawMat stock = GetMatInContent(material);
        if (stock.maxQ == -1) return true;
        if (stock != null)        
            return stock.q + count <= stock.maxQ;
        
        else        
            return false;
    }

    // return the max you can fill
    public int CountEmpty(RawMaterial material)
    {
        StockRawMat stock = GetMatInContent(material);
        if (stock == null)
            return -1;
        return stock.maxQ - stock.q;
    }

    // count an element in storage
    public int Count(RawMaterial material)
    {
        StockRawMat stock = GetMatInContent(material);
        if (stock != null)        
            return stock.q;
        return -1;
        
    }

    public void Craft(RawMaterial material)
    {
        Consume(material.craftMaterials);
        Add(material);
    }

    public void Consume(CraftMaterials[] materials)
    {
        foreach (var resources in materials)
        {
            Add(resources.rawMaterial, -resources.q);
        }
    }

    public bool CanCraft(CraftMaterials[] materials)
    {
        return GetMissedMats(materials).Count == 0;
    }

    public List<RawMaterial> GetMissedMats(CraftMaterials[] materials)
    {
        List<RawMaterial> res = new List<RawMaterial>();
        foreach (var material in materials)
        {
            if (Count(material.rawMaterial) < material.q)
                res.Add(material.rawMaterial);
        }
        return res;
    }
}
