using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarGraphBarController : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject bar;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color hoverColor;

    private Color color;
    private Renderer renderer;

    private float sideSize = 1;
    private float scale = 1;
    private string unit = "";
    private int precision = 1;

    // Start is called before the first frame update
    void Start()
    {

        renderer = bar.GetComponent<Renderer>();
        color = new Color(1, 1, 0);
    }
    public void setActive(bool active)
    {
        bar.SetActive(active);
    }

    public void setColor(Color color) {
        renderer = bar.GetComponent<Renderer>();
        renderer.material.color = color;
    }
    public void setScaling(float newScale) {
        scale = newScale;
    }

    public void setUnit(string newUnit) {
        unit = newUnit;
    }

    public void setPrecision(int n)
    {
        precision = n;
    }

    public void setCamera(Camera newCamera)
    {
        FaceFront frontFacer = textObject.GetComponent<FaceFront>();
        frontFacer.setCamera(newCamera);
    }

    public void setSideSize(float newSideSize)
    {
        sideSize = newSideSize;
    }
    public void setValue(float value)
    {
        bar.transform.localScale = new Vector3(sideSize, value*scale, sideSize);
        bar.transform.localPosition = new Vector3(0, value*scale/2, 0);
        text.text = value.ToString("n" + precision) + " " + unit;
        Vector3 barScale = bar.transform.localScale;
        Vector3 barPos = bar.transform.localPosition;
        textObject.transform.localPosition = new Vector3(barPos.x, barPos.y + 2 + barScale.y/2, barPos.z);
    }

    public void setHoverState(bool hovered)
    {
        textObject.SetActive(hovered);
    }

    void Update()
    {

    }
}
