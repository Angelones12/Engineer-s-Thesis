using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scripts.Data;
public class SaveSlotMenu : MenuEventSystem
{
    [SerializeField] private Menu menu;

    [Header("Menu Navigation")]
    [SerializeField] private Button backButton;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        string profileId = saveSlot.GetProfileId();

        DisableMenuButtons();

        DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());

        if (PlayerPrefs.GetInt("NewGameClicked", 0) == 1)
        {
            PlayerPrefs.DeleteKey(profileId + "UnlockedLevel");
            PlayerPrefs.DeleteKey("NewGameClicked");

            for (int i = 1; i <= 4; i++)
            {
                PlayerPrefs.SetInt(profileId + "ResetPointsFlag_Level_" + i, 1);
            }

            for (int levelId = 1; levelId <= 4; levelId++)
            {
                string levelTag = "Level" + levelId;
                PlayerPrefs.DeleteKey(profileId + "_" + levelTag + "_StarsCollected");
            }
        }

        if (!isLoadingGame)
        {
            DataPersistenceManager.instance.NewGame();
        }
        
        SceneManager.LoadSceneAsync("LevelsPanel");
    }

    public void OnBackClicked()
    {
        menu.ActivateMenu();
        this.DeactivateMenu();
    }

    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame = isLoadingGame;
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        
        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }
        StartCoroutine(this.SetFirstSelected(firstSelected));
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }
}
