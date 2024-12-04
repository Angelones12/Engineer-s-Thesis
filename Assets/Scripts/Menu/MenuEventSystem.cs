using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuEventSystem : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private GameObject firstSelected;

    protected virtual void OnEnable()
    {
        StartCoroutine(SetFirstSelected(firstSelected));
    }

    public IEnumerator SetFirstSelected(GameObject firstSelectedObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstSelectedObject);
    }
}