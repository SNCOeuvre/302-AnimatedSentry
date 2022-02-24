using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    CharacterController pawn;
    NavMeshAgent agent;

    Transform navTarget;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();

        PlayerTargeting player = FindObjectOfType<PlayerTargeting>();

        navTarget = player.transform;

    }

    
    void Update()
    {
        if(navTarget) agent.destination = navTarget.position;
        //pawn.SimpleMove(Vector3.zero);
    }
}
