using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureController : MonoBehaviour
{
    public Text m_Text;

    private void Start()
    {
        //Check that the Text is attached in the Inspector
        if (m_Text != null)
            //Change the Text to show the GameObject's name
            m_Text.text = "GameObject Name : " + gameObject.ToString();
    }
}
