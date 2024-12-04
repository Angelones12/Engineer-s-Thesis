using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enraged : StateMachineBehaviour
{
    private MonoBehaviour monoBehaviour;
    Transform player;
    Rigidbody rb;
    Boss_Turning boss_turning;
    private int speed;
    private int attackRange;
    Boss_Stats bossStats;
    private GameObject bossWeapon;
    private int throwForce = 20;
    private int throwUpwardForce = 10;
    private bool isAttacking = false;
    private bool alreadyThrowed = false;
    private Transform throwPoint;
    private string weaponCloneTag = "Enemy";


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monoBehaviour = animator.GetComponent<MonoBehaviour>();
        player = GameObject.Find("queen")?.transform;
        rb = animator.GetComponent<Rigidbody>();
        boss_turning = animator.GetComponent<Boss_Turning>();
        bossStats = animator.GetComponent<Boss_Stats>();
        if (bossStats != null)
        {
            speed = bossStats.boss_1_speed;
            attackRange = bossStats.boss_1_attackRange;
            bossWeapon = bossStats.objectToThrow;
            throwPoint = bossStats.throwPoint;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss_turning.LookAtPlayer();

        Vector3 target = new Vector3(player.position.x, rb.position.y, rb.position.z);
        rb.position = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);
        float distance = Vector3.Distance(player.position, rb.position);

        float epsilon = 0.01f;

        if (distance <= attackRange + epsilon)
        {
            Attack(animator);
        }
        else if(distance <= attackRange * 3.5 && distance >= attackRange * 2.5)
        {
            if (!alreadyThrowed)
            {
                monoBehaviour.StartCoroutine(ThrowWeaponCoroutine(animator));
            }
        }

    }

    public void Attack(Animator animator)
    {
        if (!isAttacking)
        {
            MonoBehaviour monoBehaviour = animator.GetComponent<MonoBehaviour>();

            monoBehaviour.StartCoroutine(AttackCoroutine(animator));
        }
    }

    private IEnumerator AttackCoroutine(Animator animator)
    {
        isAttacking = true;

        int randomAttack = Random.Range(1, 4);
        Debug.Log(randomAttack);

        switch (randomAttack)
        {
            case 1:
                animator.SetTrigger("Attack_Melee");
                Debug.Log("Attack_Melee");
                break;
            case 2:
                animator.SetTrigger("Attack_Kick");
                Debug.Log("Attack_Kick");
                break;
            case 3:
                animator.SetTrigger("Attack_BackHand");
                Debug.Log("Attack_BackHand");
                break;
            case 4:
                animator.SetTrigger("Attack_Slash");
                Debug.Log("Attack_Slash");
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);



        isAttacking = false;
    }

    private IEnumerator ThrowWeaponCoroutine(Animator animator)
    {
        alreadyThrowed = true;

        animator.SetTrigger("Attack_Throw");

        yield return new WaitForSeconds(1.1f);

        GameObject thrownWeapon = Instantiate(bossWeapon, throwPoint.position, throwPoint.rotation);

        bossWeapon.SetActive(false);

        thrownWeapon.tag = weaponCloneTag;

        thrownWeapon.transform.localScale = new Vector3(1f, 1f, 1f);

        Rigidbody weaponRb = thrownWeapon.GetComponent<Rigidbody>();

        BoxCollider weaponCollider = thrownWeapon.GetComponent<BoxCollider>();

        if (weaponRb == null)
        {
            weaponRb = thrownWeapon.AddComponent<Rigidbody>();
        }

        if (weaponCollider == null)
        {
            weaponCollider = thrownWeapon.AddComponent<BoxCollider>();
        }

        Vector3 throwDirection = (player.position - throwPoint.position).normalized;

        Vector3 forceToAdd = throwDirection * throwForce + Vector3.up * throwUpwardForce;

        weaponRb.AddForce(forceToAdd, ForceMode.Impulse);

        yield return new WaitForSeconds(1.3f);

        bossWeapon.SetActive(true);

        Destroy(thrownWeapon);

        alreadyThrowed = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack_Kick");
        animator.ResetTrigger("Attack_Melee");
        animator.ResetTrigger("Attack_BackHand");
        animator.ResetTrigger("Attack_Slash");
        animator.ResetTrigger("Attack_Throw");
    }
}
