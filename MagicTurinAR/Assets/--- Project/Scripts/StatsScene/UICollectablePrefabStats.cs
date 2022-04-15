using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICollectablePrefabStats : MonoBehaviour
{
    public Image collectableSprite;
    public Text collectableName;

    public void SetCollectablePrefab(Sprite collectableSprite, string collectableName)
    {
        this.collectableSprite.sprite = collectableSprite;
        this.collectableName.text = collectableName;
    }
}
