using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragOnMouseClick : MonoBehaviour
{
    float distance = 3;
    //float rotSpeed = 15;
    //float scaleSpeed = 0.7f;

    
    public void OnMouseDrag()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        transform.position = objPosition;

    }
    /*

    private void Update()
    {
        if (rotateRight)
        {
            transform.Rotate(0, -Time.deltaTime * rotSpeed, 0);
        }

        if (rotateLeft)
        {
            transform.Rotate(0, Time.deltaTime * rotSpeed, 0);
        }

        if (scaleUp)
        {
            transform.localScale += Time.deltaTime * scaleSpeed * transform.localScale;
        }

        if (scaleDown)
        {
            transform.localScale -= Time.deltaTime * scaleSpeed * transform.localScale;
        }
    }

    private bool rotateRight = false;
    private bool rotateLeft = false;
    private bool scaleUp = false;
    private bool scaleDown = false;

    public void RotateRight()
    {
        rotateRight = true;
    }
    public void StopRotateRight()
    {
        rotateRight = false;
    }

    public void RotateLeft()
    {
        rotateLeft = true;
    }

    public void StopRotateLeft()
    {
        rotateLeft = false;
    }

    public void ScaleUp()
    {
        scaleUp = true;
    }

    public void StopScaleUp()
    {
        scaleUp = false;
    }

    public void ScaleDown()
    {
        scaleDown = true;
    }

    public void StopScaleDown()
    {
        scaleDown = false;
    }
    */

}
