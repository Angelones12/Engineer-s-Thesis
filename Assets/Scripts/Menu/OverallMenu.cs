using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallMenu : MonoBehaviour
{
    public void ActivateMenu()
    {
        gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        gameObject.SetActive(false);
    }
}
