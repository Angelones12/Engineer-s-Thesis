using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_5_Stats : MonoBehaviour
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

    private bool pointAOccupied;
    private bool pointBOccupied;

    private Transform player;

    private PlayerAnimation playerAnimation;
    private bool isAlive = true;
    public GameObject explosionEffectPrefab;

    [Header("Spikes")]
    public GameObject PointForSpikeA;
    public GameObject PointForSpikeB;
    public GameObject spikePrefab;

    public float SpawnSpikeCooldownTime;
    private float initialSpikeCooldownTime;
    private bool cooldownSpawnSpike;

    [Header("ThunderBolt")]
    public GameObject PointForThunderBolt;
    public GameObject ThunderBoltprefab;

    public float SpawnThunderCooldownTime;
    private float initialThunderCooldownTime;
    private bool cooldownSpawnThunder;


    private KeyCode jumpKey;

    void Start()
    {
        player = GameObject.Find("queen")?.transform;
        rb = GetComponent<Rigidbody>();
        initialSpawnKnightCooldownTime = SpawnKnightCooldownTime;
        initialRookCooldownTime = SpawnRooktCooldownTime;
        initialSpikeCooldownTime = SpawnSpikeCooldownTime;
        initialThunderCooldownTime = SpawnThunderCooldownTime;
        playerAnimation = FindObjectOfType<PlayerAnimation>();
    }

    private void Awake()
    {

        jumpKey = GetKey("Jump");

    }

    void Update()
    {
        Collider[] enemyCollidersUsingTagPointA = Physics.OverlapSphere(PointToRespawnTowerA.transform.position, sphereRadius);
        pointAOccupied = enemyCollidersUsingTagPointA.Any(collider => collider.CompareTag("Enemy"));

        Collider[] enemyCollidersUsingTagPointB = Physics.OverlapSphere(PointToRespawnTowerB.transform.position, sphereRadius);
        pointBOccupied = enemyCollidersUsingTagPointB.Any(collider => collider.CompareTag("Enemy"));
        SpawnKnightRunner();
        SpawnRookRunner();
        ChasingPlayer();
        SpikeRunner();
        ThunderBoltRunner();
    }

    private void ThunderBoltRunner() {

        if (!cooldownSpawnThunder)
        {
            SpawnThunderCooldownTime -= Time.deltaTime;
            if (SpawnThunderCooldownTime <= 0f)
            {
                cooldownSpawnThunder = true;
            }
        }

        if (cooldownSpawnThunder && Input.GetKeyDown(jumpKey))
        {

            GameObject prefabInstance1 = Instantiate(ThunderBoltprefab, PointForThunderBolt.transform.position, Quaternion.Euler(0f, 0, -180f));

            prefabInstance1.transform.parent = transform;
            cooldownSpawnThunder = false;
            SpawnThunderCooldownTime = initialThunderCooldownTime;

            Destroy(prefabInstance1, 3f);
        }

    }
    private void ChasingPlayer()
    {
        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);
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
    private void SpikeRunner ()
    {
        
            if (!cooldownSpawnSpike)
            {
                SpawnSpikeCooldownTime -= Time.deltaTime;
                if (SpawnSpikeCooldownTime <= 0f)
                {
                    cooldownSpawnSpike = true;
                }
            }

            if (cooldownSpawnSpike)
            {
                
                    GameObject prefabInstance1 = Instantiate(spikePrefab, PointForSpikeA.transform.position, Quaternion.Euler(-90f, 0f, -90f));
                
                
                    GameObject prefabInstance2 = Instantiate(spikePrefab, PointForSpikeB.transform.position, Quaternion.Euler(-90f, 0f, 90f));

            prefabInstance1.transform.parent = transform;
            prefabInstance2.transform.parent = transform;

            cooldownSpawnSpike = false;
                SpawnSpikeCooldownTime = initialSpikeCooldownTime;
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
        if (fanfareParticle != null)
        {
            Vector3 spawnPosition = fanfareSpawnPoint != null ? fanfareSpawnPoint.position : transform.position;
            spawnPosition += Vector3.up * 4f;
            ParticleSystem particleInstance = Instantiate(fanfareParticle, spawnPosition, Quaternion.identity);
            particleInstance.Play();
            float particleDuration = 2f;
            PlayerPrefs.SetInt("UnlockedLevel", 5);

            Invoke("LoadLevelScene", particleDuration);
        }
        
    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private KeyCode GetKey(string keyName)
    {
        string keyString = PlayerPrefs.GetString("CustomKey_" + keyName,
            keyName == "MoveLeft" ? KeyCode.A.ToString() : KeyCode.D.ToString());
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(PointToRespawnTowerA.transform.position, sphereRadius);
        Gizmos.DrawWireSphere(PointToRespawnTowerB.transform.position, sphereRadius);
    }
}
