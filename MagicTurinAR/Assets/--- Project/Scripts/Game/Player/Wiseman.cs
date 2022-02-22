using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine;

public class Wiseman : MagicPlayer
{
    [SerializeField] private float whiteMagicToSend = 25f;

    private void Start()
    {
        Destroy(manaBar.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            uiInventory.UpdateGemsCount(inventory.AddGem()); 
           
        }
        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit hit;

        //    //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log(hit.transform.name);
        //        if (hit.collider != null)
        //        {

        //            GameObject touchedObject = hit.transform.gameObject;

        //            Debug.Log("Touched " + touchedObject.transform.name);
        //        }
        //    }
        //}

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                if (hit.collider != null)
                {

                    GameObject touchedObject = hit.transform.gameObject;
                    MagicPlayer touchedPlayer = touchedObject.GetComponent<MagicPlayer>();

                    if (touchedObject.tag == "Player" && !(touchedPlayer is Wiseman)) 
                    {
                        Debug.Log(this);
                        uiInventory.OpenWindowToSendWhiteMagic(networkPlayer, touchedObject, whiteMagicToSend);
                    }
                }
            }
        }
    }

    public void IncrementGems()
    {
        int amountToPass = 0;
        foreach (MagicItemSO MagicItem in ItemAssets.Instance.magicInventorySO.items)
        {
            if ((int)MagicItem.id == 1000)
            {
                Debug.Log("incrementGems");
                MagicItem.prefab.GetComponent<Item>().amount++;
                //itemAlreadyInInventory = true;
                //Debug.Log(Item.amount);
                amountToPass = MagicItem.prefab.GetComponent<Item>().amount;
            }
        }
        //Debug.Log(amountToPass);
        uiInventory.UpdateGemsCount(amountToPass);
    }

}
