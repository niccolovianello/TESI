using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine.SceneManagement;

public class UIInventory : MonoBehaviour
{

   
    private MagicInventory inventory;
    public Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public TMP_Text textGems;
    public TMP_Text textWhiteFragment;
    public Image windowToAR;

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
        windowToAR.gameObject.SetActive(true);
    }

    public void CloseWindowToAr()
    {
        windowToAR.gameObject.SetActive(false);
    }

    

    public void OpenSceneInAR()
    {
        //Player magicPlayer = FindObjectOfType
        GameManager gameManager = FindObjectOfType<GameManager>();
        Debug.Log(itemToSeeInAr);
        gameManager.SetPrefabToShowInAR(itemToSeeInAr);
        CloseWindowToAr();
        SceneManager.LoadScene(sceneArtifactAR, LoadSceneMode.Additive);
        gameManager.networkPlayerCamera.enabled = false;
        gameManager.audioListener.enabled = false;
        gameManager.DisableMainGame();
        gameManager.NotRenderPlayerBody();
        
    }

   

}
