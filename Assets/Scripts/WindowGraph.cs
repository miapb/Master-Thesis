using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class WindowGraph : MonoBehaviour
{
    public class GraphHelpers
    {

    }
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int numPoints = 6;
    private DarkskyForecast weather;
    private List<int> valueList;

    //Size between the points on x-axis
    [SerializeField] private float xSize = 50f;
    //Height of graph
    [SerializeField] private float graphHeight;
    //Top of our graph, max input valueList
    [SerializeField] private float yMax = 100f;
    [SerializeField] private float animationSpeed = 100f;
    [SerializeField] private TextMeshProUGUI xLabel;
    [SerializeField] private TextMeshProUGUI yLabel;
    [SerializeField] private string xLabelText;
    [SerializeField] private string yLabelText;

    private int min = 0;
    private int max = 0;
    private float zeroPositionY_current = 0;
    private float zeroPositionY_target = 0;

    private bool initialized = false;

    [SerializeField] private GameObject GraphCanvas;

    private RectTransform graphContainer;
    private RectTransform lableTemplateX;
    private RectTransform lableTemplateY;
    List<GameObject> xLines = new List<GameObject>();
    List<GameObject> yLines = new List<GameObject>();

    RectTransform xAxis;
    RectTransform yAxis;
    RectTransform grid;


    GameObject zeroLine;

    public class CityGraph
    {
        public List<int> valueList;
        public string name;
        private int numPoints;
        public int myMin = 0;
        public int myMax = 0;
        public float myYMax = 15f;

        public int min = 0;
        public int max = 0;
        public float yMax = 15f;

        private Sprite circleSprite;
        float animationSpeed;
        public GameObject cityGraphContainer;


        private RectTransform graphContainer;

        private List<GameObject> circles = new List<GameObject>();
        private List<GameObject> lines = new List<GameObject>();

        public CityGraph(
            string name, 
            List<int> valueList, 
            int numPoints, 
            RectTransform graphContainer, 
            Sprite circleSprite,
            float animationSpeed)
        {
            this.name = name;
            this.valueList = valueList;
            this.numPoints = numPoints;
            this.graphContainer = graphContainer;
            this.circleSprite = circleSprite;
            this.animationSpeed = animationSpeed;
            cityGraphContainer = new GameObject(name, typeof(RectTransform));
            RectTransform rectTransform = cityGraphContainer.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(1, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            cityGraphContainer.transform.SetParent(graphContainer.transform, false);
            rectTransform.offsetMin = new Vector2(-7, 0);//left-bottom
            rectTransform.offsetMax = new Vector2(0, 0);//right-top

            myMax = -200;
            myMin = 200;
            for (int i = 0; i < numPoints; i++)
            {
                int value = valueList[i];
                if (valueList[i] > myMax)
                {
                    myMax = value;
                }
                if (valueList[i] < myMin)
                {
                    myMin = value;
                }
            }
            myMax += 1;
            myMin -= 1;
            if (myMin > -1)
            {
                myMin = -1;
            }
            myYMax = myMax - myMin;
        }

        public void updateData(List<int> valueList_in)
        {
            valueList = valueList_in;
        }

        private GameObject CreateLine(Vector2 dotPositionA, Vector2 dotPositionB, string name, Transform parent, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(Image));
            gameObject.transform.SetParent(parent, false);
            gameObject.GetComponent<Image>().color = color;

            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);

            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }

        private void UpdateDotConnection(int i, Vector2 dotPositionA, Vector2 dotPositionB)
        {
            RectTransform rectTransform = lines[i].GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        }

        private GameObject CreateCircle(Vector2 anchoredPosition)
        {
            GameObject gameObject = new GameObject("circle", typeof(Image));
            gameObject.transform.SetParent(cityGraphContainer.transform, false);
            gameObject.GetComponent<Image>().sprite = circleSprite;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
            return gameObject;
        }

        public void createPoints()
        {
            float graphHeight = graphContainer.sizeDelta.y;

            for (int i = 0; i < valueList.Count; i++)
            {
                float xSize = graphContainer.sizeDelta.x / numPoints;
                float xPosition = xSize / 2 + i * xSize;
                float yPosition = (valueList[i] / yMax) * graphHeight;

                GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
                circles.Add(circleGameObject);
            }

            GameObject lastCircleGameObject = null;
            for (int i = 0; i < valueList.Count; i++)
            {
                float xSize = graphContainer.sizeDelta.x / numPoints;
                float xPosition = xSize / 2 + i * xSize;
                float yPosition = (valueList[i] / yMax) * graphHeight;

                if (lastCircleGameObject != null)
                {
                    lines.Add(
                        CreateLine(
                            lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition,
                            circles[i].GetComponent<RectTransform>().anchoredPosition,
                            "dotConnection",
                            cityGraphContainer.transform,
                            new Color(1, 1, 1, .5f)));
                }
                lastCircleGameObject = circles[i];
            }

            for (int i = 0; i < circles.Count; i++)
            {
                RectTransform rectTransform = circles[i].GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }

        float mapYPosition(int value)
        {
            float graphHeight = graphContainer.sizeDelta.y;
            return (graphHeight / 2 - (-graphHeight / 2)) * (value - min) / (max - min) + (-graphHeight / 2);
        }



        public void updateCircles()
        {


            for (int i = 0; i < circles.Count; i++)
            {
                RectTransform rectTransform = circles[i].GetComponent<RectTransform>();
                float xSize = graphContainer.sizeDelta.x / numPoints;
                float xPosition = xSize / 2 + i * xSize;
                float yPosition = mapYPosition(valueList[i]);
                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, new Vector2(xPosition, yPosition), animationSpeed * Time.deltaTime);

                if (valueList[i] > 0)
                {
                    circles[i].GetComponent<Image>().color = new Color32(255, 0, 0, 250);
                }
                else
                {
                    circles[i].GetComponent<Image>().color = new Color32(0, 0, 250, 250);
                }
            }


            for (int i = 0; i < lines.Count; i++)
            {
                UpdateDotConnection(i, circles[i].GetComponent<RectTransform>().anchoredPosition, circles[i + 1].GetComponent<RectTransform>().anchoredPosition);
            }

            //UpdateZeroLine();
        }
    }

    List<CityGraph> cities = new List<CityGraph>();

    private void updateCity(List<int> valueList_in, string name)
    {
        bool foundCity = false;
        foreach (CityGraph city in cities) {
            if(city.name == name)
            {
                foundCity = true;
                Destroy(city.cityGraphContainer);
                cities.Remove(city);

                break;
            }
        }
        if (!foundCity)
        {
            CityGraph city = new CityGraph(name, valueList_in, numPoints, graphContainer, circleSprite,animationSpeed);
            city.createPoints();
            cities.Add(city);
        }
        max = -200;
        min = 200;

        foreach (CityGraph city in cities)
        {

            if (city.myMax > max)
            {
                max = city.myMax;
            }
            if (city.myMin < min)
            {
                min = city.myMin;
            }
     
        }
        foreach (CityGraph city in cities)
        {
            city.max = max;
            city.min = min;
            city.yMax = yMax;
        }



    }

    public void updateGraph(List<int> valueList_in, string name)
    {
        if (!initialized)
        {
            Initialize();
        }
        valueList = valueList_in;
        updateCity(valueList_in, name);
        renderGUI();
    }

    private void renderGUI()
    {
        GraphCanvas.SetActive(cities.Count > 0);

        foreach (CityGraph city in cities)
        {
            city.updateCircles();
        }
    }

    private void Update()
    {
        renderGUI();
    }

    private float mapYPosition(int value)
    {
        return (graphHeight / 2 - (-graphHeight/2)) * (value - min) / (max - min) + (-graphHeight/2);
    }

    private void Initialize()
    {
        if (!initialized)
        {
            /*
            GameObject graphContainerObject = new GameObject("graphContainer");
            graphContainer = graphContainerObject.GetComponent<RectTransform>();
            graphContainer.sizeDelta = new Vector2(1000, 900);
            //graphContainer.transform.position = rootPosition;
            //graphContainer.transform.parent = hostTransform;
            //graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            */

            xLabel.text = xLabelText;
            yLabel.text = yLabelText;

            graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            xAxis = graphContainer.Find("xAxis").GetComponent<RectTransform>();
            grid = graphContainer.Find("grid").GetComponent<RectTransform>();

            valueList = new List<int>();
            for (int i = 0; i < numPoints; i++)
            {
                valueList.Add(0);
            }
            CreateGraph(valueList);
          
            initialized = true;
        }

    }

    private void Awake()
    {
        if (!initialized)
        {
            Initialize();
        }
    }



    

    private void CreateGraph(List<int> valueList)
    {
        //Height of graph
        graphHeight = graphContainer.sizeDelta.y;

        for (int j = 0; j < yMax; j++) {
            CreateLine(
                new Vector2(0, -graphHeight / 2 + (j * graphHeight / yMax)),
                new Vector2(800, -graphHeight / 2 + (j * graphHeight / yMax)),
                "yGrid" + j,
                grid,
                new Color(0.7f, 0.7f, 0.7f, 0.5f));
        }

        for (int j = 0; j < yMax; j++)
        {
            CreateLine(
                new Vector2(0, -graphHeight / 2 + (j * graphHeight / yMax)),
                new Vector2(10, -graphHeight / 2 + (j * graphHeight / yMax)),
                "yTick" + j,
                yAxis,
                new Color(0.1f, 0.1f, 0.1f, 1f));
        }

        for (int i = 0; i < valueList.Count; i++)
        {
            float xSize = graphContainer.sizeDelta.x / numPoints;
            float xPosition = xSize/2 + i * xSize;
            float yPosition = (valueList[i] / yMax) * graphHeight;

            CreateLine(
                new Vector2(xPosition, 0),
                new Vector2(xPosition, 10),
                "xTick" + i,
                xAxis,
                new Color(0.1f, 0.1f, 0.1f, 1f)
                );
            CreateLine(
                new Vector2(xPosition, -graphHeight / 2),
                new Vector2(xPosition, graphHeight / 2),
                "xGrid"+i,
                grid,
                new Color(0.7f, 0.7f, 0.7f, 0.5f)
                );

        }
        
        zeroLine = CreateLine(
            new Vector2(0, 0),
            new Vector2(800, 0),
            "zeroLine",
            xAxis,
            new Color(1, 1, 1, .5f));


        xAxis.SetAsLastSibling();

    }


    private GameObject CreateLine(Vector2 dotPositionA, Vector2 dotPositionB, string name, RectTransform parent, Color color)
    {
        GameObject gameObject = new GameObject(name, typeof(Image));
        gameObject.transform.SetParent(parent, false);
        gameObject.GetComponent<Image>().color = color;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0.5f);
        rectTransform.anchorMax = new Vector2(0, 0.5f);

        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
        return gameObject;
    }

    private void UpdateZeroLine()
    {
        zeroPositionY_target = mapYPosition(0);
        zeroPositionY_current = Mathf.MoveTowards(zeroPositionY_current, zeroPositionY_target, animationSpeed * Time.deltaTime);

        Vector2 dotPositionA = new Vector2(0, zeroPositionY_current);
        Vector2 dotPositionB = new Vector2(800, zeroPositionY_current);

        RectTransform rectTransform = zeroLine.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        xAxis.sizeDelta = new Vector2(distance, 3f);
        xAxis.anchoredPosition = dotPositionA;// + dir * distance * .5f;
        xAxis.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
      
        zeroLine.SetActive(Math.Abs(zeroPositionY_current) < graphHeight/2);
    }
}
