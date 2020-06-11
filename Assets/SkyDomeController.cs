using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyDomeController : MonoBehaviour
{
    MeshRenderer meshRenderer;
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject moon;
    [SerializeField] private GameObject stars;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        setExposure(0);
    }

    public void setExposure(float value)
    {
        System.DateTime sliderTime = System.DateTime.Now.AddMinutes(value * 60);
        int hour = sliderTime.Hour;
        float minutes = sliderTime.Minute;
        float timeOfDay = hour + minutes / 60f;
        float exposure = 0.64f;
        if (hour > 6 && hour < 18)
        {
            sun.SetActive(true);
            moon.SetActive(false);
            stars.SetActive(false);
        } else
        {
            sun.SetActive(false);
            moon.SetActive(true);
            stars.SetActive(true);

        }

        if (timeOfDay < 12)
        {
            exposure = timeOfDay / 12f;

        } else
        {
            exposure = 1 - (timeOfDay - 12) / 12f;
    
        }
        skyboxMaterial.SetFloat("_Exposure", exposure);
        skyboxMaterial.SetFloat("Exposure", exposure);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
