using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.SceneManagement;
using Scripts.Data;

public class Boss_4_Stats : MonoBehaviour
{
    public int hp;
    public float speed;
    public GameObject PointToRespawnTowerA;
    public GameObject PointToRespawnTowerB;
    public GameObject PointToRespawnKnights;

    private Rigidbody playerRigidbody;
    private Rigidbody rb;
    public GameObject RookPrefab;
    public GameObject KnightPrefab;
    public float sphereRadius;
    public float SpawnKnightCooldownTime;
    private float initialSpawnKnightCooldownTime;
    private bool cooldownSpawnKnight;

    public float SpawnRooktCooldownTime;
    private float initialRookCooldownTime;
    private bool cooldownSpawnRook;

    public ParticleSystem fanfareParticle;
    public Transform fanfareSpawnPoint;

    [Header("Shield")]
    public int shieldHp;
    private int maxshieldHp;
    public GameObject PointToRespawnShields;
    public GameObject ShieldPrefab;
    public float SpawnShiledCooldownTime;
    private float initialShieldCooldownTime;
    public bool isShielded;
    private bool cooldownSpawnShield;

    private GameObject ShieldInstance;


    private bool pointAOccupied;
    private bool pointBOccupied;
    
    private Transform player;

    [Header("Falling_Rocks")]
    public GameObject RockPrefab;

    public GameObject PointToRespawnRockA;
    public GameObject PointToRespawnRockB;
    public GameObject PointToRespawnRockC;

    public float SpawnRockCooldownTime;
    private float initialRockCooldownTime;
    private bool cooldownSpawnRock;


    private PlayerAnimation playerAnimation;
    private bool isAlive = true;
    public GameObject explosionEffectPrefab;

    private static DataPersistenceManager dataPersistenceManager;
    string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

    void Start()
    {
        player = GameObject.Find("queen")?.transform;
        rb = GetComponent<Rigidbody>();
        initialSpawnKnightCooldownTime = SpawnKnightCooldownTime;
        initialRookCooldownTime = SpawnRooktCooldownTime;
        initialRockCooldownTime = SpawnRockCooldownTime;
        initialShieldCooldownTime = SpawnShiledCooldownTime;
        isShielded = false;
        playerAnimation = FindObjectOfType<PlayerAnimation>();
        maxshieldHp = shieldHp;
    }


    void Update()
    {
        Collider[] enemyCollidersUsingTagPointA = Physics.OverlapSphere(PointToRespawnTowerA.transform.position, sphereRadius);
        pointAOccupied = enemyCollidersUsingTagPointA.Any(collider => collider.CompareTag("Enemy"));

        Collider[] enemyCollidersUsingTagPointB = Physics.OverlapSphere(PointToRespawnTowerB.transform.position, sphereRadius);
        pointBOccupied = enemyCollidersUsingTagPointB.Any(collider => collider.CompareTag("Enemy"));
        SpawnKnightRunner();
        SpawnRookRunner();
        SpawnRockRunner();
        SpawnShieldRunner();
        ChasingPlayer();
    }

    private void ChasingPlayer() {
        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);
    }
    private void SpawnShieldRunner()
    {
        if (!isShielded)
        {
            if (!cooldownSpawnShield)
            {
                SpawnShiledCooldownTime -= Time.deltaTime;
                if (SpawnShiledCooldownTime <= 0f)
                {
                    cooldownSpawnShield = true;
                }
            }

            if (cooldownSpawnShield)
            {
                shieldHp = maxshieldHp;
                ShieldInstance = Instantiate(ShieldPrefab, PointToRespawnShields.transform.position, Quaternion.identity);
                ShieldInstance.transform.parent = rb.transform;
                cooldownSpawnShield = false;
                isShielded = true;
                SpawnShiledCooldownTime = initialShieldCooldownTime;
            }
        }
    }


    private void SpawnRockRunner() {
        if (!cooldownSpawnRock)
        {
            SpawnRockCooldownTime -= Time.deltaTime;
            if (SpawnRockCooldownTime <= 0f)
            {
                cooldownSpawnRock = true;
            }
        }

        if (cooldownSpawnRock)
        {
            int chosePoint = Random.Range(1, 3);
            Debug.Log(chosePoint);
            switch (chosePoint) {
                case 1:
                    GameObject prefabInstance1 = Instantiate(RockPrefab, PointToRespawnRockA.transform.position, Quaternion.identity);
                    break;
                case 2:
                    GameObject prefabInstance2 = Instantiate(RockPrefab, PointToRespawnRockB.transform.position, Quaternion.identity);
                    break;
                default:
                    GameObject prefabInstance3 = Instantiate(RockPrefab, PointToRespawnRockC.transform.position, Quaternion.identity);
                    break;
            }
                

            cooldownSpawnRock = false;
            SpawnRockCooldownTime = initialRockCooldownTime;
        }


    }
    private void SpawnRookRunner()
    {
        if (!pointAOccupied || !pointBOccupied)
        {
            if (!cooldownSpawnRook)
            {
                SpawnRooktCooldownTime -= Time.deltaTime;
                if (SpawnRooktCooldownTime <= 0f)
                {
                    cooldownSpawnRook = true;
                }
            }

            if (cooldownSpawnRook)
            {
                if (!pointBOccupied)
                {
                    GameObject prefabInstance1 = Instantiate(RookPrefab, PointToRespawnTowerB.transform.position, Quaternion.Euler(0f, -90f, 0f));
                }
                if (!pointAOccupied && pointBOccupied)
                {
                    GameObject prefabInstance1 = Instantiate(RookPrefab, PointToRespawnTowerA.transform.position, Quaternion.Euler(0f, -90f, 0f));
                }
                cooldownSpawnRook = false;
                SpawnRooktCooldownTime = initialRookCooldownTime;
            }
        }
    }

    private void SpawnKnightRunner()
    {
        if (!cooldownSpawnKnight)
        {
            SpawnKnightCooldownTime -= Time.deltaTime;
            if (SpawnKnightCooldownTime <= 0f)
            {
                cooldownSpawnKnight = true;
            }
        }

        if (cooldownSpawnKnight)
        {
            GameObject prefabInstance1 = Instantiate(KnightPrefab, PointToRespawnKnights.transform.position, Quaternion.identity);
            cooldownSpawnKnight = false;
            SpawnKnightCooldownTime = initialSpawnKnightCooldownTime;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isShielded) { shieldHp-=damage; if (shieldHp <= 0) { Destroy(ShieldInstance); isShielded = false; } }
        else if (!isShielded)
        {
            hp -= damage;

            if (hp <= 0)
            {
                if (fanfareParticle != null)
                {
                    Vector3 spawnPosition = fanfareSpawnPoint != null ? fanfareSpawnPoint.position : transform.position;
                    spawnPosition += Vector3.up * 4f;
                    ParticleSystem particleInstance = Instantiate(fanfareParticle, spawnPosition, Quaternion.identity);
                    particleInstance.Play();
                    float particleDuration = 2f;
                    PlayerPrefs.SetInt(profileId + "UnlockedLevel", 5);

                    Invoke("LoadLevelScene", particleDuration);
                }
                
            }

        }
    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene("LevelsPanel");
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(PointToRespawnTowerA.transform.position, sphereRadius);
        Gizmos.DrawWireSphere(PointToRespawnTowerB.transform.position, sphereRadius);
    }
}
