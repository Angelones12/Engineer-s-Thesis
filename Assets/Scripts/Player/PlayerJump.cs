using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using UnityEngine;
using Scripts;
using System;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float jumpPower = 6f;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private float jumpSpeed = 2.5f;
    [SerializeField] private float gravity = -9.81f;

    private bool isJumping = false;
    private bool canDoubleJump = false;

    private KeyCode jumpKey;

    private void Awake()
    { 

        jumpKey = GetKey("Jump");
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            if (groundChecker.IsGrounded())
            {
                isJumping = true;
                canDoubleJump = true;
                animator.SetTrigger("JumpTrigger");
            }
            else if (canDoubleJump)
            {
                isJumping = true;
                canDoubleJump = false;
                animator.SetTrigger("JumpTrigger");
            }
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0f);
            rigidbody.AddForce(new Vector2(0, jumpPower), ForceMode.Impulse);
            rigidbody.AddForce(new Vector2(0, gravity * jumpSpeed), ForceMode.Force);
            isJumping = false;
        }
        if (rigidbody.velocity.y < 0)
        {
            rigidbody.AddForce(new Vector2(0, gravity * jumpSpeed/2), ForceMode.Acceleration);
        }
    }

    private KeyCode GetKey(string keyName)
    {
        string keyString = PlayerPrefs.GetString("CustomKey_" + keyName, keyName == "MoveLeft" ? KeyCode.A.ToString() : KeyCode.D.ToString());
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
    }

}
