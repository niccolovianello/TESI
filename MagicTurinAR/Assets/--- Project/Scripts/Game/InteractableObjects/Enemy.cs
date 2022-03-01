using System;
using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour
{
    private CapsuleCollider collider;
    
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        collider.isTrigger = true;
    }

    private void OnMouseDown()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        SceneManager.LoadScene("AR_EnemyFight", LoadSceneMode.Additive);
        gameManager.PlayerCameraObject.SetActive(false);
        gameManager.DisableMainGame();
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.NotRenderPlayerBody();
        }
    }

}
