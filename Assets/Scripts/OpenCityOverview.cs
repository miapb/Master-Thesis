using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject CityOverview;

    public void openPanel()
    {
        if (CityOverview != null)
        {
            bool isActive = CityOverview.activeSelf;
            CityOverview.SetActive(!isActive);
        }
    }
}
