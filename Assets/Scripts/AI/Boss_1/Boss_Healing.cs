using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Healing : StateMachineBehaviour
{
    Boss_Stats bossStats;
    private int halfhp;
    private float healingRate = 1.0f;
    private float elapsedTime = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bossStats = animator.GetComponent<Boss_Stats>();
        if (bossStats != null)
        {
            halfhp = bossStats.bosshalfhp;
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Have_Healed", true);
        if (bossStats.boss_1_health == halfhp) { animator.SetBool("Is_Interrupted",true); }

        elapsedTime += Time.deltaTime;
       
        if (elapsedTime >= healingRate && bossStats.boss_1_health < halfhp && !animator.GetBool("Is_Interrupted"))
        {
            animator.SetBool("HealingRightNow", true);
            
            bossStats.boss_1_health++;
            Debug.Log("Current Boss Health: " + bossStats.boss_1_health);
            elapsedTime = 0.0f;
        }
    }
}
