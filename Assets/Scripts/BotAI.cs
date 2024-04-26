using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    Vector3 dest;

    public Camera jumpScareCam;

    public float speed, catchDistance, jumpScareTime;

    public string sceneAfterDeath;
   


    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        agent.speed = speed;
        dest = player.position;
        agent.destination = dest;
        if(distance <= catchDistance)
        {
            player.gameObject.SetActive(false);
            jumpScareCam.gameObject.SetActive(true);
            StartCoroutine(killPlayer());
        }
    }

    IEnumerator killPlayer()
    {
        yield return new WaitForSeconds(jumpScareTime);
        SceneManager.LoadScene(sceneAfterDeath);
    }
}
