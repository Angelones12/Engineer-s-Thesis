using UnityEngine;

public class MagicMissileController : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float verticalOffset;

    public void SetTarget(Transform newTarget, float missileSpeed, float offset)
    {
        target = newTarget;
        speed = missileSpeed;
        verticalOffset = offset;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + Vector3.up * verticalOffset;

            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        { 
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Projectile"))
        {

            Destroy(gameObject);
        }
    }
}
