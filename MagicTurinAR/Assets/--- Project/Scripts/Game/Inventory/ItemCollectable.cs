using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour
{
    private Item item;

    private GameObject itemCollectableGO;

    public GameObject getGameObject => itemCollectableGO;

    //public static ItemCollectable SpawnItemCollectable(Vector3 position, Item item)
    //{
    //    Transform transform = Instantiate(ItemAssets.Instance.prefabItemCollectable, position, Quaternion.identity);

    //    ItemCollectable itemCollectable = transform.GetComponent<ItemCollectable>();
    //    itemCollectable.SetItem(item);

    //    return itemCollectable;
    //}

    //public void SetItem(Item item)
    //{
    //    this.item = item;
    //    itemCollectableGO = item.GetGameObject();
    //}

    //public Item GetItem()
    //{
    //    return item;
    //}

    //public void DestroySelf()
    //{
    //    Destroy(gameObject);
    //}

}
