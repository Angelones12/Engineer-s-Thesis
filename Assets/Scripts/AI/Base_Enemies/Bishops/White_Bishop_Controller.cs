using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class White_Bishop_Controller : MonoBehaviour
{
    public int hp;
    public float speed;
    public Transform pointA;
    public Transform pointB;

    private Rigidbody rb;
    private Transform currentPoint;



    private PlayerAnimation playerAnimation;
    private bool isAlive = true;
    public GameObject explosionEffectPrefab;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB;
        playerAnimation = FindObjectOfType<PlayerAnimation>();

    }

    void Update()
    {
        if (isAlive)
        {
            Vector3 direction = (currentPoint.position - transform.position).normalized;

            rb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);

            if (Vector3.Distance(transform.position, currentPoint.position) < 0.5f)
            {
                if (currentPoint == pointA)
                {
                    currentPoint = pointB;
                }
                else
                {
                    currentPoint = pointA;
                }
            }
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && playerAnimation.isAttacking)
        {
            TakeDamage(1);
            playerAnimation.isAttacking = false;
        }
        else if (other.CompareTag("Projectile"))
        {
            TakeDamage(3);
        }
    }
    private void DestroyEnemy()
    {
        UnityEngine.Debug.Log("DestroyEnemy method executed");
        isAlive = false;

        Vector3 enemyPosition = transform.position;

        float yOffset = 2.3f;
        enemyPosition.y += yOffset;

        StartCoroutine(WaitForDeathAnimation(enemyPosition));
    }
    private IEnumerator WaitForDeathAnimation(Vector3 enemyPosition)
    {
        Instantiate(explosionEffectPrefab, enemyPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
