using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

[RequireComponent(typeof(PickupAble))]
[RequireComponent(typeof(InteractAble))]
public class ShotgunFire : MonoBehaviour
{
    public int pelletCount;
    public float spreadAngle;
    private float LifeTime = 3f; 
    public float pelletFireVel = 1;
    public GameObject pellet;
    public Transform BarrelExit;
    private List<Quaternion> pellets;
    private bool canFire = true;
    private ReloadBar _bar;
    public VisualEffect ShotgunExplotion;
    
    //public GameObject LawnmowerObject;
    //public GameObject ShotgunObject;
    
    public bool _isHeld;
    
    void Awake()
    {
        ShotgunExplotion.Stop();
        pellets = new List<Quaternion>(new Quaternion[pelletCount]);
    }
    
    private void OnHeldByHandChanged(InteractAble.Hand heldByHand)
    {
        _isHeld = heldByHand.GameObject != null;
    }

    //Anders note: Change back to OnTriggerButtonChanged if something breaks
    private void OnTriggerButtonChanged(bool trackpadButtonState)
    {
        if (!trackpadButtonState || !_isHeld && !canFire)
        {
            return;
        }
        
            

            if (canFire && _isHeld)
            {
                StartCoroutine(CantFireTimer());
                print("I AM THE GOD OF HELLFIRE AND I BRING YOU");
            }

            else if (!canFire && _isHeld)
            {
                Music.PlayOneShot("SFX/shotgun_empty", transform.position);
            }
        
        
    }
    
    
    // /*
    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame && canFire)
        {
            print("I AM THE GOD OF HELLFIRE AND I BRING YOU");
            StartCoroutine(CantFireTimer());
        }
        else if (Keyboard.current.wKey.wasPressedThisFrame && !canFire)
        {
            Music.PlayOneShot("SFX/shotgun_empty", transform.position);
        }
    }
    // */

    private IEnumerator CantFireTimer()
    {
        Fire();
        ShotgunExplotion.Play();
        Music.PlayOneShot("SFX/shotgun_explode", transform.position);
        /*if (ShotgunObject.transform.parent ==  LawnmowerObject.transform)
        {
            yield return new WaitForSeconds(20);
            CanFire = true;
        }
        else
        {
            CanFire = false;
        }*/
        canFire = false;
        yield return new WaitForSeconds(20);
        canFire = true;
    }
        
    void Fire()
    {
       
        //GameObject p = new GameObject();
        for (int i = 0; i  <pelletCount ; i ++)
        {
            pellets[i] = Random.rotation;
            var p = (GameObject)Instantiate(pellet, BarrelExit.position, BarrelExit.rotation) as GameObject;
            Destroy(p, LifeTime);
            p.transform.rotation = Quaternion.RotateTowards(p.transform.rotation, pellets[i], spreadAngle);
            p.GetComponent<Rigidbody>().AddForce(p.transform.forward * pelletFireVel);
            i++;  
        }
    }


}
