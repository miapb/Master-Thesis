using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherDomeController : MonoBehaviour
{
    [SerializeField] private GameObject cloudSystem;
    [SerializeField] private GameObject rainSystem;
    [SerializeField] private GameObject sleetSystem;
    [SerializeField] private GameObject fogSystem;
    [SerializeField] private GameObject snowSystem;

    public void defaultWeatherDome()
    {

        snowSystem.SetActive(false);
        cloudSystem.SetActive(false);
        rainSystem.SetActive(false);
        sleetSystem.SetActive(false);
        fogSystem.SetActive(false);
    }
}
