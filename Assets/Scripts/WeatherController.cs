using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 	 	
using System.Threading.Tasks;	
using System.Net;
  using System;
  using System.IO;
  //using Assets;

public class WeatherControl : MonoBehaviour
{
    private const string API_KEY = "a8c66f1dd512768363c7aa05754a0d7a";
    public string CityId;
    private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes
    public GameObject SnowSystem;
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    
    void Start()
    {
        CheckSnowStatus();
    }

    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CheckSnowStatus();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }
    }

    public async void CheckSnowStatus()
    {
        bool snowing = (await GetWeather()).weather[0].main.Equals("Snow");
        if (snowing)
            SnowSystem.SetActive(true);
        else
            SnowSystem.SetActive(false);
    }

      private async Task<WeatherInfo> GetWeather()
      {
          HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}", CityId, API_KEY));
          HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string jsonResponse = reader.ReadToEnd();
          WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
          return info;
      }
}

[Serializable]
public class Weather{
    public int id;
    public string main;
}

[Serializable]
public class WeatherInfo{
    public int id;
    public string name;
    public List<Weather> weather;
}

