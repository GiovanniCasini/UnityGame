using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 offsetR = new Vector3(0.1f, 0, 0);
    private Vector3 offsetL = new Vector3(-0.1f, 0, 0);
    private Vector3 offsetU = new Vector3(0, 0.1f, 0);
    private Vector3 offsetD = new Vector3(0, -0.1f, 0);

    void Update()
    {
        if (Input.GetKey("d"))
        {
            transform.position = transform.position + offsetR;
        }
        if (Input.GetKey("a"))
        {
            transform.position = transform.position + offsetL;
        }
        if (Input.GetKey("w"))
        {
            transform.position = transform.position + offsetU;
        }
        if (Input.GetKey("s"))
        {
            transform.position = transform.position + offsetD;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (GetComponent<Camera>().orthographicSize < 80)
            {
                GetComponent<Camera>().orthographicSize += 0.1f;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (GetComponent<Camera>().orthographicSize > 1)
            {
                GetComponent<Camera>().orthographicSize -= 0.1f;
            }  
        }
    }
}
