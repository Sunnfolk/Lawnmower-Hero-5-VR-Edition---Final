 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    
    public GameObject[] pauseMenuMenu;

    [Header("Drag in the pointer object from CameraRig")]
    public GameObject laserPointer;
    


    //Enables/disables laser pointer on pause/unpause
    private void OnDisable()
    {
        laserPointer.SetActive(false);
    }

    private void OnEnable()
    {
        laserPointer.SetActive(true);
    }

    public void BackToToPauseMenu()
    {
        pauseMenuMenu[0].SetActive(true);
        pauseMenuMenu[1].SetActive(false);
        pauseMenuMenu[2].SetActive(false);
    }

    public void PauseVolumeMenu()
    {
        pauseMenuMenu[0].SetActive(false);
        pauseMenuMenu[1].SetActive(true);
        pauseMenuMenu[2].SetActive(false);
    }
    public void ConfirmQuit()
    {
        Music.SetParameter("Paused", 0);
        pauseMenuMenu[0].SetActive(false);
        pauseMenuMenu[1].SetActive(false);
        pauseMenuMenu[2].SetActive(true);
    }
}
