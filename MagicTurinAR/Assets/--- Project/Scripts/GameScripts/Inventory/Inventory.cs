using System.Collections.Generic;
using System;

public abstract class Inventory
{
    
    public List<MagicItemSO> itemList;

    public List<MagicItemSO> GetItemList()
    {
        return itemList;
    }

    public MagicItemSO GetItemFromItemList(int index)
    {
        return itemList[index];
    }

}
