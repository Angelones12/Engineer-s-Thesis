using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireArea : MonoBehaviour
{
    public delegate void CampfireAreaEvent(bool entered, Vector3 position);
    public static event CampfireAreaEvent OnEnteredCampfireArea;

    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            NotifyEnteredCampfireArea(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            NotifyEnteredCampfireArea(false);
        }
    }

    private void NotifyEnteredCampfireArea(bool entered)
    {
        OnEnteredCampfireArea?.Invoke(entered, transform.position);
    }
}

