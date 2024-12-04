using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
namespace Scripts.Data
{
    [System.Serializable]
    public class GameData
    {
        public float health;
        public Vector3 playerPosition;
        public int pointsCollected;
        public int deathCounter;

        public GameData()
        {
            health = 6;
            playerPosition = Vector3.zero;
            pointsCollected = 0;
            deathCounter = 0;
        }

        public int GetPercentageComplete()
        {
            int totalPoints = 3;
            int percentageCompleted = 0;
            if (pointsCollected != 0)
            {
                percentageCompleted = (pointsCollected * 100 / totalPoints);
            }
            return percentageCompleted;
        }
    }
}