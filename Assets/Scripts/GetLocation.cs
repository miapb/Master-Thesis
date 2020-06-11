
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using System.Threading.Tasks;
using System.Net;
using System;
using System.IO;

public class GetLocation : MonoBehaviour
{

    private const string API_KEY = "09d472f016364e06bbeb2841e08226ac";

    public async Task<OpenCageData> GetCityLocation(string city)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.opencagedata.com/geocode/v1/json?key={0}&q={1}", API_KEY, city));
        HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        OpenCageData info = JsonUtility.FromJson<OpenCageData>(jsonResponse);
        return info;
    }
}

[Serializable]
public class LocationResult
{
    public string city;

}

[Serializable]
public class CoordinateResult
{
    public float lat;
    public float lng;
}

[Serializable]
public class OpenCageResultEntry
{
    public LocationResult components;
    public CoordinateResult geometry;
}


[Serializable]
public class OpenCageData
{
    public OpenCageResultEntry[]  results;
 
}
