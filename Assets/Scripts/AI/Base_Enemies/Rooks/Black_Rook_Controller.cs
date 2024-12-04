using System.Collections;
using UnityEngine;

public class Black_Rook_Controller : MonoBehaviour
{
    private Transform playerTransform;
    public GameObject magicMissilePrefab;
    public Transform shootingPoint;
    public float shootCooldown;
    public float magicMissileSpeed;
    public float magicMissileLifetime;
    public GameObject explosionEffectPrefab;
    public int hp;

    private bool readyToShoot = true;
    private PlayerAnimation playerAnimation;
    private float shootingDistance = 22f;

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

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= shootingDistance && readyToShoot)
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
        GameObject magicMissile = Instantiate(magicMissilePrefab, shootingPoint.position, Quaternion.identity);
        MagicMissileController missileController = magicMissile.GetComponent<MagicMissileController>();

        float verticalOffset = 3f;
        missileController.SetTarget(playerTransform, magicMissileSpeed, verticalOffset);

        Destroy(magicMissile, magicMissileLifetime);
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
