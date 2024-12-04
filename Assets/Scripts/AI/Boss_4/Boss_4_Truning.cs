using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_4_Turning : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        player = GameObject.Find("queen")?.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);

            // Check if the player is on the right side
            if (player.position.x > transform.position.x)
            {
                // If on the right side, rotate the boss on the y-axis
                transform.rotation = Quaternion.Euler(0, 45, 0);
            }
            else
            {
                // If on the left side, reset the y-axis rotation
                transform.rotation = Quaternion.Euler(0, 120, 0);
            }
        }
    }
}
