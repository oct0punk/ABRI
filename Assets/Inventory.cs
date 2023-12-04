using System;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items;
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
            item = Instantiate(prefab, transform);
            item.gameObject.name = stock.material.name;
            item.image.sprite = RawMatManager.instance.GetRawMatByName(stock.material.name).icon;
            item.mat = stock.material;
            items.Add(item);
        }
        item.tmp.text = stock.q.ToString() + " / " + stock.maxQ;
    }
}
