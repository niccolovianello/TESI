namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _areaPrefab, _targetPrefab;

		public List<GameObject> _spawnedObjects;
		public GameObject parentObjectAreaTarget;

		//void Start()
		//{
  //          _locationStrings = new[] { "0,0", "0,0" };

  //          _locations = new Vector2d[_locationStrings.Length];
  //          _spawnedObjects = new List<GameObject>();
  //          for (int i = 0; i < _locationStrings.Length; i++)
  //          {
  //              var locationString = _locationStrings[i];
  //              _locations[i] = Conversions.StringToLatLon(locationString);
  //              var instance = Instantiate(_areaPrefab);
  //              instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
  //              instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
  //              _spawnedObjects.Add(instance);
  //          }
  //      }

		private void Update()
		{
			//int count = _spawnedObjects.Count;
			//for (int i = 0; i < count; i++)
			//{
			//	var spawnedObject = _spawnedObjects[i];
			//	var location = _locations[i];
			//	spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
			//	spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			//}



		}

		// CUSTOM METHOD
		public void SetNewTargetLocation(float areaLat, float areaLon, float targetLat, float targetLon)
		{
			Vector2d V2area = new Vector2d(areaLat, areaLon);
			Vector2d V2target = new Vector2d(targetLat, targetLon);

			/*
			List<string> locations = new List<string>(_locationStrings);
			locations.Clear();
			locations.Add(latitude + ", " + longitude);
			_locationStrings = locations.ToArray();
			*/

			var area = Instantiate(_areaPrefab);
			area.transform.localPosition = _map.GeoToWorldPosition(V2area, true);
			area.transform.localScale = new Vector3(60, 1, 60);
			
			var target = Instantiate(_targetPrefab);
			target.transform.localPosition = _map.GeoToWorldPosition(V2target, true);
			target.transform.localScale = new Vector3(_spawnScale , _spawnScale, _spawnScale);

			area.transform.parent = parentObjectAreaTarget.transform;
			target.transform.parent = parentObjectAreaTarget.transform;
		}

	}
}