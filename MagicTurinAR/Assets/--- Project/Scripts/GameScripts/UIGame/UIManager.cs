using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject power;

    [SerializeField] private MagicPlayer player;

    public FirebaseManager firebaseManager;
    public StoreDataScript storeDataScript;
    
    
    public void Awake()
    {
        Assert.IsNotNull(menu);
    }
    
    void Start()
    {
        menu.SetActive(false);
        firebaseManager = FindObjectOfType<FirebaseManager>();
        storeDataScript = FindObjectOfType<StoreDataScript>();
        player = FindObjectOfType<MagicPlayer>();
        //player.SetUIManager(this);
    }

    public GameObject GetPower => power;
    

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.O))
    //    {
    //        onClickSaveButton();
    //    }
    //}
 

    public void ToggleBackPack()
    {
        menu.SetActive(!menu.activeSelf);
        Debug.Log("toggle");
    }
    
    public void TogglePower()
    {
        power.SetActive(!power.activeSelf);
    }

    public void onClickSaveButton()
    {
        Debug.Log("Button Save clicked");
        firebaseManager.SaveData(storeDataScript);
    }
}