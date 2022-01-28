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

        
        //magicItem.itemGO = item.prefab;


        int returnedAmount = 1;
        if (item.isStackable)
        {

            //bool itemAlreadyInInventory = false;
            foreach (MagicItemSO MagicItem in ItemAssets.Instance.magicInventorySO.items)
            {
                if (MagicItem.itemTypeIndex == (int)magicItem.itemType)
                {
                    Debug.Log("entrato");
                    MagicItem.prefab.GetComponent<Item>().amount++;
                    //itemAlreadyInInventory = true;
                    //Debug.Log(Item.amount);
                    returnedAmount = MagicItem.prefab.GetComponent<Item>().amount;
                }
            }

            //if (!itemAlreadyInInventory)
            //{
            //    itemList.Add(item);

            //}
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
}
    
    