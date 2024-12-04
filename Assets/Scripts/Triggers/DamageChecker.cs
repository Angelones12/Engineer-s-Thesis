using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChecker : MonoBehaviour
{
    private Character character;

    private void Start()
    {
        character = GetComponentInParent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && character.GetHealth() > 0)
        {
            character.TakeDamage();
        }
    }
}
