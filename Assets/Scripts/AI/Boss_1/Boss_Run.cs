using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts;
using System.Text;
//using UnityEditorInternal;
using JetBrains.Annotations;
using System;
using Random = UnityEngine.Random;

public class Boss_Run : StateMachineBehaviour
{
    Transform player;
    Rigidbody rb;
    Boss_Turning boss_turning;
    private int speed;
    private int attackRange;
    Boss_Stats bossStats;
    private int health;
    private int healthquarter;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.Find("queen")?.transform;
        rb = animator.GetComponent<Rigidbody>();
        boss_turning = animator.GetComponent<Boss_Turning>();

        bossStats = animator.GetComponent<Boss_Stats>();
        if (bossStats != null)
        {
            speed = bossStats.boss_1_speed;
            attackRange = bossStats.boss_1_attackRange;
            health = bossStats.boss_1_health;
            healthquarter = health / 4;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss_turning.LookAtPlayer();
        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);

        float distance = Vector3.Distance(player.position, rb.position);
     
        
        float epsilon = 0.01f;

            if (distance <= attackRange + epsilon)
            {
                Attack(animator);
            
            }
       

    }

    private bool isAttacking = false;

    public void Attack(Animator animator)
    {
        if (!isAttacking)
        {
            MonoBehaviour monoBehaviour = animator.GetComponent<MonoBehaviour>();

            monoBehaviour.StartCoroutine(AttackCoroutine(animator));
        }
    }

    private IEnumerator AttackCoroutine(Animator animator)
    {
        isAttacking = true;

        int randomAttack = Random.Range(1, 5);
        Debug.Log(randomAttack);

        switch (randomAttack)
        {
            case 1:
                animator.SetTrigger("Attack_Melee");
                Debug.Log("Attack_Melee");
                break;
            case 2:
                animator.SetTrigger("Attack_Kick");
                Debug.Log("Attack_Kick");
                break;
            case 3:
                animator.SetTrigger("Attack_BackHand");
                Debug.Log("Attack_BackHand");
                break;
            case 4:
                animator.SetTrigger("Attack_Slash");
                Debug.Log("Attack_Slash");
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

       
        
        isAttacking = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack_Kick");
        animator.ResetTrigger("Attack_Melee");
        animator.ResetTrigger("Attack_BackHand");
        animator.ResetTrigger("Attack_Slash");
    }




}
