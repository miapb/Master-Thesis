using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Threading.Tasks;
using System.Net;
using System;
using System.IO;


public class CityOverviewController : MonoBehaviour
{
    private DarkskyForecast weather;
    private string cityName = "";

    [SerializeField] private TextMeshProUGUI cityNameText;
    [SerializeField] private GameObject[] hourly;
    [SerializeField] private GameObject currWeather;
    [SerializeField] private GameObject graph;
    [SerializeField] private EditMaterial terrainEdit;

    [SerializeField] private GameObject cloudSystem;
    [SerializeField] private GameObject rainSystem;
    [SerializeField] private GameObject sleetSystem;
    [SerializeField] private GameObject fogSystem;
    [SerializeField] private GameObject snowSystem;




    public void updateData(DarkskyForecast weather_in, string cityName_in)
    {
        // Update the data based on incoming data from the pressed button
        weather = weather_in;
        cityName = char.ToUpper(cityName_in[0]) + cityName_in.Substring(1);
        
        // Update the GUI to match the new data
        updateGUI();
    }

    public string getCityName()
    {
        return cityName;
    }

    public int getHourlyLength()
    {
        return hourly.Length;
    }

    public void updateGUI()
    {
        cityNameText.text = cityName;

        int currUnixTime = weather.currently.time;
        string formattedDate = UnixTimeToDateTime(currUnixTime).ToString("dd-MM-yyyy HH:mm");

        currWeather.GetComponent<CurrentlyController>().updateData(weather.currently);

        renderParticleSystem();

        if (weather.currently.icon == "snow")
        {
            terrainEdit.setSnow();
        }
        

        for (int i = 0; i < hourly.Length; i++)
        {
            hourly[i].GetComponent<HourlyController>().updateData(weather.hourly.data[i+1]);
        }

        //graph.GetComponent<WindowGraph>().updateData(weather);

    }

    private void renderParticleSystem()
    {
        
        bool snow = weather.currently.icon.Equals("snow");
        snowSystem.SetActive(snow);

        if (weather.currently.icon.Equals("cloudy") || weather.currently.icon.Equals("partly-cloudy-day") || weather.currently.icon.Equals("partly-cloudy-night"))
        {
            cloudSystem.SetActive(true);
        }

        bool rain = weather.currently.icon.Equals("rain");
        rainSystem.SetActive(rain);

        bool sleet = weather.currently.icon.Equals("sleet");
        sleetSystem.SetActive(sleet);

        bool fog = weather.currently.icon.Equals("fog");
        fogSystem.SetActive(fog);

    }

    private DateTime UnixTimeToDateTime(int unixtime)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
        return dtDateTime.AddSeconds(-dtDateTime.Second);
    }
}
