using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Threading.Tasks;
using System.Net;
using System;
using System.IO;
using TMPro;

public class WeatherControl2 : MonoBehaviour
{
    private const string API_KEY = "1e064654ec2105f13bb4beb282ea871a";
    public double longitude;
    public double latitude;
    public int time;

    private string formattedDate;
    private string currSummary;
    private string cityName;
    private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes

    [SerializeField] private GameObject SnowSystem;
    [SerializeField] private GameObject CloudSystem;
    [SerializeField] private BarGraphBarController TemperatureController;
    [SerializeField] private BarGraphBarController RainController;

    [SerializeField] private GameObject WindArrow;

    //[SerializeField] private GameObject SnowSphere;
    [SerializeField] private GameObject PartlyCloudyDay;
    [SerializeField] private GameObject PartlyCloudyNight;
    [SerializeField] private GameObject ClearDaySystem;
    [SerializeField] private GameObject ClearNightSystem;
    [SerializeField] private GameObject RainSystem;
    [SerializeField] private GameObject SleetSystem;
    [SerializeField] private GameObject FogSystem;
    [SerializeField] private GameObject WindSystem;

    [SerializeField] private TextMeshProUGUI cityNameText;
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private TextMeshProUGUI rainText;

    [SerializeField] private Image temperatureBackground;

    private DarkskyForecast weather = new DarkskyForecast();
    private DarkskyCurrent currentWeather;
    private DarkskyCurrent nextWeather;

    private float apiCheckCountdown = API_CHECK_MAXTIME;
    private bool initialized_ = false;

    private string typeActive = "rain";

    // Update is called once per frame
    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;

        if (apiCheckCountdown <= 0)
        {
            
            CheckWeatherStatus();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }
        
    }

    public bool initialized()
    {
        return initialized_;
    }

    public void initialize()
    {
        initialized_ = true;
    }

    public void setLatitude(double newLatitude)
    {
        latitude = newLatitude;
    }

    public void setLongitude(double newLongitude)
    {
        longitude = newLongitude;
    }

    public void SetBarType(string type)
    {
        typeActive = type;
    }

    public void setName(string newName)
    {
        cityName = newName;
        weather.name = newName;
    }

    public string getCityName()
    {
        return cityName;
    }
 
    public DarkskyForecast getWeatherData()
    {
        return weather;
    }

    private void updateWeatherIndices()
    {
        int index = (int)timeInstance;
        currentWeather = weather.hourly.data[index];
        nextWeather = weather.hourly.data[index < 48 ? index + 1 : 48];
    }

    public async void CheckWeatherStatus()
    {
        weather = await GetForecast();
        updateWeatherIndices();
        renderMarker();
    }

    public void renderMarker()
    {
        renderParticleSystem();
        renderCityName();
        renderTemperature();
        renderWindBearing();

        TemperatureController.setSideSize(0.5f);
        RainController.setSideSize(0.5f);

        //SnowSphere.SetActive(false);
        renderRainBar();
        renderTemperatureBar();

        //rainBarRendered = (currentWeather.precipType == "rain" && rainBarActive);
        //RainBar.SetActive(rainBarRendered);
        //SnowSphere.SetActive(currentWeather.precipType == "snow" && rainBarActive);

    }

    private void renderParticleSystem()
    {
        bool snowing = currentWeather.icon.Equals("snow");
        SnowSystem.SetActive(snowing && snowingActive);

        bool cloudy = currentWeather.icon.Equals("cloudy");
        CloudSystem.SetActive(cloudy && cloudyActive);
    
        bool partlyCloudyDay = currentWeather.icon.Equals("partly-cloudy-day");
        PartlyCloudyDay.SetActive(partlyCloudyDay && partlyCloudyDayActive);

        bool partlyCloudyNight = currentWeather.icon.Equals("partly-cloudy-night");
        PartlyCloudyNight.SetActive(partlyCloudyNight && partlyCloudyNightActive);

        bool clearDay = currentWeather.icon.Equals("clear-day");
        ClearDaySystem.SetActive(clearDay && clearDayActive);

        bool clearNight = currentWeather.icon.Equals("clear-night");
        ClearNightSystem.SetActive(clearNight && clearNight);

        bool rain = currentWeather.icon.Equals("rain");
        RainSystem.SetActive(rain && rainActive);

        bool sleet = currentWeather.icon.Equals("sleet");
        SleetSystem.SetActive(sleet && sleetActive);

        bool fog = currentWeather.icon.Equals("fog");
        FogSystem.SetActive(fog && fogActive);

        bool wind = currentWeather.icon.Equals("wind");
        WindSystem.SetActive(wind && windActive);
    }

    private void renderCityName()
    {
        cityNameText.text = char.ToUpper(cityName[0]) + cityName.Substring(1);
    }

    public void renderTemperature()
    {
        float temperature_celsius = DarkskyConversion.FahrenheitToCelsius(currentWeather.temperature);
        int rounded_temperature = (int)Math.Round(temperature_celsius);
        temperatureText.text = rounded_temperature.ToString();

        if (rounded_temperature > 0)
        {
            temperatureBackground.GetComponent<Image>().color = new Color32(204, 68, 75, 250);
        }
        else
        {
            temperatureBackground.GetComponent<Image>().color = new Color32(20, 92, 158, 250);
        }
    }

    public float timeInstance;
    private float currentFactor = 1;
    private float nextFactor = 0;

    public void AdjustTime(float newTime)
    {
        timeInstance = newTime;
        nextFactor = newTime - (float) Math.Truncate(newTime);
        currentFactor = 1 - nextFactor;
        updateWeatherIndices();
        renderMarker();
    }

    public void setRainBarActive(bool active)
    {
        RainController.setActive(active);
    }

    public void setTempActive(bool active)
    {
        TemperatureController.setActive(active);
    }

    public void setWindArrowActive(bool active)
    {
        WindArrow.SetActive(active);
    }

    bool snowingActive;
    bool cloudyActive;
    bool partlyCloudyDayActive;
    bool partlyCloudyNightActive;
    bool clearDayActive;
    bool clearNightActive;
    bool rainActive;
    bool sleetActive;
    bool fogActive;
    bool windActive;
    
    public void setSnowingActive(bool active)
    {
        snowingActive = active;
    }

    public void setCloudyActive(bool active)
    {
        cloudyActive = active;
    }

    public void setPartlyCloudyDayActive(bool active)
    {
        partlyCloudyDayActive = active;
    }

    public void setPartlyCloudyNightActive(bool active)
    {
        partlyCloudyNightActive = active;
    }

    public void setClearDayActive(bool active)
    {
        clearDayActive = active;
    }

    public void setClearNightActive(bool active)
    {
        clearNightActive = active;
    }

    public void setRainActive(bool active)
    {
        rainActive = active;
    }

    public void setSleetActive(bool active)
    {
        sleetActive = active;
    }

    public void setFogActive(bool active)
    {
        fogActive = active;
    }

    public void setWindActive(bool active)
    {
        windActive = active;
    }


    public void renderRainBar()
    {
        float precip_amount = (currentFactor*currentWeather.precipIntensity + nextFactor * nextWeather.precipIntensity) * 2500;

        double lightness = 1 - precip_amount/50 * (1-0.42);
        if(lightness < 0.42)
        {
            lightness = 0.42;
        }

        Color rainColor = ColorScale.ColorFromHSL(208.7 / 360, 0.7753, lightness);
        RainController.setValue(precip_amount / 10);
        RainController.setScaling(10);
        RainController.setUnit("mm");
        RainController.setColor(rainColor);
        //Color.HSVToRGB(0.55f, precip_amount / 50, 1);
    }

    private double mapPositiveTemperatureColor(double value)
    {
        double fromSource = 0;
        double toSource = 20;

        double fromTarget = 0;
        double toTarget = 30;
        double hue = (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        if(hue > toTarget)
        {
            hue = toTarget;
        }
        if (hue < fromTarget)
        {
            hue = toTarget;
        }
        return toTarget - hue;
    }


    public void renderTemperatureBar()
    {
        float interpolatedTemperatureFahrenheit = (currentFactor * currentWeather.temperature + nextFactor * nextWeather.temperature);
        float temperature_celsius = (interpolatedTemperatureFahrenheit - 32) * 5 / 9;
        int rounded_temperature = (int)Math.Round(temperature_celsius);

        Color temperatureColor = new Color(0, 0, 0, 0);
        if (rounded_temperature > 0)
        {
            double hue = mapPositiveTemperatureColor(rounded_temperature);
            temperatureColor = ColorScale.ColorFromHSL(hue/360.0, 1, 0.5);
        }
        else
        {
            temperatureColor = new Color32(20, 92, 158, 250);
        }

        double lightness = 1 - rounded_temperature / 50 * (1 - 0.42);
        if (lightness < 0.42)
        {
            lightness = 0.42;
        }
        TemperatureController.setValue(rounded_temperature * 0.5f);
        TemperatureController.setScaling(1);
        TemperatureController.setUnit("°C");
        TemperatureController.setColor(temperatureColor);
    }

    class ColorScale
    {
        public static Color ColorFromHSL(double h, double s, double l)
        {
            double r = 0, g = 0, b = 0;
            if (l != 0)
            {
                if (s == 0)
                    r = g = b = l;
                else
                {
                    double temp2;
                    if (l < 0.5)
                        temp2 = l * (1.0 + s);
                    else
                        temp2 = l + s - (l * s);

                    double temp1 = 2.0 * l - temp2;

                    r = GetColorComponent(temp1, temp2, h + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, h);
                    b = GetColorComponent(temp1, temp2, h - 1.0 / 3.0);
                }
            }
            return new Color((float)r, (float)g, (float)b);

        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;

            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }
    }

    /*
    public void renderSnowSphere()
    {
        Vector3 snow_scale = SnowSphere.transform.localScale;
        Vector3 snow_position = SnowSphere.transform.position;

        float precip_amount = (currentFactor * currentWeather.precipIntensity + nextFactor * nextWeather.precipIntensity) * 5000;
        float snowScale = precip_amount * 0.3f;
        snow_scale.y = snowScale;
        snow_scale.x = snowScale;
        snow_scale.z = snowScale;


        SnowSphere.transform.position = snow_position;
        SnowSphere.transform.localScale = snow_scale;
        Material snow_material = SnowSphere.GetComponent<Renderer>().material;
        snow_material.color = new Color32(255, 255, 255, 250);
        SnowSphere.SetActive(true);

    }
    */

    private void renderWindBearing()
    {
        Vector3 wind_arrow_scale = WindArrow.transform.localScale;
        float windSpeedValue = DarkskyConversion.MphToKph(weather.currently.windSpeed);
        if (windSpeedValue == 0)
        {
            WindArrow.SetActive(false);
        } 
        else
        {
            WindArrow.transform.localRotation = Quaternion.Euler(90, 0, 270 - weather.currently.windBearing);
            wind_arrow_scale.x = (windSpeedValue * 1.0f);
            WindArrow.transform.localScale = wind_arrow_scale;
        }
    }

    private async Task<DarkskyForecast> GetForecast()
    {
        string requestString = String.Format(System.Globalization.CultureInfo.InvariantCulture, "https://api.darksky.net/forecast/{0}/{1},{2}", API_KEY, latitude, longitude);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
        HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        DarkskyForecast info = JsonUtility.FromJson<DarkskyForecast>(jsonResponse);
        string tempName = weather.name;
        weather = info;
        weather.name = tempName;
        return weather;
    }

} 