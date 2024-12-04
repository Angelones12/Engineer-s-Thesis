using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Scripts.Data;

public class PointCounter : MonoBehaviour, IDataPersistence
{
    public static PointCounter instance;
    private Dictionary<string, int> levelPoints = new Dictionary<string, int>();
    private int currentPoints = 0;

    void Awake()
    {
        instance = this;
    }

    public void IncreasePoints(int p, string profileId, string levelTag)
    {
        string key = profileId + "_" + levelTag + "_StarsCollected";

        if (!levelPoints.ContainsKey(key))
        {
            levelPoints[key] = 0;
        }

        levelPoints[key] += p;
        UnityEngine.Debug.Log(levelPoints[key]);
        currentPoints += p;
        PlayerPrefs.SetInt(key, PlayerPrefs.GetInt(key, 0) + p);
        PlayerPrefs.Save();
    }

    public void LoadData(GameData data)
    {
        currentPoints = data.pointsCollected;
    }

    public void SaveData(ref GameData data)
    {
        data.pointsCollected = currentPoints;
    }

    public void ResetPoints()
    {
        levelPoints.Clear();
        currentPoints = 0;
    }

    public int GetCurrentPoints(string profileId, string levelTag)
    {
        string key = profileId + "_" + levelTag + "_StarsCollected";

        if (levelPoints.ContainsKey(key))
        {
            return levelPoints[key];
        }

        return 0;
    }
}
