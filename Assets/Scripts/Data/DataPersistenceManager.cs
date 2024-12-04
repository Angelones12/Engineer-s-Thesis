using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Specialized;

namespace Scripts.Data
{
    public class DataPersistenceManager : MonoBehaviour
    {
        [SerializeField] private string fileName;

        private GameData gameData;
        private List<IDataPersistence> dataPersistenceObjects;
        private FileDataHandler dataHandler;
        private string selectedProfileId = "";
        private HashSet<Vector3> usedCampfires = new HashSet<Vector3>();

        public static DataPersistenceManager instance { get; private set; }

        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);

            this.dataHandler = new FileDataHandler(UnityEngine.Application.persistentDataPath, fileName);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            CampfireArea.OnEnteredCampfireArea += OnEnteredCampfireArea;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            CampfireArea.OnEnteredCampfireArea -= OnEnteredCampfireArea;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            this.dataPersistenceObjects = FindAllDataPersistenceObjects();
            LoadGame();
        }

        public void OnSceneUnloaded(Scene scene)
        {
            SaveGame();
        }

        public void ChangeSelectedProfileId(string newProfileId)
        {
            this.selectedProfileId = newProfileId;
            LoadGame();
        }


        public void NewGame()
        {
            this.gameData = new GameData();
            SaveGame();
        }

        public void LoadGame()
        {
            this.gameData = dataHandler.Load(selectedProfileId);

            if (this.gameData == null)
            {
                UnityEngine.Debug.Log("No data was found. Initializing data to defaults");
                NewGame();
            }

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.LoadData(gameData);
            }
        }

        public void SaveGame()
        {

            foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
            {
                dataPersistenceObj.SaveData(ref gameData);
            }
            dataHandler.Save(gameData, selectedProfileId);
        }

        public void SaveCollectedPoints(int points)
        {
            GameData gameData = new GameData();
            gameData.pointsCollected = points;
            dataHandler.Save(gameData, selectedProfileId);
        }

        private void OnEnteredCampfireArea(bool entered, Vector3 position)
        {
            if (entered && !usedCampfires.Contains(position))
            {

                foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
                {
                    if (dataPersistenceObj is Character character)
                    {
                        character.HealToMaxHealth();
                    }
                }

                SaveGame();

                usedCampfires.Add(position);
            }
        }

        private List<IDataPersistence> FindAllDataPersistenceObjects()
        {
            IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
            return new List<IDataPersistence>(dataPersistenceObjects);
        }

        public Dictionary<string, GameData> GetAllProfilesGameData()
        {
            return dataHandler.LoadAllProfiles();
        }

        public void ResetGameData()
        {
            this.gameData = new GameData();
        }

        public string GetSelectedProfileId()
        {
            return selectedProfileId;
        }

        public GameData GameData
        {
            get { return gameData; }
            set { gameData = value; }
        } 
        
    }

}