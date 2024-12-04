using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Keybind : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonLbl;

    private string playerPrefsKey;
    private KeyCode defaultKeyCode;

    void Start()
    {
        playerPrefsKey = "CustomKey_" + gameObject.name;

        if (buttonLbl != null)
        {
            switch (gameObject.name)
            {
                case "MoveLeft":
                    defaultKeyCode = KeyCode.A;
                    break;
                case "MoveRight":
                    defaultKeyCode = KeyCode.D;
                    break;
                case "Jump":
                    defaultKeyCode = KeyCode.Space;
                    break;
                case "Attack":
                    defaultKeyCode = KeyCode.Mouse0;
                    break;
                case "Dash":
                    defaultKeyCode = KeyCode.LeftShift;
                    break;
                default:
                    defaultKeyCode = KeyCode.Space;
                    break;
            }
        }

        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            buttonLbl.text = PlayerPrefs.GetString(playerPrefsKey);
        }
        else
        {
            buttonLbl.text = defaultKeyCode.ToString();
            PlayerPrefs.SetString(playerPrefsKey, defaultKeyCode.ToString());
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if(Input.GetKey(keycode) && buttonLbl.text == "Wciœnij klawisz")
            {
                buttonLbl.text = keycode.ToString();
                PlayerPrefs.SetString(playerPrefsKey, keycode.ToString());
                PlayerPrefs.Save();
            }
        }
    }

    public void ChangeKey()
    {
        buttonLbl.text = "Wciœnij klawisz";
    }

    public KeyCode GetKey()
    {
        return (KeyCode)Enum.Parse(typeof(KeyCode), buttonLbl.text);
    }
}
