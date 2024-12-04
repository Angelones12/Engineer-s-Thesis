using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw_Movement : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody rb;
    private Transform currentPoint;
    public float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB.transform;

    }
    void Update()
    {
        Vector3 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        { rb.velocity = new Vector3(speed, 0, 0); }
        else { rb.velocity = new Vector3(-speed, 0, 0); }

        if (Vector3.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointB.transform)
        {
            currentPoint = pointA.transform;
        }
        if (Vector3.Distance(transform.position, currentPoint.position) < 1f && currentPoint == pointA.transform)
        {
            currentPoint = pointB.transform;
        }
    }
}
