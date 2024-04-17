using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    Vector3 dest;

    public float speed;

   


    void Update()
    {
        agent.speed = speed;
        dest = player.position;
        agent.destination = dest;
    }
}
