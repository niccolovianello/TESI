using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        gameManager.networkPlayerCamera.enabled = false;
        gameManager.audioListener.enabled = false;
        gameManager.DisableMainGame();
        gameManager.NotRenderPlayerBody();
    }

}
