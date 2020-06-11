using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CityOverviewInteracter : MonoBehaviour
{
    private GameObject CityOverview;
    private WeatherControl2 root;
    private CityOverviewController overview_controller;
    private WeatherDomeController weatherDome;
    [SerializeField] private EditMaterial defaultTerrain;

    void Start()
    {
        GameObject CityOverviewWrapper = GameObject.Find("CityOverviewWrapper");
        CityOverview = CityOverviewWrapper.transform.Find("CityOverview").gameObject;
        overview_controller = CityOverview.GetComponent<CityOverviewController>();
        root = gameObject.GetComponentInParent<WeatherControl2>();
    }
    public void setDefaultTerrainController(EditMaterial terrainController)
    {
        defaultTerrain = terrainController;
    }

    public void setWeatherDomeController(WeatherDomeController weatherDomeIn)
    {
        weatherDome = weatherDomeIn;
    }


    public void OpenPanel()
    {
        if (CityOverview != null)
        {            
            bool isActive = CityOverview.activeSelf;
            string city1 = root.getCityName();
            string city2 = overview_controller.getCityName();
            city2 = city2.ToLower();
            if (isActive &&  city1 == city2)
            {
                CityOverview.SetActive(false);
                defaultTerrain.setDefaultTerrain();
                weatherDome.defaultWeatherDome();
            }
            else
            {
                overview_controller.updateData(root.getWeatherData(), root.getCityName());
                CityOverview.SetActive(true);
            }
        }
    }
}
