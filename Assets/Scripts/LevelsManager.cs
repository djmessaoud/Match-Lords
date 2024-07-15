using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] levelButtons; // Assign level buttons in the inspector
    AudioSource[] _audioSources;
    void Start()
    {
        _audioSources = GetComponents<AudioSource>();
        _audioSources[0].volume = (PlayerPrefs.GetInt("musicON", 1) == 1) ? 0.15f : 0;
        _audioSources[1].volume = (PlayerPrefs.GetInt("soundON", 1) == 1) ? 0.25f : 0;
        int maxLevel = PlayerPrefs.GetInt("maxLevel", 1);
        
        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i + 1 <= maxLevel)
            {
                int levelIndex = i + 1; // Local copy for the closure
                levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        _audioSources[1].Play();
        PlayerPrefs.SetInt("currentLevel", levelIndex);
        SceneManager.LoadScene("PlayingScene");
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
