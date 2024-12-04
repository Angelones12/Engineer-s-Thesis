using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public Character character;
    private HealthSystem healthSystem;
    List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable()
    {
        Character.OnPlayerDamaged += DrawHearts;
        Character.OnPlayerRespawned += DrawHearts; 
    }

    private void OnDisable()
    {
        Character.OnPlayerDamaged -= DrawHearts;
        Character.OnPlayerRespawned -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();

        float maxHealthRemainder = character.maxHealth % 2;
        float heartsToMake = (character.maxHealth / 2 + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder = (int)Mathf.Clamp(character.HealthSystem.health - (i*2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }

    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heart = newHeart.GetComponent<HealthHeart>();
        heart.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heart);
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }
}
