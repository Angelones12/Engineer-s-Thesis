using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scripts.Data;

public class Boss_Stats : MonoBehaviour
{
    [Header("Stats")]
    public int boss_1_health;
    public int boss_1_speed;
    public int boss_1_attackRange;
    public Slider healthBar;
    public GameObject objectToThrow;
    public Transform throwPoint;
    public ParticleSystem fanfareParticle;
    public Transform fanfareSpawnPoint;

    private PlayerAnimation playerAnimation;

    private Boss_Healing boss_Healing;

    private int boss_1_max_Hp;

    public int bosshalfhp;
    private int bossquarterhp;
    private Animator animator;
    private int enragespeed;

    private static DataPersistenceManager dataPersistenceManager;
    string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

    private void Start()
    {
        playerAnimation = FindObjectOfType<PlayerAnimation>();
        animator = GetComponent<Animator>();

        bosshalfhp = boss_1_health / 2;
        bossquarterhp = boss_1_health / 4;
        enragespeed = boss_1_speed * 2;
        boss_1_max_Hp = boss_1_health;
        healthBar.maxValue = boss_1_max_Hp;
        healthBar.value = boss_1_health;
        healthBar.interactable = false;
    }

    public void TakeDamage(int damage)
    {

        boss_1_health -= damage;
        healthBar.value = boss_1_health;
        if (boss_1_health < bosshalfhp)
        {

            if (animator != null)
            {

                animator.SetBool("Is_Enraged", true);
                boss_1_speed = enragespeed;

            }
        }
        if (boss_1_health <= bossquarterhp && animator.GetBool("Have_Healed") == false)
        {

            animator.SetBool("Is_Healing", true);
        }

        if (boss_1_health <= 0)
        {
            animator.SetTrigger("Is_Dying");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon") && playerAnimation.isAttacking)
        {
            TakeDamage(1);
            
            if (animator.GetBool("HealingRightNow") == true)
            {
                animator.SetBool("Is_Interrupted", true);

            }
        }
        if (other.CompareTag("Projectile"))
        {
            TakeDamage(3);
            
            if (animator.GetBool("HealingRightNow") == true)
            {
                animator.SetBool("Is_Interrupted", true);

            }
        }
    }

    public void Die()
    {
        if (fanfareParticle != null)
        {
            Vector3 spawnPosition = fanfareSpawnPoint != null ? fanfareSpawnPoint.position : transform.position;
            spawnPosition += Vector3.up * 4f;
            ParticleSystem particleInstance = Instantiate(fanfareParticle, spawnPosition, Quaternion.identity);
            particleInstance.Play();
            float particleDuration = 2f;
            PlayerPrefs.SetInt(profileId + "UnlockedLevel", 2);

            Invoke("LoadLevelScene", particleDuration);
        }
    }

    private void LoadLevelScene()
    {
        SceneManager.LoadScene("LevelsPanel");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, boss_1_attackRange);
    }
}

