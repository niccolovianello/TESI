namespace Mapbox.Examples
{
	using UnityEngine;
	using Utils;
	using Mapbox.Unity.Map;

	public class SpawnOnMap_Custom : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		float areaRadius = 100f, targetSize = 1f;

		[SerializeField]
		GameObject areaPrefab, targetPrefab;

		public GameObject parentObjectAreaTarget;

		private GameObject area, target; 


		// CUSTOM METHOD
		public void SetNewTargetLocation(float areaLat, float areaLon, float targetLat, float targetLon)
		{
			Vector2d V2area = new Vector2d(areaLat, areaLon);
			Vector2d V2target = new Vector2d(targetLat, targetLon);

			area = Instantiate(areaPrefab);
			area.transform.localPosition = _map.GeoToWorldPosition(V2area, true);
			area.transform.localScale = new Vector3(areaRadius, 1, areaRadius);
			
			target = Instantiate(targetPrefab);
			target.transform.localPosition = _map.GeoToWorldPosition(V2target, true);
			target.transform.localScale = new Vector3(targetSize , targetSize, targetSize);

			area.transform.parent = parentObjectAreaTarget.transform;
			target.transform.parent = parentObjectAreaTarget.transform;
		}

		public void DestroyTargetLocation()
		{
			Destroy(area);
			Destroy(target);
			Debug.Log("Destroy");
		}
	}
}