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

        public GameObject markerPrefab;
        public BarGraphController barGraph;

		List<GameObject> _spawnedObjects;
        bool initialized = false;
        [SerializeField] private EditMaterial terrainController;
        [SerializeField] private WeatherDomeController weatherDome;

        

        List<string> names = new List<string>();

        void Start()
		{

		}

        public void UpdateWeatherTime(float sliderValue)
        {
            foreach(GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.AdjustTime(sliderValue);
            }
        }

        public void SetRainBarActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setRainBarActive(value);
                markerController.renderMarker();
            }
        }

        public void SetTempActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setTempActive(value);
                markerController.renderMarker();
            }
        }

        public void setWindArrowActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setWindArrowActive(value);
                markerController.renderMarker();
            }
        }

        public void setSnowingActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setSnowingActive(value);
                markerController.renderMarker();
            }
        }

        public void setCloudyActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setCloudyActive(value);
                markerController.renderMarker();
            }
        }

        public void setPartlyCloudyDayActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setPartlyCloudyDayActive(value);
                markerController.renderMarker();
            }
        }

        public void setPartlyCloudyNightActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setPartlyCloudyNightActive(value);
                markerController.renderMarker();
            }
        }

        public void setClearDayActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setClearDayActive(value);
                markerController.renderMarker();
            }
        }

        public void setClearNightActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setClearNightActive(value);
                markerController.renderMarker();
            }
        }

        public void setRainActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setRainActive(value);
                markerController.renderMarker();
            }
        }

        public void setSleetActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setSleetActive(value);
                markerController.renderMarker();
            }
        }

        public void setFogActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setFogActive(value);
                markerController.renderMarker();
            }
        }

        public void setWindActive(bool value)
        {
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                markerController.setWindActive(value);
                markerController.renderMarker();
            }
        }

        public List<DarkskyForecast> GetWeather()
        {
            List<DarkskyForecast> weather = new List<DarkskyForecast>();
            foreach (GameObject marker in _spawnedObjects)
            {
                WeatherControl2 markerController = marker.GetComponent<WeatherControl2>();
                weather.Add(markerController.getWeatherData());
            }
            return weather;
        }

        public void Initialize(List<float> latitude, List<float> longitude, List<string> cityName)
        {
            List<string> locations = new List<string>();
            _spawnedObjects = new List<GameObject>();
            _locations = new Vector2d[latitude.Count];

            for (int i = 0; i < longitude.Count; i++)
            {
                string location = latitude[i].ToString(System.Globalization.CultureInfo.InvariantCulture) + ',' + longitude[i].ToString(System.Globalization.CultureInfo.InvariantCulture);
                names.Add(cityName[i]);
                
                var locationString = location;
                _locations[i] = Conversions.StringToLatLon(locationString);
                GameObject instance = Instantiate(markerPrefab);

                CityOverviewInteracter cityOverviewInteracter = instance.GetComponentInChildren<CityOverviewInteracter>();
                cityOverviewInteracter.setDefaultTerrainController(terrainController);
                cityOverviewInteracter.setWeatherDomeController(weatherDome);
                //FaceFront faceFronter = instance.GetComponentInChildren<FaceFront>();
                //faceFronter.target = player;


                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _spawnedObjects.Add(instance);
            }

            initialized = true;

        }

        bool barChartInitialized = false;

        private bool ValidData()
        {
            if (!initialized) return false;
            
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                WeatherControl2 marker = _spawnedObjects[i].GetComponent<WeatherControl2>();
                if(marker.name == null || marker.getWeatherData().currently == null)
                {
                    return false;
                }
            }

            return true;

        }

        private void Update()
		{
            if (!initialized) return;
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
                string name = names[i];
                WeatherControl2 weatherControl = spawnedObject.GetComponent<WeatherControl2>();
                weatherControl.setLongitude(location.y);
                weatherControl.setLatitude(location.x);
                weatherControl.setName(name);
                if (!weatherControl.initialized())
                {
                    weatherControl.CheckWeatherStatus();
                    weatherControl.initialize();
                }


                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }

            if(!barChartInitialized && ValidData())
            {
                barGraph.Initialize();
                barChartInitialized = true;
            }
		}
	}
}