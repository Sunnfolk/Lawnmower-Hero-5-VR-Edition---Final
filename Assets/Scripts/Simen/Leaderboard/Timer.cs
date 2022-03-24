using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Valve.VR;
using UnityEngine.SocialPlatforms.Impl;

public class Timer : MonoBehaviour
{
    [Header("Submit Name Input Field")]
    public TMP_InputField textEntry;
    string text = "";
    [Space(5)]

    public float timeRemaining = 10;
    public bool timerIsRunning;
    public bool canSubmitScore;
    public TMP_Text timer;
    private playFabManagerIntermediate2 _intermediate2;
    private pauseEffect _pauseMenu;
    public GameObject pointer;

    private void Start()
    {
        timerIsRunning = true;
        _intermediate2 = GetComponent<playFabManagerIntermediate2>();
        _pauseMenu = GetComponent<pauseEffect>();
        if (pointer != null)
        {
            pointer.SetActive(false);
        }
    }

    private void Update()
    {
        DisplayTime(timeRemaining);
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                canSubmitScore = true;
                _pauseMenu.ScorePause();
                pointer.SetActive(true);
                
                timerIsRunning = false;
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (timer != null)
        {
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    private void OnEnable()
    {
        //Listens for keyboard clicks and if the keyboard closes
        SteamVR_Events.System(EVREventType.VREvent_KeyboardCharInput).Listen(OnKeyboard);
        SteamVR_Events.System(EVREventType.VREvent_KeyboardClosed).Listen(OnKeyboardClosed);
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
    
    // This code is being called from a UnityEvent (button/text field select)
    public void ShowKeyboard()
    {
        SteamVR.instance.overlay.ShowKeyboard(0, 0, 0, "Description", 256, "", 0);
        textEntry.text = "";
    }
}