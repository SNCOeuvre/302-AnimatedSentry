using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5;

    public Camera cam;
    
    CharacterController pawn;
    
    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    
    void Update()
    {

        //GetAxisRaw makes it where there's no gravity to input
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        bool playerWantsToMove = (v != 0 || h != 0);

        if (cam && playerWantsToMove)
        {
            float playerYaw = transform.eulerAngles.y;
            float camYaw = cam.transform.eulerAngles.y;
            /*while (camYaw > playerYaw + 180) camYaw -= 360;
            while (camYaw < playerYaw - 180) camYaw += 360;

            print($"Cam Yaw: {camYaw} Player Yaw {playerYaw}");
            */
            Quaternion targetRotation = Quaternion.Euler(0, camYaw, 0);
            transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        }
        //movement is a vector 3
        Vector3 moveDir = transform.forward * v + transform.right * h;
        if (moveDir.sqrMagnitude > 1) moveDir.Normalize();

        pawn.SimpleMove(moveDir * walkSpeed * 2);
    }
}
