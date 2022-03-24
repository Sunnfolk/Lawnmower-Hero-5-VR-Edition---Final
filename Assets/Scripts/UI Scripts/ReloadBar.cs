using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class ReloadBar : MonoBehaviour
{

    public Slider slider;
    public static float CurrentReload;
    [SerializeField] private TMP_Text reloadText;
    
    [Header("Reload % per second")]
    [SerializeField] private int reloadSpeed = 25;

    private bool CanShoot = true;

    //public GameObject LawnmowerObject;
    //public GameObject ShotgunObject;
    public ShotgunFire Shotgun;
    
    
    
    
    private void OnTrackpadButtonChanged(bool trackpadButtonState)
    {
        if (!trackpadButtonState || !Shotgun._isHeld && !CanShoot)
        {
            return;
        }


        if (CanShoot && Shotgun._isHeld)
        {
            StartCoroutine(ShootWait());
        }
    }

    private void Start()
    {
        CurrentReload = 100;
        CanShoot = true;
    }

    private void SetReload(float reload)
    {
        slider.value = reload;
    }

    public void Shoot()
    {
        CurrentReload = 0;
    }
    private void FixedUpdate()
    {
        if (CurrentReload >=100)
        {
            return;
        }
        CurrentReload += reloadSpeed * Time.deltaTime;
        SetReload(CurrentReload);
        reloadText.text = Mathf.RoundToInt(CurrentReload) + "%";
    }

    private void Update()
    {
        if (CurrentReload >=100)
        {
            CurrentReload = 100;
        }

        if (CurrentReload <=0)
        {
            CurrentReload = 0;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame && CanShoot)
        {
            StartCoroutine(ShootWait());
        }
    }

    


    public IEnumerator ShootWait()
    { 
        Shoot();
        CanShoot = false;
        /*if (ShotgunObject.transform.parent ==  LawnmowerObject.transform)
        {
            yield return new WaitForSeconds(20);
            CanShoot = true;
        }
        else
        {
            CanShoot = false;
        }*/
        yield return new WaitForSeconds(20);
        CanShoot = true;
        
    }
}