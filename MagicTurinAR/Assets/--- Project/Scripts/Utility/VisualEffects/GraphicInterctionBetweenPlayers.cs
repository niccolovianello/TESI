using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class GraphicInterctionBetweenPlayers : MonoBehaviour
{
    public GameObject prefabWhiteMagicParticleSystem;
    public GameObject prefabGemParticleSystem;

    public float scaleMultiplier = 10;


    public void WisemanSendWhiteMagic( GameObject netplay1, GameObject netplay2)
    {
        GameObject go = Instantiate(prefabWhiteMagicParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        go.transform.localScale *= scaleMultiplier;
        StartCoroutine(go.GetComponent<GemFragmentVFXScript>().TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc white magic: player1" + netplay1 + "    player2" + netplay2);
        

    }

    public void ExplorerSendGem(GameObject netplay1, GameObject netplay2)
    {
        GameObject go = Instantiate(prefabGemParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        go.transform.localScale *= scaleMultiplier;
        StartCoroutine(go.GetComponent<GemFragmentVFXScript>().TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc gem: player1" + netplay1 + "    player2" + netplay2);
    }

}
