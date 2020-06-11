using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowSliderText : MonoBehaviour
{
    [SerializeField] private TextMeshPro sliderText;
    [SerializeField] private TextMeshPro startTime;
    [SerializeField] private TextMeshPro endTime;

    private void Start()
    {
        textUpdate(0);
    }


    public void textUpdate(float value)
    {
        System.DateTime sliderTime = System.DateTime.Now.AddMinutes(value*60);
        sliderText.text = sliderTime.ToString("dddd HH:mm");

        System.DateTime now = System.DateTime.Now;
        startTime.text = now.ToString("dddd HH:mm");
        endTime.text = now.AddHours(48).ToString("dddd HH:mm");

    }
}
