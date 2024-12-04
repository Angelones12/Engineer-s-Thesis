using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Data;

public class Boss_2_Stats : MonoBehaviour
{
    [Header("Stats")]
    public int boss_2_health;
    public int boss_2_speed;
    public int boss_2_attackRange;
    public int jumpPower;

    public GameObject prefab;

    private PlayerAnimation playerAnimation;
    private Transform player;
    public Animator animator;
    public Rigidbody rb;
    public ParticleSystem fanfareParticle;
    public Transform fanfareSpawnPoint;

    public float tauntCooldownTime;
    private float initialTauntCooldownTime;
    public float knockBackForce;
    public float knochBackArea;
    private bool cooldownTaunt = true;


    public float jumpWithWaveCooldownTime;
    private float initialJumpWithWaveCooldownTime;
    private bool cooldownJumpWithWave = true;


    public float rollCooldownTime;
    private float initialRollCooldownTime;
    private bool cooldownRoll;

    public float fallFlatCooldownTime;
    private float initialFallFlatCooldownTime;
    private bool cooldownFallFlat;

    public float jumpCooldownTime;
    private float initialJumpCooldownTime;
    private bool cooldownJump;

    private static DataPersistenceManager dataPersistenceManager;
    string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

    private KeyCode jumpKey;
    void Start()
    {
        player = GameObject.Find("queen")?.transform;
        playerAnimation = FindObjectOfType<PlayerAnimation>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        initialTauntCooldownTime = tauntCooldownTime;
        initialJumpWithWaveCooldownTime = jumpWithWaveCooldownTime;
        initialRollCooldownTime = rollCooldownTime;
        initialFallFlatCooldownTime = fallFlatCooldownTime;
    }
    private void Awake()
    {

        jumpKey = GetKey("Jump");

    }


    void Update()
    {
        TauntRunner();
        JumpWithWaveAttackRunner();
        RollAttackRunner();
        FallFlatAttackRunner();
        JumpAttackRunner();

    }
    private void JumpAttackRunner()
    {
        if (!cooldownJump)
        {
            jumpCooldownTime -= Time.deltaTime;
            if (jumpCooldownTime <= 0f)
            {
                cooldownJump = true;
            }
        }

        if (PlayerInKnockBackArea() && cooldownJump && Input.GetKeyDown(jumpKey))
        {
            animator.SetTrigger("JumpAttack");
            cooldownJump = false;
            jumpCooldownTime = initialJumpCooldownTime;
        }
    }
    private void FallFlatAttackRunner()
    {
        if (!cooldownFallFlat)
        {
            fallFlatCooldownTime -= Time.deltaTime;
            if (fallFlatCooldownTime <= 0f)
            {
                cooldownFallFlat = true;
            }
        }

        if (!PlayerInKnockBackArea() && cooldownFallFlat)
        {
            animator.SetTrigger("Fall_Flat");
            cooldownFallFlat = false;
            fallFlatCooldownTime = initialFallFlatCooldownTime;
        }
    }

    private void RollAttackRunner() {
        if (!cooldownRoll)
        {
            rollCooldownTime -= Time.deltaTime;
            if (rollCooldownTime <= 0f)
            {
                cooldownRoll = true;
            }
        }

        if (!PlayerInKnockBackArea() && cooldownRoll)
        {
            animator.SetTrigger("Roll");
            cooldownRoll = false;
            rollCooldownTime = initialRollCooldownTime;
        }


    }
    private void JumpWithWaveAttackRunner()
    {
        if (!cooldownJumpWithWave)
        {
            jumpWithWaveCooldownTime -= Time.deltaTime;
            if (jumpWithWaveCooldownTime <= 0f)
            {
                cooldownJumpWithWave = true;
                
            }
        }

        if (!PlayerInKnockBackArea() && cooldownJumpWithWave)
        {
            animator.SetTrigger("JumpAttackWithWave");
            cooldownJumpWithWave = false;
            jumpWithWaveCooldownTime = initialJumpWithWaveCooldownTime;
        }
    }

    private void TauntRunner()
    {
        if (!cooldownTaunt)
        {
            tauntCooldownTime -= Time.deltaTime;
            if (tauntCooldownTime <= 0f)
            {
                cooldownTaunt = true;
            }
        }

        if (cooldownTaunt && PlayerInKnockBackArea())
        {
            cooldownTaunt = false;
            animator.SetTrigger("Taunt");
            tauntCooldownTime = initialTauntCooldownTime;
        }
    }

    private bool PlayerInKnockBackArea()
    {
        if (player != null)
        {
            float playerX = player.position.x;
            float boss2X = rb.position.x;
            if (Mathf.Abs(playerX - boss2X) < knochBackArea)
            {
                return true;
            }
        }

        return false;
    }

    private void TakeDamage(int damage) {
        boss_2_health-=damage;

        if (boss_2_health <= 0)
        {
            animator.SetTrigger("Dying");
            if (fanfareParticle != null)
            {
                Vector3 spawnPosition = fanfareSpawnPoint != null ? fanfareSpawnPoint.position : transform.position;
                spawnPosition += Vector3.up * 4f;
                ParticleSystem particleInstance = Instantiate(fanfareParticle, spawnPosition, Quaternion.identity);
                particleInstance.Play();
                float particleDuration = 2f;
                PlayerPrefs.SetInt(profileId + "UnlockedLevel", 3);

                Invoke("LoadLevelScene", particleDuration);
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
    private KeyCode GetKey(string keyName)
    {
        string keyString = PlayerPrefs.GetString("CustomKey_" + keyName,
            keyName == "MoveLeft" ? KeyCode.A.ToString() : KeyCode.D.ToString());
        return (KeyCode)Enum.Parse(typeof(KeyCode), keyString);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,knochBackArea);
    }
}
