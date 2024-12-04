using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2_behaviour : StateMachineBehaviour
{
    Transform player;
    Rigidbody rb;
    Boss_Turning boss_turning;
    Boss_2_Stats boss_2_Stats;
    float speed;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();

        player = GameObject.Find("queen")?.transform;
        boss_turning = animator.GetComponent<Boss_Turning>();

        boss_2_Stats = animator.GetComponent<Boss_2_Stats>();
        if (boss_2_Stats != null)
        {
            speed = boss_2_Stats.boss_2_speed;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        boss_turning.LookAtPlayer();
        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);
    }
}

