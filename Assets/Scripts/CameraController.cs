using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerTargeting player;

    public Vector3 targetOffset;

    private Camera cam;

    private float pitch = 0;
    private float yaw = 0;
    private float dollyDis = 10;
    public float mouseSensX = 5;
    public float mouseSensY = -5;
    public float mouseSensScoll = 5;

    //how many seconds to shake....~~~~~fgwfefwfgewf
    private float shakeTimer = 0;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        if (player == null)
        {
            PlayerTargeting script = FindObjectOfType<PlayerTargeting>();
            if (script != null) player = script;
        }
    }

    
    void Update()
    {
        //is the player aiming?
        bool isAiming = (player && player.target && player.playerWantsToAim);
        
        //1. ease rig position:

        if(player)
        {
            //transform.position = ..easing
            transform.position = AnimMath.Ease(transform.position, player.transform.position + targetOffset, .01f);
        }

        //2. set rig rotation (todo: ease)
        float playerYaw = AnimMath.AngleWrapDegrees(yaw, player.transform.eulerAngles.y);

        //mouse x, mouse delta x
        if (isAiming)
        {
            Quaternion tempTarget = Quaternion.Euler(0, playerYaw, 0);

            transform.rotation = AnimMath.Ease(transform.rotation, tempTarget, .001f);


        }
        else
        {
            float mx = Input.GetAxis("Mouse X");
            float my = Input.GetAxis("Mouse Y");
        

            yaw += mx * mouseSensX;
            pitch += my * mouseSensY;

            //if (yaw > 360) yaw -= 360;
            //if (yaw < 0) yaw += 360;

            pitch = Mathf.Clamp(pitch, -10, 75);


            transform.rotation = AnimMath.Ease(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .01f);
        }

        //3. dolly camera in/out
        
        dollyDis += Input.mouseScrollDelta.y * mouseSensScoll;
        dollyDis = Mathf.Clamp(dollyDis, 3, 20);

        float tempZ = isAiming ? 10 : dollyDis;

        cam.transform.localPosition = AnimMath.Ease(cam.transform.localPosition, new Vector3(0, 0, -tempZ), .02f);

        //4. rotate the camera object(not rig)

        if (isAiming)
        {
            //rig rotation
            Vector3 vToAimTarget = player.target.transform.position - cam.transform.position;
            Quaternion worldRot = Quaternion.LookRotation(vToAimTarget);
            Quaternion localRot = worldRot;

            if (cam.transform.parent)
            {
                localRot = Quaternion.Inverse(cam.transform.parent.rotation) * worldRot;
            }

            Vector3 euler = localRot.eulerAngles;
            euler.z = 0;
            localRot.eulerAngles = euler;
            
            //point at target
            cam.transform.localRotation = AnimMath.Ease(cam.transform.localRotation, localRot, .001f);
        }
        else
        {
            cam.transform.localRotation = AnimMath.Ease(cam.transform.localRotation, Quaternion.identity, .001f);
        }

        UpdateShake();

    }

    void UpdateShake()
    {
        if (shakeTimer < 0) return;
        shakeTimer -= Time.deltaTime;
        float p = shakeTimer / 1;
        p = p * p;
        p = AnimMath.Lerp(1, .98f, p);

        //sends back halfway
        Quaternion randomRot = AnimMath.Lerp(Random.rotation, Quaternion.identity, p); 

        cam.transform.localRotation *= randomRot;//+= Random.onUnitSphere * .1f;

    }
    public void Shake(float time)
    {
        if(time > shakeTimer) shakeTimer = time;

        //for growing intensity
        //shakeTimer += time;
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
