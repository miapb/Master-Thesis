using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TemperatureGraphInteracter : MonoBehaviour
{
    private GameObject GraphCanvas;
    private TemperatureGraphController temperatureGraphController;
    private WeatherControl2 root;


    void Start()
    {
        GameObject TemperatureGraphWrapper = GameObject.Find("TemperatureGraphWrapper");
        GraphCanvas = TemperatureGraphWrapper.transform.Find("GraphCanvas").gameObject;
        temperatureGraphController = GraphCanvas.GetComponent<TemperatureGraphController>();
        root = gameObject.GetComponentInParent<WeatherControl2>();
    }

    public void OpenPanel()
    {
        if (GraphCanvas != null)
        {
            bool isActive = GraphCanvas.activeSelf;
            string city1 = root.getCityName();
            string city2 = temperatureGraphController.getCityName();
            city2 = city2.ToLower();
            temperatureGraphController.updateData(root.getWeatherData(), root.getCityName());
            /*
            if (isActive && city1 == city2)
            {
                GraphCanvas.SetActive(false);
            }
            else
            {
                temperatureGraphController.updateData(root.getWeatherData(), root.getCityName());
                GraphCanvas.SetActive(true);
            }
            */

            
        }
    }
}
