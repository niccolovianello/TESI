using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemFragmentVFX : MonoBehaviour
{
    [SerializeField]
    private float velocity = 20f;
    [SerializeField]
    private Vector3 offsetMidPoint = new Vector3(0, 9, 0);
    [SerializeField]
    private Vector3 offsetStartEndPoint = new Vector3(0, 3, 0);

    private Vector3 startMarker, endMarker;
    private Vector3[] waypoints;
    private int currentStartPoint;
    private float startTime;
    private float journeyLength;

    void Start()
    {
        currentStartPoint = 0;

        SetPoints();

    }
    public IEnumerator TransitionPrtycleSystemCoroutine(GameObject go, GameObject netplay1, GameObject netplay2)
    {
        Vector3 midPoint = FindMidPoint(netplay1.gameObject.transform.position, netplay2.gameObject.transform.position);
        
        waypoints = new Vector3[] { netplay1.gameObject.transform.position + offsetStartEndPoint, FindMidPoint(netplay1.gameObject.transform.position, netplay2.gameObject.transform.position) + offsetMidPoint, netplay2.gameObject.transform.position + offsetStartEndPoint};

        SetPoints();

        float distCovered = (Time.time - startTime);
        float fracJourney = distCovered / journeyLength;

        
        while (fracJourney < 1f || currentStartPoint + 2 <= waypoints.Length)
        {
            distCovered = (Time.time - startTime) * velocity;
            fracJourney = distCovered / journeyLength;
            go.transform.position = Vector3.Slerp(startMarker, endMarker, fracJourney);


            if (fracJourney >= 1f && currentStartPoint + 2 < waypoints.Length)
            {
                currentStartPoint++;
                SetPoints();
                fracJourney = 0;

            }
            if (fracJourney >= 1f && currentStartPoint + 2 == waypoints.Length)
            {

                currentStartPoint++;
            }


            yield return null;
        }

        Debug.Log("uscito");
        currentStartPoint = 0;
        Destroy(go);

    }

    private void SetPoints()
    {
        startMarker = waypoints[currentStartPoint];
        endMarker = waypoints[currentStartPoint + 1];
        journeyLength = Vector3.Distance(startMarker, endMarker);
        startTime = Time.time;

    }
    static private Vector3 FindMidPoint(Vector3 starpos, Vector3 endpos)
    {
        return Vector3.Lerp(starpos, endpos, 0.5f);
    }
}
