using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BatBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    public float batDamage =  2f;

    [Tooltip("The tip of the bat")]
    public Transform batTip;

    public List<Vector3> positionTracker = new List<Vector3>();
    private float difference;
    private float speed;
    [Tooltip("Values used to define the ranges of the damages")]
    //public int minSpeed1 =10, minSpeed2 = 15, medSpeed1 = 15, medSpeed2 =20, maxSpeed = 20;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        positionTracker.Add(batTip.position);
    }

    [ExecuteInEditMode]
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), "Velocity: "+ speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            rb.useGravity = true;
        }
        else if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            rb.useGravity = false;
        }
    }

    private void FixedUpdate()
    {
        // todo: don't track unless in hands.
        
        positionTracker.Add(batTip.position);
        difference = Vector3.Distance(positionTracker[0], positionTracker[1]);
        positionTracker.Remove(positionTracker[0]);

        speed = difference / Time.deltaTime;
        
        /*if (speed > 21f)
        {
           Debug.Log("speed = "+ speed);
        }*/
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("EvilGnome") || 
            (other.collider.CompareTag("Gnome Lair")) ||
            (other.collider.CompareTag("Wasp")))
        {
            if (speed >= 21f)
            {
                GetEnemyDoDamage(other, 50f);
                print(speed + ": Fast af Boyyyy");
            }

            else if (speed >= 16f)
            {
                GetEnemyDoDamage(other, 40f);
                print("Do you have anny idea how fast im going");
            }

            else if (speed >= 6f)
            {
                GetEnemyDoDamage(other, 5f);
                print("I'm Fast");
            }
        }
        
        if (speed >= 6f)
        {
            Music.PlayOneShot("SFX/equipment_collide", transform.position);
        }
    }

    private void GetEnemyDoDamage(Collision wasp, float damage)
    {
        if (wasp.collider.GetComponent<EnemyHealth>() != null)
        {
            wasp.collider.GetComponent<EnemyHealth>().health -= batDamage * damage;
        }
    }
}