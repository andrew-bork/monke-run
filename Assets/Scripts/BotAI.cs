using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Animator animator;

    MonkeAnimator monkeAnimator;
    Vector3 dest;

    public float speed;
    public float runSpeed = 0.5f;




    private void Start()
    {

        agent.speed = speed;
        monkeAnimator = new MonkeAnimator(animator);
    }

    void Update()
    {
        agent.destination = dest = player.position;
        
        if(agent.velocity.magnitude > runSpeed)
        {
            monkeAnimator.run();
        }else if(agent.velocity.magnitude > 0.01f)
        {
            monkeAnimator.walk();
        }else
        {
            monkeAnimator.idle();
        }
    }
}


class MonkeAnimator
{
    Animator animator;
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
}