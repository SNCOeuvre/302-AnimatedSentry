using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PointAt : MonoBehaviour
{

    public Transform target;

    public bool lockAxisX = false;
    public bool lockAxisY = false;
    public bool lockAxisZ = false;

    private Quaternion startRotation;
    private Quaternion goalRotation;
    

    void Start()
    {
        //playerTargeting = GetComponentInParent<PlayerTargeting>();
        startRotation = transform.localRotation;
    }

    void Update()
    {
        TurnTowardsTarget();
    }

    private void TurnTowardsTarget()
    {
        //if (playerTargeting && playerTargeting.target && playerTargeting.playerWantsToAim)
        if (target != null)
        {
            Vector3 vToTarget = target.position - transform.position;
            vToTarget.Normalize();
            //Vector3 vToTarget = playerTargeting.target.transform.position - transform.position;
            Quaternion worldRot = Quaternion.LookRotation(vToTarget, Vector3.up);

            Quaternion localRot = worldRot;
            if (transform.parent)
            {
                 localRot = Quaternion.Inverse(transform.parent.rotation) * worldRot;

            }

            Vector3 euler = localRot.eulerAngles;

            //euler.x *= weightAxisX;
            //euler.x *= weightAxisY;
            //euler.x *= weightAxisZ;

            if (lockAxisX) euler.x = startRotation.eulerAngles.x;
            if (lockAxisY) euler.y = startRotation.eulerAngles.y;
            if (lockAxisZ) euler.z = startRotation.eulerAngles.z;
            


            localRot.eulerAngles = euler;

            goalRotation = localRot;
        }
        else
        {
            //transform.localRotation = startRotation;
            goalRotation = startRotation;

        }
        transform.localRotation = AnimMath.Ease(transform.localRotation, goalRotation, .001f);
    }
}
