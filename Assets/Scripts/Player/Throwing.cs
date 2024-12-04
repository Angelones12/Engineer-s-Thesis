using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Throwing : MonoBehaviour
{
    [Header("References")]
    public Transform attackPoint;
    public GameObject objectToThrow;
    public Transform playerTransform;
    public GroundChecker groundChecker;
    public Slider cooldownSlider;

    [Header("Settings")]
    public float throwCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    private bool readyToThrow;

    public Animator playerAnimator;
    private PlayerAnimation playerAnimation;

    private void Start()
    {
        readyToThrow = true;

        if (playerTransform == null)
        {
            playerTransform = transform;
        }

        playerAnimator = GetComponent<Animator>();
        playerAnimation = FindObjectOfType<PlayerAnimation>();
        groundChecker = GetComponentInChildren<GroundChecker>();

        if (cooldownSlider != null)
        {
            cooldownSlider.minValue = -10f;
            cooldownSlider.maxValue = 0f;
            cooldownSlider.value = 0f;

            cooldownSlider.interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && groundChecker.IsGrounded() && !playerAnimation.isAttacking)
        {
            readyToThrow = false;

            playerAnimator.SetTrigger("ShootTrigger");

            StartCoroutine(ThrowAfterAnimation(0.5f));

            StartCoroutine(UpdateCooldownSlider());
        }
    }

    private IEnumerator ThrowAfterAnimation(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);

        Throw();
    }

    private void Throw()
    {
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, attackPoint.rotation);

        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection;

        if (playerTransform.localScale.z > 0f)
        {
            forceDirection = attackPoint.transform.forward;
        }
        else
        {
            forceDirection = -attackPoint.transform.forward;
        }

        Vector3 forceToAdd = forceDirection * throwForce + attackPoint.transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        StartCoroutine(ResetThrow());
    }

    private IEnumerator ResetThrow()
    {
        yield return new WaitForSeconds(throwCooldown);

        readyToThrow = true;

        if (cooldownSlider != null)
        {
            cooldownSlider.value = 0f;
        }
    }

    private IEnumerator UpdateCooldownSlider()
    {
        float elapsedTime = 0f;

        while (elapsedTime < throwCooldown)
        {
            if (cooldownSlider != null)
            {
                cooldownSlider.value = Mathf.Lerp(-10f, 0f, elapsedTime / throwCooldown);
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        if (cooldownSlider != null)
        {
            cooldownSlider.interactable = false;
        }
    }
}
