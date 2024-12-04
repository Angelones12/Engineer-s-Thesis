using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private string groundTag = "Ground";

    private int groundedObjectsCount;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(groundTag))
        {
            groundedObjectsCount++;
            isGrounded = groundedObjectsCount > 0;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag(groundTag))
        {
            groundedObjectsCount--;
            isGrounded = groundedObjectsCount > 0;
        }
    }

    private bool isGrounded;

    public bool IsGrounded()
    {
        return isGrounded;
    }

}
