using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class GUIStatsManager : MonoBehaviour
{
    [Header("HorizontalNavigation")]
    public Image sxPoint;
    public Image dxPoint;
    public Image cxPoint;

    [Header("Collectables Container")]
    public GridLayoutGroup explorerArtifactContainer;
    public GridLayoutGroup hunterRuneContainer;
    public GridLayoutGroup wisemanBooksContainer;

    [Header("Usernames")]
    public Text explorerUsername;
    public Text hunterUsername;
    public Text wisemanUsername;

    [Header("Collectable Prefab")]
    public GameObject collectablePrefab;

    private TurnManager turnManager;
    private void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();

        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            switch (np.TypePlayerEnum)
            {
                case NetworkPlayer.TypePlayer.Explorer:
                    explorerUsername.text = np.FirebaseManager.username;
                    break;
                case NetworkPlayer.TypePlayer.Hunter:
                    hunterUsername.text = np.FirebaseManager.username;
                    break;
                case NetworkPlayer.TypePlayer.Wiseman:
                    wisemanUsername.text = np.FirebaseManager.username;
                    break;
            }
                       
        }

        foreach (MagicItemSO miSO in turnManager.ItemsPicked)
        {
            switch (miSO.itemType)
            {
                case MagicItemSO.ItemType.Artifact:
                    GameObject artifact = Instantiate(collectablePrefab, explorerArtifactContainer.transform);
                    artifact.GetComponent<UICollectablePrefabStats>().SetCollectablePrefab(miSO.Sprite, miSO.name);
                    break;

                case MagicItemSO.ItemType.Book:
                    GameObject book = Instantiate(collectablePrefab, wisemanBooksContainer.transform);
                    book.GetComponent<UICollectablePrefabStats>().SetCollectablePrefab(miSO.Sprite, miSO.name);
                    break;

                case MagicItemSO.ItemType.Rune:
                    GameObject rune = Instantiate(collectablePrefab, hunterRuneContainer.transform);
                    rune.GetComponent<UICollectablePrefabStats>().SetCollectablePrefab(miSO.Sprite, miSO.name);
                    break;
            }

        }

    }

    public void Log()
    {
        Debug.Log("cambia");
    }

}
