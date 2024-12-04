using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Turning : MonoBehaviour
{

    public Transform player;
    private float distance;
    public bool isFlipped = false;

    public void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;

            if (transform.position.x > player.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                isFlipped = true;
            }
            else if (transform.position.x < player.position.x && isFlipped)
            {
                transform.localScale = flipped;
                isFlipped = false;
            }
        }
    }

    public float CalculateDistance() {
        distance = transform.position.x - player.position.x;
        return distance;
    }

    }