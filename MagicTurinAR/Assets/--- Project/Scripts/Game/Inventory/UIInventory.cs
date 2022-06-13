using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine.SceneManagement;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class UIInventory : MonoBehaviour
{

    
    private MagicInventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public Text textGems;
    public Text textWhiteFragment;
    public Text textWhiteMagicToSend;
    public Image windowToAR;
    public Image windowToSendWhiteMagic;
    public Button confirmSendWhiteMagic;

    [SerializeField] private Text descriptionItemInAR;



    public GameObject itemToSeeInAr;

    public string sceneArtifactAR;
    public enum TypePlayerUI
    { 
        Explorer,
        Wiseman,
        Hunter
    }
    
    public TypePlayerUI typePlayerUI;



    public GameObject GetItemToSeeInAR()
    {
        return itemToSeeInAr;
    }
    public void SetInventory(MagicInventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshItems();


    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        Debug.Log("On_ItemListChanged invoked");
        RefreshItems();
    }

    public void UpdateGemsCount(int amountGems)
    {
        textGems.text = amountGems.ToString();
    }

    public void UpdateWhiteFragmentCount(int amountWhiteFragment)
    {
        textWhiteFragment.text = amountWhiteFragment.ToString();
    }
    private void RefreshItems()
    {
        // when i refresh the inventory is necessary to destroy the old items of inventory 

        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        //now that the inventory is empty i can fill it with new items


        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;
        foreach (MagicItemSO item in inventory.GetItemList())
        {

            if (!item.isStackable)
            {
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);

                itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
                {
                    //Debug.Log(item);
                    //See Item in AR
                    inventory.ClickOnItem(item);
                };

                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
                Image image = itemSlotRectTransform.GetComponent<Image>();
                image.sprite = item.Sprite;  // <-------
                x++;
                Debug.Log("Added");
            }
            
        }
    }


    public void OpenWindowToAr(MagicItemSO item)
    {
        itemToSeeInAr = item.prefab;
        descriptionItemInAR.text = item.description;
        windowToAR.gameObject.SetActive(true);
    }

    public void OpenWindowToSendWhiteMagic(NetworkPlayer nt, GameObject touchedObject, float whiteMagicToSend)
    {
        Debug.Log("inizia magia");
        windowToSendWhiteMagic.gameObject.SetActive(true);
        confirmSendWhiteMagic.onClick.RemoveAllListeners();

        confirmSendWhiteMagic.onClick.AddListener(() => SendWhiteMagic(nt, touchedObject, whiteMagicToSend));
        Debug.Log("Dopo il listener");


    }

    public void SendWhiteMagic(NetworkPlayer nt, GameObject touchedObject, float whiteMagicToSend)
    {
        Debug.Log("manda magia");

        foreach (MagicItemSO item in ItemAssets.Instance.magicInventorySO.items)
        {
            if (item.id == 2000 && item.prefab.GetComponent<MagicItem>().amount > 0) // White Fragment specific code
            {
                int nfragmet = System.Int32.Parse(textWhiteMagicToSend.text);
                if (item.prefab.GetComponent<MagicItem>().amount >= nfragmet)
                {
                    item.prefab.GetComponent<MagicItem>().amount -= nfragmet;
                    nt.CmdSendWhiteMagic(touchedObject, whiteMagicToSend * nfragmet);
                    textWhiteFragment.text = item.prefab.GetComponent<MagicItem>().amount.ToString();
                    textWhiteMagicToSend.text = "0";
                    Debug.Log(item.prefab.GetComponent<MagicItem>().amount);
                    break;
                }
                else
                {
                    Debug.LogError("You don't have enough white fragments!");
                }

            }
            else if (item.prefab.GetComponent<MagicItem>().amount > 0)
            {
                Debug.LogError("You have not enough white magic!");
            }


        }
    }
    public void CloseWindowToAr()
    {
        windowToAR.gameObject.SetActive(false);
    }

    public void CloseWindowToSendWhiteMagic()
    {
        windowToSendWhiteMagic.gameObject.SetActive(false);
    }



    public void OpenSceneInAR()
    {
        //Player magicPlayer = FindObjectOfType
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.SetPrefabToShowInAR(itemToSeeInAr);
        gameManager.networkPlayerCamera.enabled = false;
        gameManager.audioListener.enabled = false;
        gameManager.DisableMainGame();
        Debug.Log(itemToSeeInAr);
        NetworkPlayer.localPlayer.CmdSetBusy(true);

        CloseWindowToAr();
       
        
        
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.NotRenderPlayerBody();
        }

        SceneManager.LoadScene(sceneArtifactAR, LoadSceneMode.Additive);
    }

    public void IncreaseWhiteMagicToSend()
    {
        int wm = System.Int32.Parse(textWhiteMagicToSend.text);
        foreach (MagicItemSO item in ItemAssets.Instance.magicInventorySO.items)
            if (item.id == 2000 && wm < item.prefab.GetComponent<MagicItem>().amount)
                wm++;
        textWhiteMagicToSend.text = wm.ToString();
    }

    public void DecreaseWhiteMagicToSend()
    {
        int wm = System.Int32.Parse(textWhiteMagicToSend.text);
       
        if(wm >0)
            wm--;
       textWhiteMagicToSend.text = wm.ToString();
    }

    public void SendGemToHunter()
    {
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            if (np.isLocalPlayer)
            {
                FindObjectOfType<UIManager>().OpenWindowToDestroyGemWiseman();
               
            }
        }
    }



}
