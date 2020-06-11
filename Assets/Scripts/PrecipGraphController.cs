using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PrecipGraphController : MonoBehaviour
{
    private DarkskyForecast weather;
    private string cityName = "";
    [SerializeField] private WindowGraph graph;
    [SerializeField] private TextMeshProUGUI cityNameText;

    public void updateData(DarkskyForecast weather_in, string cityName_in)
    {
        // Update the data based on incoming data from the pressed button
        weather = weather_in;
        cityName = char.ToUpper(cityName_in[0]) + cityName_in.Substring(1);

        // Update the GUI to match the new data
        updateGUI();
    }

    public void updateGUI()
    {

        cityNameText.text = cityName;
        List<int> valueList = new List<int>();
        for (int i = 0; i < weather.hourly.data.Length; i++)
        {
            float precipValue = weather.hourly.data[i].precipIntensity;
            int roundedPrecipValue= (int)Math.Round(precipValue);
            valueList.Add(roundedPrecipValue);
        }

        graph.updateGraph(valueList, cityName);
    }

    public string getCityName()
    {
        return cityName;
    }
}
