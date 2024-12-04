using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Scripts.Data;
using UnityEngine.UI;
using System.ComponentModel.Design;
using System.Diagnostics;

public class LevelMenu : MonoBehaviour
{
    public TextMeshProUGUI[] starsCountTexts;
    public Image starImagePrefab;
    public float starSpacing;
    public Button[] buttons;

    private GameData data;
    string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt(profileId + "UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    private void Start()
    {
        string profileId = DataPersistenceManager.instance.GetSelectedProfileId();

        foreach (var starsCountText in starsCountTexts)
        {
            string levelIdString = starsCountText.name.Substring(5);
            if (int.TryParse(levelIdString, out int levelId))
            {
                string levelTag = "Level" + levelId;
                int pointsCollected = PlayerPrefs.GetInt(profileId + "_" + levelTag + "_StarsCollected", 0);
                UnityEngine.Debug.Log(pointsCollected);
                DisplayStars(starsCountText, pointsCollected);
            }
            else
            {
                UnityEngine.Debug.LogError("Nie udało się sparsować numeru poziomu");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void DisplayStars(TextMeshProUGUI starsCountText, int points)
    {
        foreach (Transform child in starsCountText.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < points; i++)
        {
            Image starImage = Instantiate(starImagePrefab, starsCountText.transform);

            float xPos = i * (starImage.rectTransform.sizeDelta.x + starSpacing);
            starImage.rectTransform.anchoredPosition = new Vector2(xPos, 0f);
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level_" + levelId;
        SceneManager.LoadScene(levelName);
    }
}
