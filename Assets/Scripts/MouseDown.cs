using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseDown : MonoBehaviour
{
 
    void OnMouseDown()
    {
        PrecipGraphInteracter interacter = GetComponentInParent<PrecipGraphInteracter>();
        interacter.OpenPanel();
    }
}
