using System.Collections;
using System.Collections.Generic;
//using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEditor;
using UnityEngine;



public class Black_Pawn_Controller : MonoBehaviour
{

    [Header("Stats")]
    public int hp;
    public int speed;
    public int attackRange;
    public float attackSpeed;
    public int areaOfSight;

    [Header("Points")]
    public GameObject pointA;
    public GameObject pointB;
    private Transform currentPoint;

    private bool cooldownAttack;
    private float attackCooldownTimer;
    private float rotationSpeed = 5.0f;


    Rigidbody rb;
    Transform player;
    private Animator animator;
    private PlayerAnimation playerAnimation;
    private bool isAlive = true;
    public GameObject explosionEffectPrefab;

    void Start()
    {
        player = GameObject.Find("queen")?.transform;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerAnimation = FindObjectOfType<PlayerAnimation>();
        currentPoint = pointA.transform;

        cooldownAttack = false;
        attackCooldownTimer = 0f;
    }


    void Update()
    {
        if (isAlive)
        {
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
                if (Vector3.Distance(player.position, rb.position) < attackRange)
                {
                    Debug.Log("Attack");
                    animator.SetTrigger("Attack");
                    cooldownAttack = true;
                    attackCooldownTimer = 1f / attackSpeed;
                }
            }

            if (PlayerInSight() && player != null)
            {
                ChasingPlayer();
                LookAtPlayer();
            }
            else
            {
                MovingBetweenPoints();
            }
        }
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
    private void MovingBetweenPoints()
    {
        if (!PlayerInSight() || player == null)
        {
            FaceCurrentPoint();
            Vector3 point = currentPoint.position - transform.position;
            int zero = 0;
            if (currentPoint == pointB.transform)
            {
                rb.velocity = new Vector3(speed, zero, zero);
            }
            else
            {
                rb.velocity = new Vector3(-speed, zero, zero);
            }

            if (Vector3.Distance(transform.position, currentPoint.position) < 1f)
            {

                if (!PlayerInSight() || player == null)
                {
                    if (currentPoint == pointB.transform)
                    {
                        currentPoint = pointA.transform;
                    }
                    else
                    {
                        currentPoint = pointB.transform;
                    }
                    TurnToObject();
                }
            }
        }
    }

    private void FaceCurrentPoint()
    {
        if (currentPoint != null)
        {
            Vector3 directionToFace = currentPoint.position - transform.position;
            directionToFace.y = 0;
            directionToFace = Quaternion.Euler(0, 90, 0) * directionToFace;
            if (directionToFace != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToFace);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void TurnToObject()
    {
        if (rb.velocity.x < 0)
        {

            rb.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (rb.velocity.x > 0)
        {

            rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }





    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 lookDirection = player.position - transform.position;
            if (lookDirection.x < 0)
            {

                rb.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                rb.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
    }




    private void ChasingPlayer()
    {
        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        float distanceToPlayer = Vector3.Distance(rb.position, target);

        if (distanceToPlayer > 0.1f)
        {
            float direction = Mathf.Sign(target.x - rb.position.x);
            rb.velocity = new Vector3(speed * 1.5f * direction, 0, 0);

            float clampedX = Mathf.Clamp(rb.position.x, pointA.transform.position.x, pointB.transform.position.x);
            rb.position = new Vector3(clampedX, rb.position.y, rb.position.z);
        }
    }
    public void TakeDamage(int damage)
    {
        hp-=damage;

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
        animator.SetTrigger("pawnDeathTrigger");

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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, areaOfSight);
    }
}
