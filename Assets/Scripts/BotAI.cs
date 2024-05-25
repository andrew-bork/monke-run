using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Camera jumpScareCam;
    public Rigidbody rb;

    public float speed, catchDistance, jumpScareTime, jumpCooldown = 2f;
    public string sceneAfterDeath;
    public bool isGrounded;

    private Vector3 dest;
    private float lastJumpTime;

    private void Start()
    {
        isGrounded = true;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        lastJumpTime = -jumpCooldown;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        agent.speed = speed;
        dest = player.position;
        agent.destination = dest;

        if (ShouldJump())
        {
            Jump();
        }

        if (distance <= catchDistance)
        {
            player.gameObject.SetActive(false);
            jumpScareCam.gameObject.SetActive(true);
            agent.speed = 0;
            agent.isStopped = true;
            StartCoroutine(killPlayer());
        }
        
    }
    private IEnumerator killPlayer()
    {
        yield return new WaitForSeconds(jumpScareTime);
        SceneManager.LoadScene(sceneAfterDeath);
    }

    private bool ShouldJump()
    {
        // Check conditions for the bot to jump
        return isGrounded && !agent.hasPath && Time.time - lastJumpTime >= jumpCooldown;
    }


    private void Jump()
    {
        Debug.Log("Jumping");

        // Disable NavMeshAgent control
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.isStopped = true;

        // Enable Rigidbody control
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 force = Vector3.up * player.position.y;
        // Apply an upward force to the Rigidbody
        rb.AddForce(force, ForceMode.Impulse);
        lastJumpTime = Time.time;
    }

    private IEnumerator PushToPlayer()
    {
        Debug.Log("Pushed");
        // Wait for a moment to reach the apex
        yield return new WaitForSeconds(1); // Adjust the delay as needed

        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Remove vertical component to focus on horizontal movement

        // Face the player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Adjust force based on bot's mass and desired jump distance
        float forceMagnitude = speed * rb.mass;

        // Apply horizontal force towards the player
        rb.AddForce(direction * forceMagnitude, ForceMode.Impulse);

        Debug.Log($"Applying force towards player: {direction * forceMagnitude}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("touching ground");
        agent.Warp(transform.position);
        // Re-enable NavMeshAgent control
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;
        agent.destination = dest;

        // Disable Rigidbody control
        rb.isKinematic = false;
        rb.useGravity = false;
        isGrounded = true;

        Debug.Log("agent enabled");
        
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("left ground");
        if (collision.collider != null && collision.collider.CompareTag("Floor"))
        {
            isGrounded = false;
            StartCoroutine(PushToPlayer());
        }
    }
}