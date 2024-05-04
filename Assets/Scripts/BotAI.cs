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

    public float speed, catchDistance, jumpScareTime, jumpHeight;

    public string sceneAfterDeath;

    public Rigidbody rb;

    public bool isGrounded;

    private void Start()
    {
        isGrounded = true;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        agent.speed = speed;
        dest = player.position;
        agent.destination = dest;
        if (!agent.hasPath && isGrounded)
        {
            Debug.Log("ready to jump");
            if (agent.enabled)
            {
                // set the agents target to where you are before the jump
                // this stops her before she jumps. Alternatively, you could
                // cache this value, and set it again once the jump is complete
                // to continue the original move
                // disable the agent
                agent.updatePosition = false;
                agent.updateRotation = false;
                agent.isStopped = true;
            }
            // make the jump
            rb.isKinematic = false;
            rb.useGravity = true;
            
            isGrounded = false;
            Debug.Log("Jumped");
        }
        if (distance <= catchDistance)
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

   private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider != null && collision.collider.tag == "Floor")
        {
            if (!isGrounded)
            {
                if (agent.enabled)
                {
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    agent.isStopped = false;
                }
                rb.isKinematic = true;
                rb.useGravity = false;
                isGrounded = true;
                agent.destination = dest;

                Debug.Log("agent enabled");
            }
        }
    }
   
}