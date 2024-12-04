using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class EnemyAI3D : MonoBehaviour
{
    [SerializeField] int hp;
    [SerializeField] float speed;
    [SerializeField] int areaOfSight;
    [SerializeField] Vector3 moveDirection = new Vector3(-1f, 0.25f, 0f);
    [SerializeField] GameObject rightCheck,leftCheck, roofCheck, groundCheck;
    [SerializeField] Vector3 rightCheckSize,leftCheckSize , roofCheckSize, groundCheckSize;
    [SerializeField] LayerMask groundLayer, cameraborder;
    [SerializeField] bool goingUp = true;
    bool enterState = true;

    public Transform player;
    

    private bool touchedGround, touchedRoof, touchedRight,touchedLeft;
    private Rigidbody rb;
    [Header("ForPatrolling")]
    private bool isPatrolling;
    public Transform pointA;
    public Transform pointB;
    private Transform currentPoint;

    private bool isInCamera, isOutsideCamera;

    [Header("BetweenCamera")]
    public Transform leftPoint;
    public Transform rightPoint;
    public Transform topPoint;

    private PlayerAnimation playerAnimation;
    private bool isAlive = true;
    public GameObject explosionEffectPrefab;
    void Start()
    {
        playerAnimation = FindObjectOfType<PlayerAnimation>();
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB;
    }

    void Update()
    {
        if (EnemyBetweenCamera()) { isPatrolling = false; HitLogic(); }
    }

    void FixedUpdate()
    {
        if (!EnemyBetweenCamera() && !PlayerInSight()) { Patrolling(); }
        if (!isPatrolling) { rb.velocity = moveDirection * speed; }
    }

    void HitLogic()
    {
        touchedRight = HitDetector(rightCheck, rightCheckSize, groundLayer | cameraborder);
        touchedLeft = HitDetector(leftCheck, leftCheckSize, groundLayer | cameraborder);
        touchedRoof = HitDetector(roofCheck, roofCheckSize, groundLayer | cameraborder);
        touchedGround = HitDetector(groundCheck, groundCheckSize, groundLayer | cameraborder);
        if (touchedLeft) 
        {
            Flip();
        }
        if (touchedRight)
        {
            Flip();
        }
        if (touchedRoof && goingUp)
        {
            ChangeYDirection();
        }
        if (touchedGround && !goingUp)
        {
            ChangeYDirection();
        }
    }

    bool HitDetector(GameObject gameObject, Vector3 size, LayerMask layer)
    {
        return Physics.OverlapBox(gameObject.transform.position, size / 2, Quaternion.identity, layer).Length > 0;
    }


    void ChangeYDirection()
    {
        moveDirection.y = -moveDirection.y;
        goingUp = !goingUp;
    }

    void Flip()
    {
        moveDirection.x = -moveDirection.x;
        Debug.Log("Flipping!");
    }
    void Patrolling() {
        isPatrolling = true;
        Vector3 direction = (currentPoint.position - transform.position).normalized;
        
        rb.velocity = new Vector3(direction.x * speed, direction.y * speed, 0);

        if (Vector3.Distance(transform.position, currentPoint.position) < 1f)
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
    private bool EnemyBetweenCamera() {

        if (player != null)
        {
            float enemyX = rb.position.x;
            float enemyY = rb.position.y;
            float leftbarier = leftPoint.position.x;
            float rightbarier = rightPoint.position.x;
            float topbarier = topPoint.position.y;
            if (enemyX > leftbarier && enemyY < topbarier && enemyX < rightbarier)
            {
                
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && playerAnimation.isAttacking)
        {
            Flip();
            TakeDamage(1);
            playerAnimation.isAttacking = false;
        }
        else if (other.CompareTag("Projectile"))
        {
            Flip();
            TakeDamage(3);
        }
        if (other.CompareTag("Player")) {
            Flip();
        }
    }

    public void TakeDamage(int damage)
    {
        hp-=damage;

        if (hp <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawWireCube(roofCheck.transform.position, roofCheckSize);
        Gizmos.DrawWireCube(rightCheck.transform.position, rightCheckSize);
        Gizmos.DrawWireCube(leftCheck.transform.position, leftCheckSize);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, areaOfSight);
    }
}
