using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class MonkeAnimator : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Animator animator;

    public float speed;
    public float runSpeed = 0.5f;

    int isMovingHash;
    int isRunningHash;
    int chestPoundHash;
    public MonkeAnimator(Animator _animator)
    {
        animator = _animator;
    }

    public void run()
    {
        animator.SetBool("isMoving", true);
        animator.SetBool("isRunning", true);
    }

    public void walk()
    {
        animator.SetBool("isMoving", true);
        animator.SetBool("isRunning", false);

    }

    public void chestPound()
    {
        animator.SetTrigger("triggerPound");
    }

    public void idle()
    {
        animator.SetBool("isMoving", false);
        animator.SetBool("isRunning", false);
    }


    void Update()
    {

        if (agent.speed > runSpeed)
        {
            run();
        }
        else if (agent.velocity.magnitude > 0.01f)
        {
            walk();
        }
        else
        {
            idle();
        }
    }
}