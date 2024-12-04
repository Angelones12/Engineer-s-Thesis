using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public GameObject groundChecker;
    public float sphereRadius;
    public GameObject explosionEffectPrefab;

    private bool touchedGround;
    private bool touchedPlayer;
    private bool touchedEnemy;

    private bool isTriggered;
  
    void Start()
    {
        isTriggered = false;
    }

    void Update()
    {
        Collider[] enemyCollidersUsingTagPointA = Physics.OverlapSphere(groundChecker.transform.position, sphereRadius);
        touchedGround = enemyCollidersUsingTagPointA.Any(collider => collider.CompareTag("Ground"));
        touchedEnemy = enemyCollidersUsingTagPointA.Any(collider => collider.CompareTag("Enemy"));
        touchedPlayer = enemyCollidersUsingTagPointA.Any(collider => collider.CompareTag("Player"));
        if (touchedEnemy || touchedGround || touchedPlayer) { if (!isTriggered) { DestroyEnemy(); } }
    }
    private void DestroyEnemy()
    {
        isTriggered = true;
        Vector3 enemyPosition = transform.position;


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
        Gizmos.DrawWireSphere(groundChecker.transform.position, sphereRadius);
        
    }
}
