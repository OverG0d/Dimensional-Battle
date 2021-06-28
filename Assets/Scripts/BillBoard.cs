using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{

    public Camera main_Camera;

    // Update is called once per frame
    void Update()
    {
       
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + main_Camera.transform.rotation * Vector3.back,
       main_Camera.transform.rotation * Vector3.up);
    }
}
