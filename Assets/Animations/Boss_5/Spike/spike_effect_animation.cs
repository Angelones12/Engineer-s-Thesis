using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike_effect_animation : StateMachineBehaviour
{
    Transform transform;
    float speed;
    float scaleFactor;
    Transform player;
    bool haveSwitched;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
        speed = 10.0f;
        scaleFactor = 1.0f;
        haveSwitched = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float scaleRate = 80f;
        scaleFactor += scaleRate * Time.deltaTime;
        scaleFactor = Mathf.Clamp(scaleFactor, 1.0f, 200.0f);
        Vector3 newScale = Vector3.one * scaleFactor;
        transform.localScale = newScale;



        if (scaleFactor >= 200.0f)
        {
            Destroy(animator.gameObject);
        }
    }
}

