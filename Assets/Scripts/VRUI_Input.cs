using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;
using TMPro;

public class VRUI_Input : MonoBehaviour
{
    private LaserPointer laserPointer;
    //private  trackedController;
    public SteamVR_Input_Sources RightInputSource = SteamVR_Input_Sources.RightHand;
    [SerializeField] private GameObject MainMenu;
    private bool mainMenuActive = false;
    private bool alreadyClicked = false;
    Dropdown previousDropdown = null;
    Dropdown previouslyOpenedDropdown = null;
    public SteamVR_Action_Boolean MenuToggle;

    [SerializeField] private FallbackLaserPointer fallbackLaserPointer;

    private void OnEnable()
    {
        laserPointer = GetComponent<LaserPointer>();
        MenuToggle.AddOnStateDownListener(ToggleMainMenu, RightInputSource);
        
        laserPointer.PointerIn -= HandlePointerIn;
        laserPointer.PointerIn += HandlePointerIn;
        laserPointer.PointerOut -= HandlePointerOut;
        laserPointer.PointerOut += HandlePointerOut;
        

        fallbackLaserPointer.PointerIn -= HandlePointerIn;
        fallbackLaserPointer.PointerIn += HandlePointerIn;
        fallbackLaserPointer.PointerOut -= HandlePointerOut;
        fallbackLaserPointer.PointerOut += HandlePointerOut;


        /*
        trackedController = GetComponent<SteamVR_TrackedController>();
        if (trackedController == null)
        {
            trackedController = GetComponentInParent<SteamVR_TrackedController>();
        }
        trackedController.TriggerClicked -= HandleTriggerClicked;
        trackedController.TriggerClicked += HandleTriggerClicked;
         */
    }
    public void ToggleMainMenu(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger");
        mainMenuActive = !mainMenuActive;
        MainMenu.SetActive(mainMenuActive);
    }


    private void Update()
    {
        var triggerValue = SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource);

        if (triggerValue > 0.5)//&& !alreadyClicked)
        {
            HandleTriggerClicked();
            alreadyClicked = true;
        }
        if (triggerValue < 0.2)
        {
            alreadyClicked = false;
        }
    }



    private void HandleTriggerClicked()
    {
        GameObject clickedObject = EventSystem.current.currentSelectedGameObject;
        if (!clickedObject) return;
        if (clickedObject.tag == "Scrollbar")
        {
            Scrollbar scrollbar = clickedObject.GetComponent<Scrollbar>();
            Debug.Log(laserPointer.localHit);
            scrollbar.value = 1 - (laserPointer.localHit.y * -1) / 150;
        }
        if (clickedObject.tag == "Slider")
        {
            Slider slider = clickedObject.GetComponent<Slider>();
            Debug.Log(laserPointer.localHit);
            float width = 240f;
            float x = laserPointer.localHit.x + width/2;
            float percentage = x / width;
            slider.value = percentage * slider.maxValue;
            //slider.value = 1 - ((laserPointer.localHit.x * -1) /250)*48             ;
        }
        //if (alreadyClicked) return;
        if (clickedObject.tag == "Dropdown" && !alreadyClicked)
        {
            if (previouslyOpenedDropdown != null)
            {
                previouslyOpenedDropdown.Hide();
            }

            previouslyOpenedDropdown = clickedObject.GetComponent<Dropdown>();
        }

        if (clickedObject != null)
        {
            if (!alreadyClicked || clickedObject.tag == "ContinousButton")
            {
                ExecuteEvents.Execute(clickedObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
    }

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        var button = e.target.GetComponent<Button>();
        var toggle = e.target.GetComponent<Toggle>();
        var scrollbar = e.target.GetComponent<Scrollbar>();
        var dropdown = e.target.GetComponent<Dropdown>();
        var mark_option = e.target.tag == "DropdownOption";
        var scrollbar2 = e.target.tag == "Scrollbar";

        var sliderComponent = e.target.GetComponent<Slider>();
        var scrollbarHandle = e.target.tag == "ScrollbarHandle";
        var slider = e.target.tag == "Slider";

        var barGraph = e.target.tag == "BarGraphBar";
        if (barGraph)
        {
            var bar = e.target.GetComponentInParent<BarGraphBarController>();
            bar.setHoverState(true);
        }

        var sliderHandle = e.target.tag == "SliderHandle";

        if (scrollbar2)
        {
            scrollbar = e.target.GetComponent<Scrollbar>();
        }
        if (scrollbarHandle)
        {
            scrollbar = e.target.GetComponentInParent<Scrollbar>();
        }

        if (slider)
        {
            sliderComponent = e.target.GetComponent<Slider>();
        }
        if (sliderHandle)
        {
            sliderComponent = e.target.GetComponentInParent<Slider>();
        }



        if (mark_option)
        {
            GameObject mark_parent = e.target.transform.parent.gameObject;
            mark_parent.GetComponent<Toggle>().Select();
            /*
            Transform t = mark_parent.transform;
            bool foundDropdown = false;
            while (t.parent != null && !foundDropdown && mark_text != "undefined")
            {
                if (t.parent.tag == "Dropdown" && SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource) > 0.5)
                {
                    foundDropdown = true;
                    Dropdown dropdownComponent = t.parent.gameObject.GetComponent<Dropdown>();

                    int hovered_index = -1;
                    for (int i = 0; i < dropdownComponent.options.Count; i++)
                    {
                        if (dropdownComponent.options[i].text == mark_text)
                        {
                            hovered_index = i;
                        }
                    }
                    if(hovered_index > -1)
                    {

                        Toggle toggle = mark_parent.GetComponent<Toggle>();
                        toggle.isOn = true;
                    }

                } else
                {
                    t = t.parent.transform;
                }
            }
            Debug.Log("Found a mark!");
            */

        }

        if (button != null)
        {
            button.Select();
            Debug.Log("HandlePointerIn", e.target.gameObject);
        }
        if (toggle != null)
        {
            toggle.Select();
            TextMeshProUGUI text = toggle.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.color = new Color(0.6f, 0.6f, 0.6f);

            Debug.Log("HandlePointerIn", e.target.gameObject);
        }
        else if (scrollbar != null)
        {
            scrollbar.Select();
            Debug.Log("HandlPointerIn", e.target.gameObject);
        }
        else if (sliderComponent != null)
        {
            sliderComponent.Select();
            Debug.Log("HandlPointerIn", e.target.gameObject);
        }
        else if (dropdown != null)
        {
            dropdown.Select();
            previousDropdown = dropdown;
            Debug.Log("HandlePointerIn", e.target.gameObject);
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {

        var button = e.target.GetComponent<Button>();
        var toggle = e.target.GetComponent<Toggle>();
        var scrollbar = e.target.GetComponent<Scrollbar>();
        var dropdown = e.target.GetComponent<Dropdown>();
        var sliderComponent = e.target.GetComponent<Slider>();

        var mark_option = e.target.tag == "DropdownOption";
        var scrollbar2 = e.target.tag == "Scrollbar";
        var slider = e.target.tag == "Slider";
        var sliderHandle = e.target.tag == "SliderHandle";

        var barGraph = e.target.tag == "BarGraphBar";
        if (barGraph)
        {
            var bar = e.target.GetComponentInParent<BarGraphBarController>();
            bar.setHoverState(false);
        }

        if (scrollbar2)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        if (slider)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }


        if (mark_option)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if (toggle != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            TextMeshProUGUI text = toggle.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.color = new Color(1, 1, 1);
        }
        else if (scrollbar != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if (dropdown != null)
        {
            //previousDropdown.Select();
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if (sliderComponent != null)
        {
            //previousDropdown.Select();
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if (sliderHandle)
        {
            //previousDropdown.Select();
            //EventSystem.current.SetSelectedGameObject(null);
        }
    }

}