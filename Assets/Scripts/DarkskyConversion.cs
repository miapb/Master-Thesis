using UnityEngine;
using System.Collections;

static public class DarkskyConversion 
{
    static public float FahrenheitToCelsius(float f) {
        return  (f - 32) * 5 / 9;
    }

    static public float MphToKph(float mph)
    {
        return mph * 1.609344f;
    }
}
