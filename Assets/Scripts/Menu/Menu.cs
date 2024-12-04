using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Menu : MenuEventSystem
{
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    public void PlayGame()
    {
        PlayerPrefs.SetInt("NewGameClicked", 1);

        saveSlotMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }

    public void OnLoadGame()
    {
        saveSlotMenu.ActivateMenu(true);
        PlayerPrefs.DeleteKey("NewGameClicked");
        for (int i = 1; i <= 4; i++) 
        {
            for (int profileId = 0; i <= 2; i++)
            {
                PlayerPrefs.DeleteKey(profileId + "ResetPointsFlag_Level_" + i);
            }
        }
        this.DeactivateMenu();
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }

    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
