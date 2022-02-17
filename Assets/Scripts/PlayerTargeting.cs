using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public float visionDistance = 10;
    //allows to read, but not set outside this class
    public TargetableObject target { get; private set; }
    public bool playerWantsToAim { get; private set; }

    private List<TargetableObject> validTargets = new List<TargetableObject>();
    private float coolDownScan = 0;
    private float coolDownPickTarget = 0;

    void Start()
    {
        
    }

    void Update()
    {

        playerWantsToAim = Input.GetButton("Fire2");

        coolDownScan -= Time.deltaTime;
        coolDownPickTarget -= Time.deltaTime;

        if (playerWantsToAim)
        {
            if (coolDownScan <= 0) ScanForTargets();
            if (coolDownPickTarget <= 0) PickATarget();
        }
        else
        {
            target = null;
        }

        print(target);
    }
           
            


    void ScanForTargets()
    {
        coolDownScan = .5f;

        validTargets.Clear();

        TargetableObject[] things = GameObject.FindObjectsOfType<TargetableObject>();
        foreach (TargetableObject thing in things)
        {
            Vector3 vToThing = thing.transform.position - transform.position;

            //is close enough to see
            if (vToThing.sqrMagnitude < visionDistance * visionDistance)
            {
                float alignment = Vector3.Dot(transform.forward, vToThing.normalized);

                //is within so many degrees of forward
                if (alignment > .35f)
                {
                validTargets.Add(thing);
                }

            }

        }
    }
    void PickATarget()
    {
        if (target) return;
        float closestDistanceSoFar = 0;
        foreach (TargetableObject thing in validTargets)
        {
            Vector3 vToThing = thing.transform.position - transform.position;
            float dis = vToThing.sqrMagnitude;
            if (dis < closestDistanceSoFar || target == null)
            {
                closestDistanceSoFar = dis;
                target = thing;
            }
        }
    }
}
