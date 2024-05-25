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
        isMovingHash = Animator.StringToHash("isMoving");
        isRunningHash = Animator.StringToHash("isRunning");
        chestPoundHash = Animator.StringToHash("triggerPound");
    }

    public void run()
    {
        animator.SetBool(isMovingHash, true);
        animator.SetBool(isRunningHash, true);
    }

    public void walk()
    {
        animator.SetBool(isMovingHash, true);
        animator.SetBool(isRunningHash, false);

    }

    public void chestPound()
    {
        animator.SetTrigger(chestPoundHash);
    }

    public void idle()
    {
        animator.SetBool(isMovingHash, false);
        animator.SetBool(isRunningHash, false);
    }


    void Update()
    {

        if (agent.velocity.magnitude > runSpeed)
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