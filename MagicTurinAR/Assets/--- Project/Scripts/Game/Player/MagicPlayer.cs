using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MagicItemSO;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class MagicPlayer : Player
{

    [SerializeField] public UIInventory uiInventory;
    // [SerializeField] private List<MagicItem> items = new List<MagicItem>();
    
    public MagicInventory inventory;
    public UIManager _uiManager;
    public GameObject playerbody;
    public NetworkPlayer networkPlayer;
    public int maxDistanceFromTHeOthers = 50;


    private void Start()
    {
        NetworkPlayer[] npls = FindObjectsOfType<NetworkPlayer>();
        foreach (NetworkPlayer nt in npls)
        {
            if (nt.isLocalPlayer)
            {
                networkPlayer = nt;
            }
        }
    }

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
                //IncrementWhiteEnergy();
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

    public override void OpenDialogWindowToSeeArtifactsInAR(MagicItemSO item)
    {
        uiInventory.OpenWindowToAr(item);
    }

    public override void RenderPlayerBody()
    {
        playerbody.SetActive(true);
    }

    public override void NotRenderPlayerBody()
    {
        playerbody.SetActive(false);
    }

    public override bool IsCloseToTeamMembers()
    {
        bool isNear = false;

        foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
        {
            if (!nt.isLocalPlayer && Vector3.Distance(this.transform.position, nt.transform.position) < maxDistanceFromTHeOthers)
            {
                isNear = true;
            }
        
        }

        return isNear;
        
    }
}
