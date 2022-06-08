using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Valve.VR;

public class MenuManager : MonoBehaviour
{
    [Header("Leaderboard Name Input Field")]
    public TMP_InputField textEntry;
    string text = "";

    [Space(5)]
    [Header("Difficulty select buttons")]
    public Button nextButton;
    public Button previousButton;

    //Used to determine selected difficulty and level
    private int difficultyIndex = 0;

    private int levelIndex = 0;

    //Difficulty UI text
    [Space(5)]
    [Header("Difficulty select text and array")]
    public string[] levelText;
    public TMP_Text levelTMPText;

    [Space(7)]
    //The different menus
    public GameObject[] gamemodes;
    public GameObject[] leaderboardlevels;
    public GameObject[] leaderboardDifficulty;
    public GameObject[] otherOptionsMenu;
    public GameObject[] startButtonArray;
    
    //Slider and volume management. THIS CAN BE MOVED SOMEWHERE ELSE IF NEEDED

    public static float MasterVolume = 1;
    public static float SFXVolume = 1;
    public static float AmbienceVolume = 1;
    public static float MusicVolume = 1;
    public Slider[] sliders;

    [SerializeField] private SceneController _SceneController;

    void Update()
    {
        //Sets the easy/intermediate/hard text in the difficulty select UI
        levelTMPText.text = levelText[difficultyIndex];

        SelectedLevel();

        SelectedScoreboard();
        
        //print(difficultyIndex);
        
    }



    private void OnEnable()
    {
        //Listens for keyboard clicks and if the keyboard closes
        SteamVR_Events.System(EVREventType.VREvent_KeyboardCharInput).Listen(OnKeyboard);
        SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Listen(OnKeyboardClosed);
    }

    private void Start()
    {
        SelectedLevel();
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = 1;
        }

        //StartCoroutine(LoadLights());
    }

    private IEnumerator LoadLights()
    {
        yield return new WaitForSeconds(5);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        
        yield return new WaitForEndOfFrame();
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
    }
    public void NextLevel()
    {
        HideStartButton();
        if (difficultyIndex < 2)
        {
            difficultyIndex++;
        }
        else
        {
            difficultyIndex = 0;
        }
    }

    public void PreviousLevel()
    {
        HideStartButton();
        if (difficultyIndex > 0)
        {
            difficultyIndex--;
        }
        else
        {
            difficultyIndex = 2;
        }
    }

    private void SelectedLevel()
    {
        if (difficultyIndex == 0)
        {
            leaderboardlevels[0].SetActive(true);
            leaderboardlevels[1].SetActive(false);
            leaderboardlevels[2].SetActive(false);
        }
        else if (difficultyIndex == 1)
        {
            leaderboardlevels[0].SetActive(false);
            leaderboardlevels[1].SetActive(true);
            leaderboardlevels[2].SetActive(false);
        }

        if (difficultyIndex == 2)
        {
            leaderboardlevels[0].SetActive(false);
            leaderboardlevels[1].SetActive(false);
            leaderboardlevels[2].SetActive(true);
        }
    }

    private void SelectedScoreboard()
    {
        if (difficultyIndex == 0)
        {
            gamemodes[0].SetActive(true);
            gamemodes[1].SetActive(false);
            gamemodes[2].SetActive(false);
        }
        else if (difficultyIndex == 1)
        {
            gamemodes[0].SetActive(false);
            gamemodes[1].SetActive(true);
            gamemodes[2].SetActive(false);
        }

        if (difficultyIndex == 2)
        {
            gamemodes[0].SetActive(false);
            gamemodes[1].SetActive(false);
            gamemodes[2].SetActive(true);
        }
    }

    //Called from UnityEvent button press
    public void BackToOtherOptions()
    {
        otherOptionsMenu[0].SetActive(true);
        otherOptionsMenu[1].SetActive(false);
        otherOptionsMenu[2].SetActive(false);
    }

    public void VolumeMenu()
    {
        otherOptionsMenu[0].SetActive(false);
        otherOptionsMenu[1].SetActive(true);
        otherOptionsMenu[2].SetActive(false);
    }

    public void CreditsMenu()
    {
        otherOptionsMenu[0].SetActive(false);
        otherOptionsMenu[1].SetActive(false);
        otherOptionsMenu[2].SetActive(true);
    }

    public void BackToLevelSelect()
    {
        SelectedScoreboard();
        gamemodes[3].SetActive(true);
        gamemodes[4].SetActive(false);
    }

    // Has player selected level 1 or 2 on the respective difficulty
    public void SelectedLevel(int levelselected)
    {
        levelIndex = levelselected;
        if (levelIndex == 0)
        {
            leaderboardDifficulty[0].SetActive(true);
            leaderboardDifficulty[1].SetActive(false);
            leaderboardDifficulty[2].SetActive(true);
            leaderboardDifficulty[3].SetActive(false);
            leaderboardDifficulty[4].SetActive(true);
            leaderboardDifficulty[5].SetActive(false);
            
            startButtonArray[0].SetActive(true);
            startButtonArray[1].SetActive(false);
            startButtonArray[2].SetActive(true);
            startButtonArray[3].SetActive(false);
            startButtonArray[4].SetActive(true);
            startButtonArray[5].SetActive(false);
            
        }
        else
        {
            leaderboardDifficulty[0].SetActive(false);
            leaderboardDifficulty[1].SetActive(true);
            leaderboardDifficulty[2].SetActive(false);
            leaderboardDifficulty[3].SetActive(true);
            leaderboardDifficulty[4].SetActive(false);
            leaderboardDifficulty[5].SetActive(true);
            
            startButtonArray[0].SetActive(false);
            startButtonArray[1].SetActive(true);
            startButtonArray[2].SetActive(false);
            startButtonArray[3].SetActive(true);
            startButtonArray[4].SetActive(false);
            startButtonArray[5].SetActive(true);
        }
    }

    // This code is being called from a UnityEvent (button/text field select)
    public void ShowKeyboard()
    {
        SteamVR.instance.overlay.ShowKeyboard(0, 0, 0, "Description", 256, "", 0);
        textEntry.text = "";
    }


    //Runs every SteamVR keyboard button press
    private void OnKeyboard(VREvent_t args)
    {
        print("Clicked something!");
        VREvent_Keyboard_t keyboard = args.data.keyboard;
        byte[] inputBytes = new byte[] { keyboard.cNewInput0, keyboard.cNewInput1, keyboard.cNewInput2, keyboard.cNewInput3, keyboard.cNewInput4, keyboard.cNewInput5, keyboard.cNewInput6, keyboard.cNewInput7 };
        int len = 0;
        for (; inputBytes[len] != 0 && len < 7; len++) ;
        string input = System.Text.Encoding.UTF8.GetString(inputBytes, 0, len);

            System.Text.StringBuilder textBuilder = new System.Text.StringBuilder(1024);
            uint size = SteamVR.instance.overlay.GetKeyboardText(textBuilder, 1024);
            text = textBuilder.ToString();
            print(text);
            textEntry.text = text;
    }

    private void OnKeyboardClosed(VREvent_t args)
    {
        // Might use this to unselect input field. Not sure yet
        //EventSystem.current.SetSelectedGameObject(null);
    }

    public void VolumeSliderChange()
    {
        MasterVolume = sliders[0].value;
        SFXVolume = sliders[1].value;
        AmbienceVolume = sliders[2].value;
        MusicVolume = sliders[3].value;
        
        print("Master: " + MasterVolume + "| SFX: " + SFXVolume + "| Ambience: " + AmbienceVolume + "| Music: " + MusicVolume);
    }

    private void HideStartButton()
    {
        startButtonArray[0].SetActive(false);
        startButtonArray[1].SetActive(false);
        startButtonArray[2].SetActive(false);
        startButtonArray[3].SetActive(false);
        startButtonArray[4].SetActive(false);
        startButtonArray[5].SetActive(false);
    }

    public void StartLevel()
    {
        if (levelIndex == 0)
        {
            if (difficultyIndex == 0)
            {
                print("Loading easy small day");
                _SceneController.LoadScene("Level_Easy_Small");
            }
            else if (difficultyIndex == 1)
            {
                _SceneController.LoadScene("Intermediate_Medium_Day");
            }
            else
            {
                _SceneController.LoadScene("Advanced_Medium_Day");
            }
        }
        else
        {
            if (difficultyIndex == 0)
            {
                print("loading easy medium night");
                _SceneController.LoadScene("Easy_Medium_Night");
            }
            else if (difficultyIndex == 1)
            {
                _SceneController.LoadScene("Intermediate_Medium_Night");
            }
            else
            {
                _SceneController.LoadScene("Advanced_Large_Night");
            }
        }
    }

    public void LoadCredits()
    {
        _SceneController.LoadScene("EndCreditsScene");
    }
}
