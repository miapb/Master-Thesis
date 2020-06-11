using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class HourlyController : MonoBehaviour
{
    [SerializeField] private GameObject CloudSystem;
    [SerializeField] private GameObject PartlyCloudyDaySystem;
    [SerializeField] private GameObject PartlyCloudyNightSystem;
    [SerializeField] private GameObject ClearDaySystem;
    [SerializeField] private GameObject ClearNightSystem;
    [SerializeField] private GameObject SnowSystem;
    [SerializeField] private GameObject SleetSystem;
    [SerializeField] private GameObject RainSystem;
    [SerializeField] private GameObject FogSystem;
    [SerializeField] private GameObject WindSystem;

    [SerializeField] private TextMeshPro temperature;
    [SerializeField] private TextMeshPro time;

    private DarkskyCurrent weather;

    public void updateData(DarkskyCurrent weather_in)
    {
        weather = weather_in;

        renderGUI();
    }

    public void renderGUI()
    {
        float temp_celsius = (weather.temperature - 32) * 5 / 9;
        int rounded_temp = (int)Math.Round(temp_celsius);

        string getIcon = weather.icon;
        RenderParticleSystem(getIcon);

        int unixTime = weather.time;
        string hour = UnixTimeToDateTime(unixTime).ToString("HH");

        temperature.text = rounded_temp.ToString() + "°C";
        time.text = hour;
    }

    private DateTime UnixTimeToDateTime(int unixtime)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
        return dtDateTime.AddSeconds(-dtDateTime.Second);
    }

    private void RenderParticleSystem(string icon)
    {
        bool cloudy = icon.Equals("cloudy");
        CloudSystem.SetActive(cloudy);

        bool partlyCloudy = icon.Equals("partly-cloudy-day");
        PartlyCloudyDaySystem.SetActive(partlyCloudy);

        bool partlyCloudyNight = icon.Equals("partly-cloudy-night");
        PartlyCloudyNightSystem.SetActive(partlyCloudyNight);

        bool clearDay = icon.Equals("clear-day");
        ClearDaySystem.SetActive(clearDay);

        bool clearNight = icon.Equals("clear-night");
        ClearNightSystem.SetActive(clearNight);

        bool snow = icon.Equals("snow");
        SnowSystem.SetActive(snow);

        bool sleet = icon.Equals("sleet");
        SleetSystem.SetActive(sleet);

        bool rain = icon.Equals("rain");
        RainSystem.SetActive(rain);

        bool fog = icon.Equals("fog");
        FogSystem.SetActive(fog);

        bool wind = icon.Equals("wind");
        WindSystem.SetActive(wind);
    }

}
