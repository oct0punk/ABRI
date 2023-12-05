using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items;
    public GameObject content;
    public InventoryItem prefab;

    private void Awake()
    {
        items = new List<InventoryItem>();
    }

    public void UpdateItem(StockRawMat stock)
    {
        
        InventoryItem item = items.Find(im => im.mat == stock.material);
        if (item == null)
        {
            item = Instantiate(prefab, content.transform);
            item.gameObject.name = stock.material.name;
            item.image.sprite = RawMatManager.instance.GetRawMatByName(stock.material.name).icon;
            item.mat = stock.material;
            items.Add(item);
            RectTransform rect = content.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.rect.width + 240.0f, rect.rect.height);
        }
        item.tmp.text = stock.q.ToString() + " / " + stock.maxQ;
    }
}
