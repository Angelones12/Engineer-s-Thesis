using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Data;

public class ArenaEntrance : MonoBehaviour
{
    public string bossArenaSceneName = "Arena_1";

    private static DataPersistenceManager dataPersistenceManager;
    string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt(profileId + "UnlockedLevel", 0) == 3)
            {
                PlayerPrefs.SetInt(profileId + "UnlockedLevel", 4);
            }
            SceneManager.LoadScene(bossArenaSceneName);
           
        }
    }
}