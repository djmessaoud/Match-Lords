using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject optionsPanel;
    [SerializeField]
    private GameObject exitPanel;
    [SerializeField]
    private GameObject MenuButtons;
    [SerializeField]
    private Sprite onSprite;
    [SerializeField]
    private Sprite offSprite;
    [SerializeField]
    private GameObject musicSwitch;
    [SerializeField]
    private GameObject soundSwitch;
    private bool _music;
    private bool _sound;
    private const float _musicVolume = 0.45f;
    private const float _soundVolume = 1f;
    AudioSource[] _audioSources;
    // Start is called before the first frame update
    void Start()
    {
        _audioSources = GetComponents<AudioSource>();
        updateOptionsUI();
     Debug.Log($"music : {_music} sound : {_sound}");
    }
    void updateOptionsUI()
    {
        //Get settings from preferences
        _music = PlayerPrefs.GetInt("musicON", 1) == 1;
        _sound = PlayerPrefs.GetInt("soundON", 1) == 1;
        Debug.Log($"UPDATE UI FUNCTION |||| music : {_music} sound : {_sound}");
        //Music setting update on UI
        if (_music)
        {
            musicSwitch.GetComponent<Image>().sprite = onSprite;
            _audioSources[0].volume = _musicVolume;
            Debug.Log("added on sprite to music");
        }
        else
        {
            musicSwitch.GetComponent<Image>().sprite = offSprite;
            _audioSources[0].volume = 0;
            Debug.Log("added off sprite to music");
        }//Sound setting update
        if (_sound)
        {
            soundSwitch.GetComponent<Image>().sprite = onSprite;
            _audioSources[1].volume = _soundVolume;
        }
        else
        {
            soundSwitch.GetComponent<Image>().sprite = offSprite;
            _audioSources[1].volume = 0;
        }
    }
    void updateOptions(bool music, bool sound)
    {
        if (music)
        {
            PlayerPrefs.SetInt("musicON", 1);
        }
        else
        {
            PlayerPrefs.SetInt("musicON", 0);
        }
        if (sound)
        {
            PlayerPrefs.SetInt("soundON", 1);
        }
        else
        {
            PlayerPrefs.SetInt("soundON", 0);
        }
    }
    public void musicChange()
    {
        _audioSources[1].Play();
        if (_music)
        {
            _music = false;
            musicSwitch.GetComponent<Image>().sprite = offSprite;
            _audioSources[0].volume = 0;
        }
        else
        {
            _music = true;
            musicSwitch.GetComponent<Image>().sprite = onSprite;
            _audioSources[0].volume = _musicVolume;
        }
        Debug.Log($"music : {_music} sound : {_sound}");
    }
    public void soundChange()
    {
        _audioSources[1].Play();
        if (_sound)
        {
            _sound = false;
            soundSwitch.GetComponent<Image>().sprite = offSprite;
            _audioSources[1].volume = 0;
        }
        else
        {
            _sound = true;
            soundSwitch.GetComponent <Image>().sprite = onSprite;
            _audioSources[1].volume = _soundVolume;
        }
        Debug.Log($"music : {_music} sound : {_sound}");
    }
    public  void startGame()
    {
        _audioSources[1].Play();
        SceneManager.LoadScene("SelectLevel");
    }
    public void viewOptions()
    {
        _audioSources[1].Play();
        updateOptionsUI();
        MenuButtons.SetActive(false);
        optionsPanel.SetActive(true);
    }
    public void closeOptions()
    {
        _audioSources[1].Play();
        updateOptions(_music, _sound);
        MenuButtons.SetActive(true);
        optionsPanel.SetActive(false);
    }
    public void closeExit()
    {
        _audioSources[1].Play();
        MenuButtons.SetActive(true);
        exitPanel.SetActive(false);
    }
    public void exitGame()
    {
        _audioSources[1].Play();
        MenuButtons.SetActive(false);
        exitPanel.SetActive(true);
    }
    public void closeApp()
    {
        Application.Quit();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
