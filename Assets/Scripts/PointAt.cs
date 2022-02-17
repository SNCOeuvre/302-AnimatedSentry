using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAt : MonoBehaviour
{
    private PlayerTargeting playerTargeting;

    void Start()
    {
        playerTargeting = GetComponentInParent<PlayerTargeting>();
    }

    void Update()
    {
        TurnTowardsTarget();
    }

    private void TurnTowardsTarget()
    {
        if (playerTargeting && playerTargeting.target && playerTargeting.playerWantsToAim)
        {
            Vector3 vToTarget = playerTargeting.target.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(vToTarget.normalized);
        }
    }
}
