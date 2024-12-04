using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion_effect_code : StateMachineBehaviour
{
    Rigidbody rb;
    Transform transform;
    float speed;
    float scaleFactor;
    Transform player;
    float inicialspeed;
    bool haveSwitched;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody>();
        transform = animator.transform;
        speed = 10.0f;
        inicialspeed = speed;
        scaleFactor = 1.0f;
        haveSwitched = false;
        player = GameObject.Find("queen")?.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        speed = playerPosition();
        rb.velocity = new Vector3(speed, 0, 0);

       
        if (speed < 0 && !haveSwitched)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            haveSwitched = true;
        }
        else if (speed > 0 && !haveSwitched)
        {
            transform.rotation = Quaternion.identity;
            haveSwitched = true;
        }

        float scaleRate = 0.4f;
        scaleFactor += scaleRate * Time.deltaTime;
        scaleFactor = Mathf.Clamp(scaleFactor, 1.0f, 2.0f);
        Vector3 newScale = Vector3.one * scaleFactor;
        transform.localScale = newScale;

        if (scaleFactor >= 2.0f)
        {
            Destroy(animator.gameObject);
        }
    }

    float playerPosition()
    {
        if (player != null)
        {
            if (rb.position.x > player.position.x)
            {
                return -inicialspeed;
            }
            else
            {
                return inicialspeed;
            }
        }
        else
        {
            return inicialspeed;
        }
    }
}
