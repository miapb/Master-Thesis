using UnityEngine;
using System.Collections;
using System;

[Serializable] public class DarkskyForecast
{
    [NonSerialized] public string name;
    public float longitude;
    public float latitude;
    public DarkskyCurrent currently;
    public DarkskyHourly hourly;
}
