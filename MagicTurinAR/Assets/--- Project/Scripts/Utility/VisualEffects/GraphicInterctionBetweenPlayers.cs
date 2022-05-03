using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class GraphicInterctionBetweenPlayers : MonoBehaviour
{
    public GameObject prefabWhiteMagicParticleSystem;
    public GameObject prefabGemParticleSystem;
    public Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 offsetMid = new Vector3(0, 7, 0);
    public float scaleMultiplier = 8;


    public void WisemanSendWhiteMagic( GameObject netplay1, GameObject netplay2)
    {
        GameObject go = Instantiate(prefabWhiteMagicParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        go.transform.localScale *= scaleMultiplier;
        StartCoroutine(TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc white magic: player1" + netplay1 + "    player2" + netplay2);
        

    }

    public void ExplorerSendGem(GameObject netplay1, GameObject netplay2)
    {
        GameObject go = Instantiate(prefabGemParticleSystem, netplay1.gameObject.transform.position, Quaternion.identity);
        go.transform.localScale *= scaleMultiplier;
        StartCoroutine(TransitionPrtycleSystemCoroutine(go, netplay1, netplay2));
        Debug.Log("Visual effetc gem: player1" + netplay1 + "    player2" + netplay2);
    }

    public IEnumerator TransitionPrtycleSystemCoroutine(GameObject go, GameObject netplay1, GameObject netplay2)
    {

        float elapsedTime = 0;
        float waitTime = 2f;
        while (elapsedTime < waitTime/2)
        {
            go.transform.position = Vector3.Slerp(netplay1.gameObject.transform.position + offset, FindMidPoint(netplay1.transform.position, netplay2.transform.position) + offsetMid, elapsedTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        while (elapsedTime < waitTime)
        {
            go.transform.position = Vector3.Slerp(FindMidPoint(netplay1.transform.position, netplay2.transform.position) + offsetMid, netplay2.gameObject.transform.position + offset, elapsedTime);
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        Destroy(go);
        Vibration.VibratePeek();
    }

    private Vector3 FindMidPoint(Vector3 starpos, Vector3 endpos)
    {
        Vector3 result = new Vector3();
        result.x = starpos.x + (endpos.x - starpos.x / 2);
        result.y = starpos.y + (endpos.y - starpos.y / 2);
        result.z = starpos.z + (endpos.z - starpos.z / 2);

        return result;
    }
}
