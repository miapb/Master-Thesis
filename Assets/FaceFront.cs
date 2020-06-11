using UnityEngine;
using System.Collections;

public class FaceFront : MonoBehaviour
{
    public Camera m_Camera;
    public GameObject target = null;

    //Orient the camera after all movement is completed this frame to avoid jittering
    public void setCamera(Camera newCam)
    {
        m_Camera = newCam;
    }

    void LateUpdate()
    {
        if (gameObject.activeInHierarchy)
        {
            //m_Camera = Camera.main;

            /*
            transform.LookAt(
                transform.position + m_Camera.transform.rotation * Vector3.forward,
                m_Camera.transform.rotation * Vector3.up);
            Vector3 euler = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(euler.x, euler.y, 0);

            if (target != null)
            {
                transform.LookAt(target.transform);
            }
            */
            transform.LookAt(Camera.main.transform);
            Vector3 euler = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(euler.x*-1, euler.y+180, euler.z);
        }
    }
}
