using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Examples;
using System;

public class CityLoader : MonoBehaviour
{
    public List<string> cities = new List<string>();
    private List<string> cityName = new List<string>();
    private List<float> latitude = new List<float>();
    private List<float> longitude = new List<float>();
    public GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        FindCityLocations();
    }

    async private void FindCityLocations()
    {
        GetLocation locationGetter = this.GetComponent<GetLocation>();
        foreach (string city in cities)
        {
            OpenCageData apiResult = await locationGetter.GetCityLocation(city);
            latitude.Add(apiResult.results[0].geometry.lat);
            longitude.Add(apiResult.results[0].geometry.lng);
            cityName.Add(city);

        }
        
        SpawnOnMap markerSpawner = map.transform.GetComponent<SpawnOnMap>();
        markerSpawner.Initialize(latitude, longitude, cityName);
    }
}
