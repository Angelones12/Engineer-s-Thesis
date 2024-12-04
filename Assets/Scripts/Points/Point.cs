using System.Collections.Generic;
using UnityEngine;
using Scripts.Data;
using System.Diagnostics;
using System.Collections;

public class Point : MonoBehaviour
{
    [SerializeField] private ParticleSystem collectParticle;
    [SerializeField] private GameObject particleSpawnPoint;
    [SerializeField] private int value;
    public bool isCollected;

    private string PlayerPrefsKey => "Point_" + gameObject.name + "_IsCollected";

    private static DataPersistenceManager dataPersistenceManager;
    private string levelTag;
    private int currentLevel;

    string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

    void Awake()
    {
        StartCoroutine(FindDataPersistenceManager());
    }

    private IEnumerator FindDataPersistenceManager()
    {
        yield return new WaitForSeconds(0.1f);
        dataPersistenceManager = FindObjectOfType<DataPersistenceManager>();
        profileId = DataPersistenceManager.instance.GetSelectedProfileId();
        levelTag = gameObject.tag;
        currentLevel = int.Parse(levelTag.Substring(5));
       
        if (PlayerPrefs.GetInt(profileId + "ResetPointsFlag_Level_" + currentLevel, 0) == 1)
        {
            ResetPoints();
            PlayerPrefs.SetInt(profileId + "ResetPointsFlag_Level_" + currentLevel, 0);
            PlayerPrefs.Save();
        }
        else
        {
            isCollected = PlayerPrefs.GetInt(PlayerPrefsKey, 0) == 1;

            if (isCollected)
            {
                gameObject.SetActive(false);
            }
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CollectPoint();
        }
    }

    void CollectPoint()
    {
        isCollected = true;
        PlayerPrefs.SetInt(PlayerPrefsKey, 1);
        PlayerPrefs.Save();
    
        PointCounter.instance.IncreasePoints(value, profileId, levelTag);

        SaveCollectedPoints();

        if (collectParticle != null && particleSpawnPoint != null)
        {
            StartCoroutine(PlayCollectParticle());
        }

        gameObject.SetActive(false);
    }

    private IEnumerator PlayCollectParticle()
    {
        ParticleSystem particleInstance = Instantiate(collectParticle, particleSpawnPoint.transform.position, Quaternion.identity);
        particleInstance.Play();
        yield return new WaitForSeconds(particleInstance.main.duration);
        Destroy(particleInstance.gameObject);
    }

    private void SaveCollectedPoints()
    {
        if (dataPersistenceManager != null)
        {
            dataPersistenceManager.GameData.pointsCollected += value;
            int totalCollectedPoints = dataPersistenceManager.GameData.pointsCollected;
            dataPersistenceManager.SaveCollectedPoints(totalCollectedPoints);
        }
        else
        {
            UnityEngine.Debug.LogError("DataPersistenceManager is null");
        }
    }

    public static void ResetPoints()
    {
        Point[] points = FindObjectsOfType<Point>();

        foreach (Point point in points)
        {
            point.isCollected = false;
            PlayerPrefs.SetInt(point.PlayerPrefsKey, 0);  
            PlayerPrefs.Save();

            point.gameObject.SetActive(true);
        }
    }
}


