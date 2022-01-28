using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIGameScript : MonoBehaviour
{
    public FirebaseManager firebaseManager;
    public StoreDataScript storeDataScript;
    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        storeDataScript = FindObjectOfType<StoreDataScript>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            onClickSaveButton();
        }
    }

    public void onClickSaveButton() {

        Debug.Log("Button Save clicked");
        firebaseManager.SaveData(storeDataScript);
    }

    
   
}
