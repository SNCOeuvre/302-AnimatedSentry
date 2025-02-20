using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    public float visionDistance = 10;
    [Range(1,20)]
    public int roundsPerSecond = 5;

    public PointAt boneShoulderRight;
    public PointAt boneShoulderLeft;

    //allows to read, but not set outside this class

    //public getter, but makes them private; allows to be obtained without being able to change externally
    public TargetableObject target { get; private set; }
    public bool playerWantsToAim { get; private set; }
    public bool playerWantsToAttack { get; private set; }
    private List<TargetableObject> validTargets = new List<TargetableObject>();
    private float coolDownScan = 0;
    private float coolDownPickTarget = 0;
    private float coolDownAttack = 0;

    private CameraController cam;

    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        playerWantsToAttack = Input.GetButton("Fire1");
        playerWantsToAim = Input.GetButton("Fire2");

        coolDownScan -= Time.deltaTime;
        coolDownPickTarget -= Time.deltaTime;
        coolDownAttack -= Time.deltaTime;

        if (playerWantsToAim)
        {

            if (target != null)
            {

                //turn towards it~~~
                Vector3 toTarget = target.transform.position - transform.position;
                toTarget.y = 0;


                if (toTarget.magnitude > 3 && !CanSeeThing(target))
                {
                    target = null;
                }
            }
            if (coolDownScan <= 0) ScanForTargets();
            if (coolDownPickTarget <= 0) PickATarget();
        }
        else
        {
            target = null;
        }


        //first if, then if statement; if target is true, grab transform, if not, null
        if(boneShoulderLeft) boneShoulderLeft.target = target ? target.transform : null;
        if(boneShoulderRight) boneShoulderRight.target = target ? target.transform : null;

        DoAttack();
        //print(target);
    }

    void DoAttack()
    {
        if (coolDownAttack > 0) return;
        if (!playerWantsToAim) return;
        if (!playerWantsToAttack) return;
        if (target == null) return;
        if (!CanSeeThing(target)) return;

        coolDownAttack = 1f / roundsPerSecond;

        //spawn projectiles...

        //or take health away from target
        boneShoulderLeft.transform.localEulerAngles += new Vector3(-28, 0, 0);
        boneShoulderRight.transform.localEulerAngles += new Vector3(-29, 0, 0);
        if(cam) cam.Shake(0.2f);
    }
    void ScanForTargets()
    {
        coolDownScan = .5f;

        validTargets.Clear();

        TargetableObject[] things = GameObject.FindObjectsOfType<TargetableObject>();
        foreach (TargetableObject thing in things)
        {
            if (CanSeeThing(thing))
            {
                validTargets.Add(thing);
            }
        }
    }

    private bool CanSeeThing(TargetableObject thing)
    {
        Vector3 vToThing = thing.transform.position - transform.position;

        //is too far to see?
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false;

        //how much is in front of player?
        float alignment = Vector3.Dot(transform.forward, vToThing.normalized);

        //is within so many degrees of forward
        if (alignment < .4f) return false;

        return true;

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
