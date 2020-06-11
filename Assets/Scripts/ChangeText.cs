using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Text myText = GameObject.Find("Canvas/Temperature").GetComponent<Text>();
        myText.text = "The text is changed!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
