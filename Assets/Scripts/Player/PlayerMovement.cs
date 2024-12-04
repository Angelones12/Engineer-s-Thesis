using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using Scripts.Data;
using System;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float externalForceMultiplier = 10f;
    [SerializeField] private Rigidbody rigidbody;

    private Vector2 input;
    private Character character;

    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;

    private void Awake()
    {
        character = GetComponent<Character>();
        moveLeftKey = GetKey("MoveLeft");
        moveRightKey = GetKey("MoveRight");
    }

    private void Update()
    {
        float moveInput = 0f;

        if (Input.GetKey(moveLeftKey))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(moveRightKey))
        {
            moveInput = 1f;
        }


        input = new Vector2(moveInput, 0);

    }

    private void FixedUpdate()
    {
        if (character.canMove)
        {
            Vector3 move = input * moveSpeed * Time.fixedDeltaTime;
            rigidbody.velocity = new Vector2(move.x, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    public void ApplyExternalForce(Vector3 force)
    {
        rigidbody.AddForce(force * externalForceMultiplier, ForceMode.Impulse);
    }

    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    public bool IsMoving()
    {
        return input.x != 0;
    }

    public void UpdateInput(Vector2 newInput)
    {
        input = newInput;
    }

    private KeyCode GetKey(string keyName)
    {
        string keyString = PlayerPrefs.GetString("CustomKey_" + keyName, keyName == "MoveLeft" ? KeyCode.A.ToString() : KeyCode.D.ToString());
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
    }

    public float GetHorizontalInput()
    {
        if (Input.GetKey(moveLeftKey))
        {
            return -1f;
        }
        else if (Input.GetKey(moveRightKey))
        {
            return 1f;
        }

        return 0f;
    }

}
