using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump_Attack : StateMachineBehaviour
{
    private GameObject prefab;
    Boss_2_Stats boss_2_Stats;
    Rigidbody rb;
    private Animator animator;
    private bool haveexploded;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss_2_Stats = animator.GetComponent<Boss_2_Stats>();

        if (boss_2_Stats != null)
        {

            prefab = boss_2_Stats.prefab;
            rb = boss_2_Stats.rb;
            this.animator = boss_2_Stats.animator;
        }
        haveexploded = false;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float animationTime = stateInfo.normalizedTime * stateInfo.length;
        if (!haveexploded && animationTime >= 2.07f)
        {
            WywolajWybuch();
            haveexploded = true;
        }
    }


    public void WywolajWybuch()
    {
        if (prefab != null && rb != null)
        {
            Vector3 currentPosition = rb.transform.position;
            float newYPosition = currentPosition.y + 1.0f;
            Vector3 newPosition = new Vector3(currentPosition.x, newYPosition, currentPosition.z);
            GameObject prefabInstance1 = Instantiate(prefab, newPosition, Quaternion.identity);
            Rigidbody prefabRigidbody1 = prefabInstance1.GetComponent<Rigidbody>();
        }
    }

}
