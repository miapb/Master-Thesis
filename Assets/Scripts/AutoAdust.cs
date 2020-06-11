using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoAdust : MonoBehaviour
{
    public Slider mSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mSlider.value += 0.03f;
        if(mSlider.value >= 48)
        {
            mSlider.value = 0;
        }
    }
}
