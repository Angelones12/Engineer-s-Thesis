using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashDistance = 5f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown = 2f;

    private float lastDashTime;
    private bool isDashing;

    private PlayerMovement movement;
    private Animator animator;
    private Character character;

    private KeyCode lastKeyPressed;

    private KeyCode dash;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
        character = GetComponent<Character>();

        dash = GetKey("Dash");
    }

    private void Update()
    {
        if (character.GetHealth() > 0) 
        {
            if (Time.time - lastDashTime >= dashCooldown && Input.GetKeyDown(dash))
            {
                Dash();
            }
        }

        if (isDashing)
        {
            float dashDirection = (transform.localScale.z > 0) ? 1f : -1f;
            Vector3 dashVector = new Vector3(dashDirection, 0f, 0f) * dashDistance;
            movement.ApplyExternalForce(dashVector / dashDuration);

            isDashing = false;

            lastDashTime = Time.time;
        }
    }

    private void Dash()
    {
        isDashing = true;
        Invoke("StopDashing", dashDuration);
    }

    private void StopDashing()
    {
        isDashing = false;
    }

    private KeyCode GetKey(string keyName)
    {
        string keyString = PlayerPrefs.GetString("CustomKey_" + keyName, keyName == "MoveLeft" ? KeyCode.A.ToString() : KeyCode.D.ToString());
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
    }
}
