using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class White_Rook_Controller : MonoBehaviour
{
    private Transform playerTransform;
    public GameObject arrowPrefab;
    public Transform shootingPoint;
    public float shootCooldown;
    public float arrowSpeed;
    public float arrowLifetime;
    public GameObject explosionEffectPrefab;
    public int hp;

    private bool readyToShoot = true;
    private PlayerAnimation playerAnimation;

    void Start()
    {
        playerTransform = GameObject.Find("queen")?.transform;
        playerAnimation = FindObjectOfType<PlayerAnimation>();
    }

    void Update()
    {
        if (playerTransform == null)
        {
            UnityEngine.Debug.LogError("PlayerTransform not assigned in the inspector!");
            return;
        }

        FlipTowardsPlayer();

        if (readyToShoot)
        {
            Shoot();
            StartCoroutine(ResetShootCooldown());
        }
    }

    void FlipTowardsPlayer()
    {
        float directionToPlayer = playerTransform.position.x - transform.position.x;

        if (directionToPlayer < 0)
        {
            transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        }
        else if (directionToPlayer > 0)
        {
            transform.localScale = new Vector3(1.3f, 1.3f, -1.3f);
        }
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();

        arrow.transform.localScale = new Vector3(arrow.transform.localScale.x, arrow.transform.localScale.y, arrow.transform.localScale.z * transform.localScale.z);

        Vector3 shootDirection = (playerTransform.position + new Vector3(0f, 3f, 0f) - shootingPoint.position).normalized;
        arrowRb.AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);

        Destroy(arrow, arrowLifetime);
    }

    IEnumerator ResetShootCooldown()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(shootCooldown);
        readyToShoot = true;
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

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    private void DestroyEnemy()
    {
        Vector3 enemyPosition = transform.position;

        float yOffset = 4f;
        enemyPosition.y += yOffset;

        StartCoroutine(WaitForDeathAnimation(enemyPosition));
    }
    private IEnumerator WaitForDeathAnimation(Vector3 enemyPosition)
    {
        Instantiate(explosionEffectPrefab, enemyPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }
}
