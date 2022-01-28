using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlayer : Player
{

    [SerializeField] public UIInventory uiInventory;
   // [SerializeField] private List<MagicItem> items = new List<MagicItem>();


    private int whitePower = 100;
    
    public MagicInventory inventory;
    public UIManager _uiManager;


    private void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>();
        _uiManager = FindObjectOfType<UIManager>();
        inventory = new MagicInventory(ClickOnItemInInventory);
        uiInventory.SetInventory(inventory);
    }
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void ClickOnItemInInventory(MagicItemSO item)
    {
       
        switch (item.itemTypeIndex)
        {
            case (int)MagicItem.ItemType.WhiteMagicFragment:
                IncrementWhiteEnergy();
                break;
            case (int)MagicItem.ItemType.Gem:
                // faccio funzione gemme

                break;
            default:
                //Debug.Log(item.itemGO);
                OpenDialogWindowToSeeArtifactsInAR(item);
                
                break;
        }
        
    }



    //metodi Interfaccia
    public bool IsNearTeamMembers()
    {
        return true;
    }


    public override void OpenDialogWindowToSeeArtifactsInAR(MagicItemSO item)
    {
        uiInventory.OpenWindowToAr(item);
    }


    // metodi player

    public void IncrementWhiteEnergy()
    {
        
    }
    
    public void ReduceWhitePower()
    {
        whitePower--;
    }

    public bool HasWhitePower()
    {
        return whitePower > 0;
    }

   
}
