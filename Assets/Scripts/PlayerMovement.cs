using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform boneLegLeft;
    public Transform boneLegRight;

    public float walkSpeed = 5;
    [Range(-10,-1)]
    public float gravity = -1;

    public Camera cam;
    
    CharacterController pawn;
    private Vector3 inputDir;
    private float velocityVertical = 0;

    private float cooldownJumpWindow = 0;
    public bool isGrounded
    {
        get
        {
            return pawn.isGrounded || cooldownJumpWindow > 0;
        }
    }
    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        if (cooldownJumpWindow > 0) cooldownJumpWindow -= .5f;
        //lateral movement
        //GetAxisRaw makes it where there's no gravity to input
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        bool playerWantsToMove = (v != 0 || h != 0);

        if (cam && playerWantsToMove)
        {
            float playerYaw = transform.eulerAngles.y;
            float camYaw = cam.transform.eulerAngles.y;
            while (camYaw > playerYaw + 180) camYaw -= 360;
            while (camYaw < playerYaw - 180) camYaw += 360;
            /*
            print($"Cam Yaw: {camYaw} Player Yaw {playerYaw}");
            */


            Quaternion playerRotation = Quaternion.Euler(0, playerYaw, 0);
            Quaternion targetRotation = Quaternion.Euler(0, camYaw, 0);
            transform.rotation = AnimMath.Ease(playerRotation, targetRotation, .01f);

        }
        //movement is a vector 3
        inputDir = transform.forward * v + transform.right * h;
        if (inputDir.sqrMagnitude > 1) inputDir.Normalize();

        //vertical movement
        bool wantsToJump = Input.GetButtonDown("Jump");
        if (pawn.isGrounded)
        {
            if(wantsToJump)
            {
                cooldownJumpWindow = 0;
                velocityVertical = 10;
            }
        }
        
        
        velocityVertical += gravity * Time.deltaTime;        




        Vector3 moveAmount = inputDir * walkSpeed + Vector3.up * velocityVertical;


        pawn.Move(moveAmount * Time.deltaTime);
        if (pawn.isGrounded)
        {
            cooldownJumpWindow = .5f;
            velocityVertical = 0;
        }

        WalkAnimation();
    }

    void WalkAnimation()
    {


        Vector3 inputDirLocal = transform.InverseTransformDirection(inputDir);
        Vector3 axis = Vector3.Cross(Vector3.up, inputDirLocal);

        float alignment = Vector3.Dot(inputDirLocal, Vector3.forward);
        alignment = Mathf.Abs(alignment);


        float degrees = AnimMath.Lerp(10, 40, alignment);
        float speed = 20;

        float wave = Mathf.Sin(Time.time * speed) * degrees;
        boneLegLeft.localRotation = Quaternion.AngleAxis(wave, axis);
        boneLegRight.localRotation = Quaternion.AngleAxis(-wave, axis);
    }
}
