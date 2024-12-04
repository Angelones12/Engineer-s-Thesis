using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private GroundChecker groundChecker;
    [SerializeField] private string isMovingParameterName = "IsMoving";
    [SerializeField] private string isGroundedParameterName = "IsGrounded";
    [SerializeField] private string isFallingParameterName = "IsFalling";

    private int _isMovingHash;
    private int _isGroundedHash;
    private int _isFallingHash;

    private Character character;
    public bool isAttacking;

    private KeyCode attack;

    private void Start()
    {
        _isMovingHash = Animator.StringToHash(isMovingParameterName);
        _isGroundedHash = Animator.StringToHash(isGroundedParameterName);
        _isFallingHash = Animator.StringToHash(isFallingParameterName);

        character = GetComponent<Character>();
        attack = GetKey("Attack");
    }

    private void Update()
    {
        bool isMoving = movement.IsMoving();
        animator.SetBool(_isMovingHash, isMoving);
        animator.SetBool(_isGroundedHash, groundChecker.IsGrounded());

        bool isFalling = rigidbody.velocity.y < -0.01f;
        animator.SetBool(_isFallingHash, isFalling);

        if (Input.GetKeyDown(attack) && !PauseMenu.GameIsPaused)
        {
            isAttacking = true;
            animator.SetTrigger("AttackTrigger");
            StartCoroutine(WaitForAttackAnimation());
        }

        if (isMoving && character.canMove)
        {
            float horizontalInput = movement.GetHorizontalInput();
            if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(0.4f, 0.4f, -0.4f);
            }
            else if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
        }
    }

    private IEnumerator WaitForAttackAnimation()
    {
        yield return new WaitForSeconds(1.2f); 

        isAttacking = false;
    }

    private KeyCode GetKey(string keyName)
    {
        string keyString = PlayerPrefs.GetString("CustomKey_" + keyName, keyName == "MoveLeft" ? KeyCode.A.ToString() : KeyCode.D.ToString());
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
    }
}
