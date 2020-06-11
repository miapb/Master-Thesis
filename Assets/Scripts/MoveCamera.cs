using UnityEngine;
using Mapbox.Unity.Map;

public class MoveCamera : MonoBehaviour
{
    public float camSpeed = 30f;
    public float camBorderThickness = 10f;
    public Vector2 camLimit;

    public float scrollSpeed = 20f;
    //public float minY = 20f;
    //public float maxY = 120f;
    

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        bool edgePanEnabled = false;
        if (Input.GetKey("w") || (edgePanEnabled && (Input.mousePosition.y >= Screen.height - camBorderThickness)))
        {
            pos.z += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || (edgePanEnabled && (Input.mousePosition.y <= camBorderThickness)))
        {
            pos.z -= camSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || (edgePanEnabled && (Input.mousePosition.x >= Screen.width - camBorderThickness)))
        {
            pos.x += camSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || (edgePanEnabled && (Input.mousePosition.x <= camBorderThickness)))
        {
            pos.x -= camSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;


        //pos.x = Mathf.Clamp(pos.x, -camLimit.x, camLimit.x);
        //pos.y = Mathf.Clamp(pos.y, minY, maxY);
        //pos.z = Mathf.Clamp(pos.z, -camLimit.y, camLimit.y);
        transform.position = pos;
    }
        

}
