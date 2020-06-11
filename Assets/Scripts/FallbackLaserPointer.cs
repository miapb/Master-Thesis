using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallbackLaserPointer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HoverHandler hoverHandler;

    void Start()
    {
        
    }

    Ray ray;
    RaycastHit hit;

    public event PointerEventHandler PointerIn;
    public event PointerEventHandler PointerOut;
    public event PointerEventHandler PointerClick;

    public virtual void OnPointerIn(PointerEventArgs e)
    {
        if (PointerIn != null)
            PointerIn(this, e);
    }

    public virtual void OnPointerClick(PointerEventArgs e)
    {
        if (PointerClick != null)
            PointerClick(this, e);
    }

    public virtual void OnPointerOut(PointerEventArgs e)
    {
        if (PointerOut != null)
            PointerOut(this, e);
    }




    Transform previousContact = null;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool bHit = (Physics.Raycast(ray, out hit));

        if (bHit)
        {
            Debug.Log(hit.collider.name);
            //hoverHandler.HandleHover(hit);
        }

        if (previousContact && previousContact != hit.transform)
        {

            PointerEventArgs args = new PointerEventArgs();
            Debug.Log("Out: " + hit.transform + " to " + previousContact);
            //args.fromInputSource = pose.inputSource;
            args.distance = 0f;
            args.flags = 0;
            args.target = previousContact;
            OnPointerOut(args);
            previousContact = null;
        }

        if (bHit && previousContact != hit.transform)
        {
            PointerEventArgs argsIn = new PointerEventArgs();
            Debug.Log("In: " + hit.transform + " to " + previousContact);

            //argsIn.fromInputSource = pose.inputSource;
            argsIn.distance = hit.distance;
            argsIn.flags = 0;
            argsIn.target = hit.transform;
            OnPointerIn(argsIn);
            previousContact = hit.transform;
        }
        if (!bHit)
        {
            previousContact = null;
        }

    }
}
