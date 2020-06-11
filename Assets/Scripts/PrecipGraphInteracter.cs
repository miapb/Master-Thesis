using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrecipGraphInteracter : MonoBehaviour
{
    private GameObject GraphCanvas;
    private PrecipGraphController precipGraphController;
    private WeatherControl2 root;


    void Start()
    {
        GameObject PrecipGraphWrapper = GameObject.Find("PrecipGraphWrapper");
        GraphCanvas = PrecipGraphWrapper.transform.Find("GraphCanvas").gameObject;
        precipGraphController = GraphCanvas.GetComponent<PrecipGraphController>();
        root = gameObject.GetComponentInParent<WeatherControl2>();
    }

    public void OpenPanel()
    {
        if (GraphCanvas != null)
        {
            bool isActive = GraphCanvas.activeSelf;
            string city1 = root.getCityName();
            string city2 = precipGraphController.getCityName();
            city2 = city2.ToLower();

            if (isActive && city1 == city2)
            {
                GraphCanvas.SetActive(false);
            }
            else
            {
                precipGraphController.updateData(root.getWeatherData(), root.getCityName());
                GraphCanvas.SetActive(true);
            }

        }
    }
}
