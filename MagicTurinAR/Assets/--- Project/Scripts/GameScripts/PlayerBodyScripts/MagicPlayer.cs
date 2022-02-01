using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MagicItemSO;

public class MagicPlayer : Player
{

    [SerializeField] public UIInventory uiInventory;
   // [SerializeField] private List<MagicItem> items = new List<MagicItem>();


    private int whitePower = 100;
    
    public MagicInventory inventory;
    public UIManager _uiManager;
    public GameObject playerbody;


    
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void SetPlayerBody(GameObject _playerbody)
    {
        this.playerbody = _playerbody;
    }

    public void InitializeInventory()
    {
        uiInventory = FindObjectOfType<UIInventory>();
        Debug.Log(uiInventory);
        inventory = new MagicInventory(ClickOnItemInInventory);
        uiInventory.SetInventory(inventory);

       
    }

    public void ClickOnItemInInventory(MagicItemSO item)
    {
       
        switch (item.itemType)
        {
            case ItemType.WhiteFragment:
                IncrementWhiteEnergy();
                break;
            case ItemType.Gem:
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

    public override void RenderPlayerBody()
    {
        playerbody.SetActive(true);
    }

    public override void NotRenerPlayerBody()
    {
        playerbody.SetActive(false);
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
