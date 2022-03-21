using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class GraphicInterctionBetweenPlayers : MonoBehaviour
{
    public GameObject prefabWhiteMagicParticleSystem;
    public GameObject prefabGemParticleSystem;


    public void WisemanSendWhiteMagic( GameObject netplay1, GameObject netplay2)
    {
        GameObject go = Instantiate(prefabWhiteMagicParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        StartCoroutine(TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc white magic: player1" + netplay1 + "    player2" + netplay2);
        

    }

    public void ExplorerSendGem(GameObject netplay1, GameObject netplay2)
    {
        GameObject go = Instantiate(prefabGemParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        StartCoroutine(TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc gem: player1" + netplay1 + "    player2" + netplay2);
    }

    public IEnumerator TransitionPrtycleSystemCoroutine(GameObject go, GameObject netplay1, GameObject netplay2)
    {

        float elapsedTime = 0;
        float waitTime = 2f;
        while (elapsedTime < waitTime)
        {
            go.transform.position = Vector3.Slerp(netplay1.gameObject.transform.position, netplay2.gameObject.transform.position, elapsedTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        Destroy(go);
        Vibration.VibratePeek();
    }
}
