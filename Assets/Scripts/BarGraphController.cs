using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mapbox.Examples;

public class BarGraphController : MonoBehaviour
{
    // Start is called before the first frame update
    private class City {
        public string name;
        public int temprature;
        public int rain;
        public int snow;

        public GameObject temperatureWrapper;
        public List<GameObject> temperatureBars;

        public GameObject rainWrapper;
        public List<GameObject> rainBars;

        public GameObject windWrapper;
        public List<GameObject> windBars;


        public List<GameObject> snowBars;
        public DarkskyForecast forecast;

        public City(DarkskyForecast forecast, int temprature, int rain, int snow)
        {
            this.temperatureBars = new List<GameObject>();
            this.rainBars = new List<GameObject>();
            this.windBars = new List<GameObject>();

            this.snowBars = new List<GameObject>();
            this.forecast = forecast;
            this.name = forecast.name;
            this.temprature = temprature;
            this.rain = rain;
            this.snow = snow;
        }
    }
    private List<GameObject> cityObjects = new List<GameObject>();
    private List<City> cities = new List<City>();

    [SerializeField] private float margin = 2;
    [SerializeField] private float width = 30;

    [SerializeField] private GameObject textPrefab;
    //[SerializeField] private GameObject timeInstencePrefab;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private SpawnOnMap spawner;
    [SerializeField] private Camera camera;

    private float scale = 1f;
    private float sideSize = 1;

    private bool initialized = false;
    private List<DarkskyForecast> weather;

    public void Initialize()
    {
        weather = spawner.GetWeather();
        for(int i = 0; i < weather.Count; i++)
        {
            cities.Add(new City(weather[i], 20, 10, 50));
        }
        /*
        cities.Add(new City("Trondheim", 20, 10, 50));
        cities.Add(new City("Stavanger", 5, 1, 2));
        cities.Add(new City("Bergen", 4, 3, 1));
        cities.Add(new City("Drammen", 0, 10, 1));
        cities.Add(new City("Oslo", 10, 20, 30));
        cities.Add(new City("Trondheim", 20, 10, 50));
        cities.Add(new City("Stavanger", 5, 1, 2));
        cities.Add(new City("Bergen", 4, 3, 1));
        cities.Add(new City("Drammen", 0, 10, 1));
        */

        int typeCount = 3;
        float freeSpace = width - (typeCount * cities.Count * sideSize) - 2*margin;
        float spacing = freeSpace / (cities.Count - 1);

        for (int i = 0; i < cities.Count; i++)
        {
            City city = cities[i];

            GameObject cityObject = new GameObject();
            cityObject.name = city.name;

            cityObject.transform.parent = transform;
            cityObject.transform.localPosition = new Vector3(i * (spacing + (sideSize * typeCount)) + sideSize / 2 + margin, 0, 0.5f) * scale;
            cityObject.transform.localRotation = new Quaternion();
            cityObject.transform.localScale = new Vector3(1, 1, 1);

            GameObject cityNameText = Instantiate(textPrefab);
            cityNameText.transform.parent = cityObject.transform;
            cityNameText.transform.localPosition = new Vector3(0, 0.05f, -3);
            cityNameText.transform.localRotation = Quaternion.Euler(90, 45, 0);
            cityNameText.transform.localScale = new Vector3(1, 1, 1);

 
            cityNameText.GetComponent<TextMeshPro>().text = char.ToUpper(city.name[0]) + city.name.Substring(1);

            city.temperatureWrapper = new GameObject();
            city.temperatureWrapper.transform.parent = cityObject.transform;
            city.temperatureWrapper.name = "Temperature Bars";
            city.temperatureWrapper.transform.localPosition = new Vector3(0, 0, 0);
            city.temperatureWrapper.transform.localRotation = Quaternion.Euler(0, 0, 0);
            city.temperatureWrapper.transform.localScale = new Vector3(1, 1, 1);

            city.rainWrapper = new GameObject();
            city.rainWrapper.transform.parent = cityObject.transform;
            city.rainWrapper.name = "Rain Bars";
            city.rainWrapper.transform.localPosition = new Vector3(1, 0, 0);
            city.rainWrapper.transform.localRotation = Quaternion.Euler(0, 0, 0);
            city.rainWrapper.transform.localScale = new Vector3(1, 1, 1);

            city.windWrapper = new GameObject();
            city.windWrapper.transform.parent = cityObject.transform;
            city.windWrapper.name = "Wind Bars";
            city.windWrapper.transform.localPosition = new Vector3(2, 0, 0);
            city.windWrapper.transform.localRotation = Quaternion.Euler(0, 0, 0);
            city.windWrapper.transform.localScale = new Vector3(1, 1, 1);

            for (int j = 0; j < city.forecast.hourly.data.Length; j++)
            {
                float temp = DarkskyConversion.FahrenheitToCelsius(city.forecast.hourly.data[j].temperature);
                city.temperatureBars.Add(CreateBar(
                    temp,
                    "Temperature",
                    "°C",
                    1,
                    1f,
                    city.temperatureWrapper.transform,
                    temp > 4 ? new Color(1, 0, 0) : new Color(58f/255f, 189f/255f, 203f/256f),
                    j, 0));
                city.rainBars.Add(CreateBar(
                    city.forecast.hourly.data[j].precipIntensity*100,
                    "Rain",
                    "mm",
                    2,
                    100f,
                    city.rainWrapper.transform,
                    city.forecast.hourly.data[j].precipType == "rain" ? new Color(0.1f, 0.1f, 1) : new Color(0.9f, 0.9f, 0.9f),
                    j, 0));
                city.windBars.Add(CreateBar(
                    DarkskyConversion.MphToKph(city.forecast.hourly.data[j].windSpeed),
                    "Wind",
                    "kmh",
                    3,
                    1f,
                    city.windWrapper.transform,
                    new Color(0, 1, 0),
                    j, 0));
                /*
                city.snowBars.Add(CreateBar(
                    city.forecast.hourly.data[j].precipIntensity*100,
                    "Snow",
                    cube.transform,
                    new Color(0.9f, 0.9f, 0.9f),
                    j, 2));
                */
            }
            cityObjects.Add(cityObject);
        }
        initialized = true;
    }

    bool displayRain  = true;
    public void setRain(bool value)
    {
        displayRain = value;
    }

    bool displayTemperature = true;
    public void setTemperature(bool value)
    {
        displayTemperature = value;
    }

    bool displayWind = true;
    public void setWind(bool value)
    {
        displayWind = value;
    }

    private GameObject CreateBar(float value, string name, string unit, int precision, float scale, Transform parent, Color color, int zPosition, int xPosition)
    {
        //GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject bar = Instantiate(barPrefab);
        bar.transform.parent = parent;
        bar.name = name;
        BarGraphBarController barController = bar.GetComponent<BarGraphBarController>();
        barController.setColor(color);
        barController.setCamera(camera);
        barController.setUnit(unit);
        barController.setPrecision(precision);
        barController.setScaling(scale);
        barController.setValue(value);

        bar.transform.localScale = new Vector3(sideSize, 1, sideSize);
        bar.transform.localPosition = new Vector3(xPosition, 0, zPosition);
        bar.transform.localRotation = new Quaternion();
        //Renderer renderer = bar.GetComponent<Renderer>();
        //renderer.material.color = color;
        bar.layer = 5;

        //MeshRenderer mesh = bar.GetComponent<MeshRenderer>();
        //mesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return bar;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;
        weather = spawner.GetWeather();

        UpdateBars();
    }

    private bool ValidData()
    {
        for(int i = 0; i < weather.Count; i++)
        {
            if (weather[i].name == null || weather[i].hourly == null) return false;
        }
        return true;
    }

    private bool done = false;
    private void UpdateBars()
    {
        if (!ValidData()) return;
        done = true;


        for (int i = 0; i < cityObjects.Count; i++)
        {
            int typeCount = 0;
            City city = cities[i];

            city.temperatureWrapper.SetActive(displayTemperature);
            city.temperatureWrapper.transform.localPosition = new Vector3(typeCount, 0, 0);
            if (displayTemperature) typeCount++;

            city.rainWrapper.SetActive(displayRain);
            city.rainWrapper.transform.localPosition = new Vector3(typeCount, 0, 0);
            if (displayRain) typeCount++;

            city.windWrapper.SetActive(displayWind);
            city.windWrapper.transform.localPosition = new Vector3(typeCount, 0, 0);
            if (displayWind) typeCount++;

            float freeSpace = width - (typeCount * cities.Count * sideSize) - 2*margin;
            float spacing = freeSpace / (cities.Count - 1);

            cityObjects[i].transform.localPosition = new Vector3(i * (spacing + (sideSize*typeCount)) + sideSize/2 + margin, 0, 0.5f) * scale;
        }


    }
}
