using System.Collections;
using UnityEngine;

public class CutSceneCamera : MonoBehaviour
{
    public void MoveCam(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(Dolly(startPos, endPos));
    }

    private IEnumerator Dolly(Vector3 startPos, Vector3 endPos)
    {
        var elapsedTime = Time.deltaTime;

        while (elapsedTime < 1)
        {
            var position = transform.position;
            position = Vector3.Slerp(new Vector3(20, position.y, 0), new Vector3(50, position.y, 0), elapsedTime);
            transform.position = position;
            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(.01f);
        }
    }
}