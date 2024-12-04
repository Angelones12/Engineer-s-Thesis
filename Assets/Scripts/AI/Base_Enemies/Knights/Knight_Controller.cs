using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knight_Controller : MonoBehaviour
{
    [Header("For Patrolling")]
    public int hp;
    public float speed;
    public float areaOfSight;
    public float attackSpeed;

    private float moveDirection = 1;
    private bool facingRight = false;

    public Transform groundCheckPoint;
    public Transform wallCheckPoint;
    public float sphereRadius;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    private bool cooldownAttack;
    private float attackCooldownTimer;
    private float rotationSpeed = 5.0f;

    [Header("For Jumping")]
    public float jumpHeight;
    private Transform player;
    public Transform groundCheck;
    public Vector3 boxSize;
    [SerializeField] private bool isGrounded;

    private bool checkingGround;
    private bool checkingWall;
    private bool checkingEnemy;

    private Rigidbody rb;

    private Animator animator;
    private PlayerAnimation playerAnimation;
    private bool isAlive = true;
    public GameObject explosionEffectPrefab;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("queen")?.transform;
        animator = GetComponent<Animator>();
        playerAnimation = FindObjectOfType<PlayerAnimation>();
    }

    void FixedUpdate()
    {
        Collider[] groundColliders = Physics.OverlapSphere(groundCheckPoint.position, sphereRadius, groundLayer);
        checkingGround = groundColliders.Length > 0;

        Collider[] wallColliders = Physics.OverlapSphere(wallCheckPoint.position, sphereRadius, groundLayer);
        checkingWall = wallColliders.Length > 0;

        Collider[] enemyCollidersUsingTag = Physics.OverlapSphere(wallCheckPoint.position, sphereRadius);
        checkingEnemy = enemyCollidersUsingTag.Any(collider => collider.CompareTag("Enemy"));


        Collider[] isgroundColliders = Physics.OverlapBox(groundCheck.position, boxSize / 2f, Quaternion.identity, groundLayer);
        isGrounded = isgroundColliders.Length > 0;
        if (isAlive)
        {
            if (!PlayerInSight() && isGrounded)
            {
               
                Patrolling();
            }
            if (cooldownAttack)
            {
                attackCooldownTimer -= Time.deltaTime;
                if (attackCooldownTimer <= 0f)
                {
                    cooldownAttack = false;
                }
            }
            if (!cooldownAttack)
            {
                
                if (PlayerInSight() && isGrounded)
                {
                    JumpAttack();
                    FlipTowardsPlayer();
                    cooldownAttack = true;
                    attackCooldownTimer = 1f / attackSpeed;

                }

            }
        }
        }
    private void JumpAttack()
    {
        float distanceFromPlayer = player.position.x - transform.position.x;
        if (isGrounded)
        {
            rb.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode.Impulse);
        }
    }




    private void Patrolling() {
        if (!checkingGround || checkingWall || checkingEnemy) {
            if (facingRight)
            {
                Flip();


            }
            else if (!facingRight) {
                Flip();
            
            }
        }
        rb.velocity = new Vector3(speed * moveDirection, rb.velocity.y, 0);
    
    }

    private void FlipTowardsPlayer() {
        float playerPosition = player.position.x - transform.position.x;
        if (playerPosition > 0 && facingRight == true)
        {
            Flip();
        }
        else if (playerPosition < 0 && !facingRight) {
            Flip();
        }
    }
    private void Flip() {
        moveDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private bool PlayerInSight()
    {
        if (player != null)
        {
            float playerX = player.position.x;
            float pawnX = rb.position.x;
            float playerY = player.position.y;
            float pawnY = rb.position.y;
            if (Mathf.Abs(playerX - pawnX) < areaOfSight && Mathf.Abs(playerY - pawnY) < areaOfSight)
            {
                return true;
            }
        }

        return false;
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
        animator.SetTrigger("knightDeathTrigger");

        Vector3 enemyPosition = transform.position;

        float yOffset = 2.3f;
        enemyPosition.y += yOffset;

        StartCoroutine(WaitForDeathAnimation(enemyPosition));
    }
    private IEnumerator WaitForDeathAnimation(Vector3 enemyPosition)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Instantiate(explosionEffectPrefab, enemyPosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPoint.position, sphereRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (wallCheckPoint.position, sphereRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(groundCheck.position, boxSize);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(areaOfSight, areaOfSight, areaOfSight));
    }
}
