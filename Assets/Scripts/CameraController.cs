using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;

    public Vector3 targetOffset;

    private Camera cam;
    private float pitch = 0;
    private float yaw = 0;
    private float dollyDis = 10;
    public float mouseSensX = 5;
    public float mouseSensY = -5;
    public float mouseSensScoll = 5;


    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    
    void Update()
    {
        //1. ease position:

        if(target)
        {
            //transform.position = ..easing
            transform.position = AnimMath.Ease(transform.position, target.position + targetOffset, .01f);
        }

        //2. ser rotation (todo: ease)

        //mouse x, mouse delta x
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        

        yaw += mx * mouseSensX;
        pitch += my * mouseSensY;

        pitch = Mathf.Clamp(pitch, -10, 75);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        //3. dolly camera in/out
        dollyDis += Input.mouseScrollDelta.y * mouseSensScoll;
        dollyDis = Mathf.Clamp(dollyDis, 3, 20);
        cam.transform.localPosition = AnimMath.Ease(cam.transform.localPosition, new Vector3(0, 0, -dollyDis), .02f);


    }

    private void OnDrawGizmos()
    {
        //draws box around target
        if (!cam) cam = GetComponentInChildren<Camera>();
        if (!cam) return;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
        //draws a line from camera to target
        Gizmos.DrawLine(transform.position, cam.transform.position);
    }
}
