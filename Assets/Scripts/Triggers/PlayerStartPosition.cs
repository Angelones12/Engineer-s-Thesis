using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStartPosition : MonoBehaviour
{
    public Character character;
    public string levelName;


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Vector3.zero;

            character = player.GetComponent<Character>();

            character.HealthSystem.OnDeath += OnPlayerDeath;

        }
    }

    private void OnPlayerDeath()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(levelName);
    }

}
