using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

namespace ____Project.Scripts
{
	public class SpawnTargetOnMap : MonoBehaviour
	{
		[SerializeField] private AbstractMap map;

		[SerializeField] private float areaRadius = 100f, targetSize = 1f;
		
		private GameObject _areaPrefab, _targetPrefab, _area, _target;

		public GameObject parentObjectAreaTarget;

		public void SetNewTargetLocation(float areaLat, float areaLon, float targetLat, float targetLon, Vector3 offset)
		{
			
			var mm = FindObjectOfType<MissionsManager>();
			
			_areaPrefab = mm.currentMission.goalExplorerMissionAreaPrefab;
			_targetPrefab = mm.currentMission.goalExplorerMissionPrefab;
			var v2Area = new Vector2d(areaLat, areaLon);
			var v2Target = new Vector2d(targetLat, targetLon);

			_area = Instantiate(_areaPrefab, parentObjectAreaTarget.transform, true);
			_area.transform.localPosition = map.GeoToWorldPosition(v2Area);
			_area.transform.localScale = new Vector3(areaRadius, 1, areaRadius);
			
			_target = Instantiate(_targetPrefab, parentObjectAreaTarget.transform, true);
			var localPosition = map.GeoToWorldPosition(v2Target);
			localPosition += offset;
			_target.transform.localPosition = localPosition;
			_target.transform.localScale = new Vector3(targetSize , targetSize, targetSize);

			SetNavigationPower(_target.transform);

		}

		public void DestroyTargetLocation()
		{
			Destroy(_area);
			Destroy(_target);
			Debug.Log("Destroy");
		}

		public static void SetNavigationPower(Transform target)
		{
			var ex = FindObjectOfType<Explorer>();

			if (ex != null)
				ex.GetDirections()._waypoints[1] = target.transform;
		}

		

	}
}