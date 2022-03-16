using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class GraphicInterctionBetweenPlayersScript : MonoBehaviour
{
    public GameObject prefabWhiteMagicParticleSystem;
    public GameObject prefabGemParticleSystem;


    public void WisemanSendWhiteMagic( NetworkPlayer netplay1, NetworkPlayer netplay2)
    {
        GameObject go = Instantiate(prefabWhiteMagicParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        StartCoroutine(TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc white magic");
        

    }

    public void ExplorerSendGem(NetworkPlayer netplay1, NetworkPlayer netplay2)
    {
        GameObject go = Instantiate(prefabGemParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        StartCoroutine(TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc gem");
    }

    public IEnumerator TransitionPrtycleSystemCoroutine(GameObject go, NetworkPlayer netplay1, NetworkPlayer netplay2)
    {

        float elapsedTime = 0;
        float waitTime = 2f;
        while (elapsedTime < waitTime)
        {
            go.transform.position = Vector3.Slerp(netplay1.gameObject.transform.position, netplay2.gameObject.transform.position, 2.0f);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        Destroy(go);
        Vibration.VibratePeek();
    }
}
