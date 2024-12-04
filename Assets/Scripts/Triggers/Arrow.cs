using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Camera mainCamera;


    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ground") || other.CompareTag("Projectile"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        CheckIfOutOfScreen();
    }

    void CheckIfOutOfScreen()
    {
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

        if (viewportPoint.x < 0 || viewportPoint.x > 1 || viewportPoint.y < 0 || viewportPoint.y > 1)
        {
            Destroy(gameObject);
        }
    }
}
