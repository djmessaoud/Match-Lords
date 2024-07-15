using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject GemBtnPrefab; // Assign GemButton prefab here
    public Transform GemsPanel; // Assign the Panel here
    public List<Sprite> gemSprites; // Assign the gem sprites here
    public Sprite defaultSprite; // Assign the default button image here
    public Text scoreText; // Assign a UI Text for score display
    public List<Sprite> goldenGems;
    public List<Text> goldenGemsScores;
    public GameObject winningPanel;
    public Text winningTextField;
    private List<Button> buttons = new List<Button>();
    private List<Sprite> selectedGems = new List<Sprite>();
    private int score = 0;
    private Button _firstGemSelected = null;
    private Button _secondGemSelected = null;
    public int _level = 1;
    private int _orangeGemsCollected = 0;
    private int _yellowGemsCollected = 0;
    private int _greenGemsCollected = 0;
    private int _blueGemsCollected = 0;
    private int _matchesNeeded = 10;
    AudioSource[] _audioSources;
    private const float _musicVolume = 0.45f;
    private const float _soundVolume = 1f;
    private const int _numberOfGems = 10; //number of different gems/cards to play with (will be multiplied by 2) 
    private const int _nLevels = 9;
    private AudioClip _musicToPlay;
    public List<AudioClip> playMusics;
    // Start is called before the first frame update
    void Start()
    {
        _level = PlayerPrefs.GetInt("currentLevel", 1);
        _audioSources = GetComponents<AudioSource>();
        //Music to play
        _musicToPlay = playMusics[UnityEngine.Random.Range(0,playMusics.Count-1)]; //select random music from the list
        _audioSources[0].clip = _musicToPlay;
        //Soundtracks volume
        _audioSources[0].volume = (PlayerPrefs.GetInt("musicON", 1) == 1) ? _musicVolume : 0; //Melody
        _audioSources[0].Play();
      //  _audioSources[0].volume = _audioSources[4].volume - 0.05f; //Beat
        //Sounds Volume
        _audioSources[1].volume = (PlayerPrefs.GetInt("soundON", 1) == 1) ? _soundVolume : 0;
        _audioSources[2].volume = _audioSources[1].volume - 0.5f; //Winning sound volume
        _audioSources[3].volume = _audioSources[2].volume - 0.21f; //Cards Matched sound volume
        InitializeLevel();
    }
    public void RestartLevel()
    {
        _audioSources[1].Play();
        cancelCollectedGems();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void cancelCollectedGems()
    {
        PlayerPrefs.SetInt("orangeGems", PlayerPrefs.GetInt("orangeGems", 0) - _orangeGemsCollected);
        PlayerPrefs.SetInt("yellowGems", PlayerPrefs.GetInt("yellowGems", 0) - _yellowGemsCollected);
        PlayerPrefs.SetInt("greenGems", PlayerPrefs.GetInt("greenGems", 0) - _greenGemsCollected);
        PlayerPrefs.SetInt("blueGems", PlayerPrefs.GetInt("blueGems", 0) - _blueGemsCollected);
        _orangeGemsCollected = 0; _yellowGemsCollected = 0; _blueGemsCollected = 0; _greenGemsCollected = 0;
    }
    public void OnNextLevelClicked()
    {
        _audioSources[1].Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitToMenu()
    {
        _audioSources[1].Play();
        cancelCollectedGems();
        SceneManager.LoadScene("SelectLevel");
    }
    // Update is called once per frame
    void InitializeLevel()
    {
        //
        _matchesNeeded = _numberOfGems;
        winningPanel.SetActive(false);
        updateGoldenGemsFields();
        GemsPanel.GetComponent<GridLayoutGroup>().enabled = true;
        //Create Gems buttons in the panel 
        for (int i = 0; i < _numberOfGems*2; i++) //creating buttons for both gems and their copies!
        {
            GameObject buttonObject = Instantiate(GemBtnPrefab, GemsPanel);
            Button button = buttonObject.GetComponent<Button>();
            button.image.sprite = defaultSprite;
            button.onClick.AddListener(() => GemButtonClicked(button));
            buttons.Add(button);
        }
        //Capture positions of the buttons after automatical adjustment from the GridLayoutGroup
        List<Vector3> positions = new List<Vector3>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(GemsPanel.GetComponent<RectTransform>());
        //Save the positions of buttons before disabling the GridLayoutGroup to avoid automatic re-positioning of cards (buttons)
        foreach (Button button in buttons)
        {
            positions.Add(button.transform.localPosition);
            Debug.Log($"Position  : {button.transform.localPosition}");
        }

        // Disable the GridLayoutGroup
        GemsPanel.GetComponent<GridLayoutGroup>().enabled = false;

        // Set the saved positions to the buttons
        for (int i = 0; i < buttons.Count; i++)
             buttons[i].transform.localPosition = positions[i];

        //Select random 10 Gems from the list of 15 Gems
        List<Sprite> tempList = new List<Sprite>(gemSprites);
        for (int i = 0; i < _numberOfGems; i++)
        {
            int j = UnityEngine.Random.Range(0, tempList.Count);
            Sprite selectedGem = tempList[j];
            selectedGems.Add(selectedGem);
            selectedGems.Add(selectedGem); // add 2nd copy of the gem
            tempList.RemoveAt(j);
        }

        //Shuffle the list of the selected gems before adding them to buttons
        for (int i = 0; i < selectedGems.Count; i++)
        {
            Sprite tempSprite = selectedGems[i];
            int j = UnityEngine.Random.Range(0, selectedGems.Count);
            selectedGems[i] = selectedGems[j];
            selectedGems[j] = tempSprite;
        }

        // Assign gems to buttons
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<GemButtonHandler>().SetGem(selectedGems[i]);
        }
    }
    //update the golden gems numbers in preferences function
    void updateGoldenGemsFields()
    {
        goldenGemsScores[0].text = PlayerPrefs.GetInt("orangeGems", 0).ToString();
        goldenGemsScores[1].text = PlayerPrefs.GetInt("greenGems", 0).ToString();
        goldenGemsScores[2].text = PlayerPrefs.GetInt("yellowGems", 0).ToString();
        goldenGemsScores[3].text = PlayerPrefs.GetInt("blueGems", 0).ToString();
    }
    //Show winning panel!
    void ShowWinningPanel()
    {
        winningTextField.text = $"LEVEL {_level}\nCOMPLETE";
        winningPanel.SetActive(true);
        
             _audioSources[2].Play();
        // Open next level for the player and save it into preferences!
        int maxLevel = PlayerPrefs.GetInt("maxLevel", 1);
        if ((_level == maxLevel) && _level < _nLevels)
        {
            PlayerPrefs.SetInt("maxLevel", maxLevel + 1);
        }
    }
    void GemButtonClicked(Button clickedBtn)
    {
        _audioSources[1].Play();
        if (_firstGemSelected == null)
        {
            _firstGemSelected = clickedBtn;
            _firstGemSelected.image.sprite = _firstGemSelected.GetComponent<GemButtonHandler>().GetGem();
        }
        else if (_secondGemSelected == null && clickedBtn != _firstGemSelected)
        {
            _secondGemSelected = clickedBtn;
            _secondGemSelected.image.sprite = _secondGemSelected.GetComponent<GemButtonHandler>().GetGem();
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 1 second

        if (_firstGemSelected.GetComponent<GemButtonHandler>().GetGem() == _secondGemSelected.GetComponent<GemButtonHandler>().GetGem())
        {
            // gems selected are a MATCH! disable both and increment score
            _firstGemSelected.gameObject.SetActive(false);
            _secondGemSelected.gameObject.SetActive(false);
            score += 20;
            //Play sound
            _audioSources[3].Play();
            //Check if gems are golden (Rare)
            var Gem = _firstGemSelected.GetComponent<GemButtonHandler>().GetGem();
            for (int i = 0; i < goldenGems.Count; i++)
            {
                if (Gem == goldenGems[i])
                {
                    // Save goldenGems globally
                    switch (i)
                    {
                        case (0):
                            {
                                PlayerPrefs.SetInt("orangeGems", PlayerPrefs.GetInt("orangeGems", 0) + 1);
                                _orangeGemsCollected++;
                                break;
                            }
                        case (1):
                            {
                                PlayerPrefs.SetInt("greenGems", PlayerPrefs.GetInt("greenGems", 0) + 1);
                                _greenGemsCollected++;
                                break;
                            }
                        case (2):
                            {
                                PlayerPrefs.SetInt("yellowGems", PlayerPrefs.GetInt("yellowGems", 0) + 1);
                                _yellowGemsCollected++;
                                break;
                            }
                        case (3):
                            {
                                PlayerPrefs.SetInt("blueGems", PlayerPrefs.GetInt("blueGems", 0) + 1);
                                _blueGemsCollected++;
                                break;
                            }
                    }
                    //Save locally to level
                    //   goldenGemsScores[i].text = (Int32.Parse(goldenGemsScores[i].text) + 1).ToString();
                    score += 15 + _level;
                    updateGoldenGemsFields();
                    break;
                }
            }
            _matchesNeeded--;
            Debug.Log($"Matches needed : {_matchesNeeded}");
            if (_matchesNeeded == 0)
            {
               // PlayerPrefs.SetInt("currentLevel", _level + 1);
                ShowWinningPanel();
            }
            scoreText.text = score.ToString();
        }
        else
        {
            // No match
            _firstGemSelected.image.sprite = defaultSprite;
            _secondGemSelected.image.sprite = defaultSprite;
            //decrement score
            score -= _level;
            if (score < 0) score = 0;
            scoreText.text = score.ToString();
        }

        _firstGemSelected = null;
        _secondGemSelected = null;
    }
}

