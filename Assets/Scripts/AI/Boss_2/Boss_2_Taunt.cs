using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_2_Taunt : StateMachineBehaviour
{
    Boss_2_Stats boss_2_Stats;
    private float knockBackForce;
    public Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("queen")?.transform;
        boss_2_Stats = animator.GetComponent<Boss_2_Stats>();
        if (boss_2_Stats != null)
        {
            knockBackForce = boss_2_Stats.knockBackForce;
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player != null)
        {
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                Vector3 knockbackDirection = player.position - animator.transform.position;
                knockbackDirection.y = 0f;
                knockbackDirection.Normalize();
                float smallerKnockBackForce = knockBackForce * 0.5f;
                playerRigidbody.AddForce(knockbackDirection * smallerKnockBackForce, ForceMode.Impulse);
            }
        }
    }
}
