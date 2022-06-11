using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicInventory : Inventory
{

    public event EventHandler OnItemListChanged;
    public Action<MagicItemSO> clickItemAction;

    public MagicInventory(Action<MagicItemSO> clickItemAction)
    {
        this.clickItemAction = clickItemAction;
        itemList = new List<MagicItemSO>();

     
    }
    
    public int AddItemToInventory(MagicItemSO item, MagicItem magicItem)
    {       

        int returnedAmount = 1;
        if (item.isStackable)
        {

            foreach (MagicItemSO MagicItem in ItemAssets.Instance.magicInventorySO.items)
            {
                if ((int)MagicItem.itemType == (int) magicItem.itemType)
                {
                    MagicItem.prefab.GetComponent<Item>().amount++;                  
                    returnedAmount = MagicItem.prefab.GetComponent<Item>().amount;
                }
            }
        }

        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);

        
        return returnedAmount;
    }


    public void ClickOnItem(MagicItemSO item)
    {
        clickItemAction(item);
    }

    public int AddGem()
    {
        int returnedAmount = 1;
        foreach (MagicItemSO MagicItem in ItemAssets.Instance.magicInventorySO.items)
        {
            
            if ((int)MagicItem.itemType == 1000)
            {
                MagicItem.prefab.GetComponent<Item>().amount++;
                returnedAmount = MagicItem.prefab.GetComponent<Item>().amount;
            }
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        return returnedAmount;
        
    }
}
    
    