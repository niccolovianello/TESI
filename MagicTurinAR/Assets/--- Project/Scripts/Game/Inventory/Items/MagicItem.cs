using System;
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

    private Renderer _renderer;
    private float distanceAlpha = 0.2f;

    
    public enum ItemType
    {
        Artifact,
        Book,
        Rune,
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
                if (networkPlayer.TypePlayerIndex != (int)itemTypePlayer)
                {
                    DoNotRenderItem();
                }
                break;
            }
                
        }

        _renderer = GetComponentInChildren<Renderer>();

        _renderer.material.shader = Shader.Find("Shader Graphs/Alpha");

    }

    private void Update()
    {
        if(IsClickable()) _renderer.material.SetFloat("Alpha", 1);
        
        else _renderer.material.SetFloat("Alpha", distanceAlpha);
    }


    public override void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (IsClickable())
            {
                Debug.Log(uiInventory);
                int debugObjectMissing = 0;
                foreach (MagicItemSO miSO in ItemAssets.Instance.magicInventorySO.items)
                {
                    if (idObjectCode == miSO.id)
                    {
                        debugObjectMissing++;
                        if (miSO.isStackable)
                        {
                            switch (itemType)
                            {
                                case ItemType.Gem:
                                    {
                                        //int amountToPass = magicPlayer.inventory.AddItemToInventory(miSO, this);
                                        //Debug.Log(amountToPass);
                                        //uiInventory.UpdateGemsCount(amountToPass);
                                        if (magicPlayer.IsCloseToTeamMembers())
                                        {
                                            networkPlayer.CmdSendGem();
                                            Vibration.VibratePop();
                                            Destroy(gameObject);
                                            
                                        }
                                        else if (!magicPlayer.IsCloseToTeamMembers())
                                        {
                                            Debug.Log("You are too far from your team mates!");
                                            Vibration.VibrateNope();
                                            StartCoroutine(FindObjectOfType<UIManager>().DistanceWarningScreenSpace("You are too far from your team mates!"));
                                        }

                                        break;

                                    }

                                case ItemType.WhiteMagicFragment:
                                    {
                                        if (magicPlayer.IsCloseToTeamMembers())
                                        {
                                            int amountToPass = magicPlayer.inventory.AddItemToInventory(miSO, this);
                                            Debug.Log(amountToPass);
                                            uiInventory.UpdateWhiteFragmentCount(amountToPass);
                                            Vibration.VibratePop();
                                            Destroy(gameObject);
                                        }
                                        else if (!magicPlayer.IsCloseToTeamMembers())
                                        {
                                            Debug.Log("You are too far from your team mates!");
                                            Vibration.VibrateNope();
                                            StartCoroutine(FindObjectOfType<UIManager>().DistanceWarningScreenSpace("You are too far from your team mates!"));
                                        }

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
                            if (miSO.id == this.idObjectCode && magicPlayer.IsCloseToTeamMembers())
                            {
                                int amountToPass = magicPlayer.inventory.AddItemToInventory(miSO, this);
                                Debug.Log("Aggiunto a inventario " + this.gameObject.name);
                                Vibration.VibratePop();
                                
                                Destroy(gameObject);
                                break;
                            }
                            else if (!magicPlayer.IsCloseToTeamMembers())
                            {
                                Debug.Log("You are too far from your team mates!");
                                Vibration.VibrateNope();
                                StartCoroutine(FindObjectOfType<UIManager>().DistanceWarningScreenSpace("You are too far from your team mates!"));
                            }


                        }
                    }
                }

                if (debugObjectMissing == 0)
                {                   
                        Debug.LogError("Object missing in Inventory Scriptable Object's instance");                   
                }
            }

            else
            {
                Vibration.VibrateNope();
                StartCoroutine(FindObjectOfType<UIManager>().DistanceWarningScreenSpace("You're too far away from this collectable.\nGet closer to catch it!"));
            }
        }
    }
    

    public override void DoNotRenderItem()
    {   
        gameObject.SetActive(false);
    }

    
}