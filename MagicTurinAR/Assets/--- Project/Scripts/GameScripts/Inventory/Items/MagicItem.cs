using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class MagicItem : Item
{
    
    [SerializeField] public MagicPlayer magicPlayer;
    public NetworkPlayer networkPlayer;
    public ItemType itemType;
    public ItemTypePlayer itemTypePlayer;
    public int idObjectCode;
    public enum ItemType
    {
        Artifact,
        Rune,
        Book,
        Gem,
        WhiteMagicFragment
    }

    public enum ItemTypePlayer
    {
        Explorer,
        Wiseman,
        Hunter
    }



    private void Awake()
    {
        colliderCenter = new Vector3(0, 3, 0);
        colliderItem = GetComponent<SphereCollider>();
        colliderItem.isTrigger = true;
        colliderItem.radius = colliderRadius;
        colliderItem.center = colliderCenter;

        //canvasItem = GetComponentInChildren<Canvas>();
        //textWarning = GetComponentInChildren<TMP_Text>();
        //Debug.Log(canvasItem);
        //canvasItem.enabled = false;
        uiInventory = FindObjectOfType<UIInventory>();

    }

    private void Start()
    {

        player = FindObjectOfType<Player>();
        magicPlayer = FindObjectOfType<MagicPlayer>();
        NetworkPlayer[] networkPlayers = FindObjectsOfType<NetworkPlayer>();

        foreach (NetworkPlayer nt in networkPlayers)
        {
            if (nt.isLocalPlayer)
            {
                networkPlayer = nt;
                if ((int)networkPlayer.TypePlayerIndex != (int)itemTypePlayer)
                {
                    DoNotRenderItem();
                }
                break;
            }
                
        }

        if (uiInventory == null)
        {
            Debug.LogWarning("uiInventory null!");
            Destroy(gameObject);
        }
           
    }

    public override void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (IsClickable())
            {

                Debug.Log(uiInventory);
                foreach (MagicItemSO miSO in ItemAssets.Instance.magicInventorySO.items)
                {

                    if (this.idObjectCode == miSO.id)
                    {
                        if (miSO.isStackable)
                        {
                            switch (itemType)
                            {
                                case ItemType.Gem:
                                    {
                                        int amountToPass = magicPlayer.inventory.AddItemToInventory(miSO, this);
                                        Debug.Log(amountToPass);
                                        uiInventory.UpdateGemsCount(amountToPass);
                                        Destroy(gameObject);
                                        break;
                                    }

                                case ItemType.WhiteMagicFragment:
                                    {
                                        break;
                                    }

                                default:
                                    {
                                        Debug.LogWarning("Are you sure that this object should be stackable?");
                                        break;
                                    }

                            }
                        }
                        else
                        {
                            //Debug.Log(uiInventory);                          
                            Debug.Log(this.name);
                            if (miSO.id == this.idObjectCode)
                            {
                                int amountToPass = magicPlayer.inventory.AddItemToInventory(miSO, this);
                                Debug.Log("Aggiunto a inventario " + this.gameObject.name);
                                Destroy(gameObject);
                                break;
                            }
                        }
                    }

                    else
                    {
                        Debug.LogError("Object missing in Inventory Scriptable Object's instance");
                    }

                }

                // aggiungi roba all'inventario
            }

            else
            {
                //canvasItem.enabled = true;
                //textWarning.text = "This item is too far, get closer!";
                StartCoroutine(DisableTextWarning());
                // avvisa che l'oggetto è troppo lontano
            }
        }
    }


    public IEnumerator DisableTextWarning()
    {
        yield return new WaitForSeconds(4f);
        //canvasItem.enabled = false;
    }

    public override void DoNotRenderItem()
    {
        this.gameObject.SetActive(false);
    }
}