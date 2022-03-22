using UnityEngine;
using static MagicItemSO;
using UnityEngine.UI;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class MagicPlayer : Player
{

    [SerializeField] public UIInventory uiInventory;
    // [SerializeField] private List<MagicItem> items = new List<MagicItem>();
    
    public MagicInventory inventory;
    public UIManager _uiManager;
    public NetworkPlayer networkPlayer;
    public int maxDistanceFromTheOthers = 50;

    public Slider manaBar;
    
    public ManaManager manaManager;
    
    public float maxMana = 100;
    public float currentMana = 100;

    public float CurrentMana
    {
        get => currentMana;
        set => currentMana = value;
    }
    
    
    private void Awake()
    {
        NetworkPlayer[] npls = FindObjectsOfType<NetworkPlayer>();
        foreach (NetworkPlayer nt in npls)
        {
            if (nt.isLocalPlayer)
            {
                networkPlayer = nt;
            }
        }
        
        manaBar = GameObject.Find("ManaBar").GetComponent<Slider>();
        //currentMana = maxMana;
    }
    
    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
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

  

    public override bool IsCloseToTeamMembers()
    {
        bool isNear = false;

        foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
        {
            if (!nt.isLocalPlayer && Vector3.Distance(this.transform.position, nt.transform.position) < maxDistanceFromTheOthers)
            {
                isNear = true;
            }
            else
            {
                isNear = false;
                break;
            }
            
        
        }

        return isNear;
        
    }
}
