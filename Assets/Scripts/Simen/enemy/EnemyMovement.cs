using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Movement towards player
    public float speed;
    public float rotateSpeed;
    public transformVariable target;
    public float range = 10f;

    public bool inCombat;

    private bool affectingSpeed;

    private bool enteredTargetRange;

    private Rigidbody _RB;

    private void Start()
    {
        _RB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        {
            if (Vector3.Distance(transform.position, target.playerTransform.position) <= range)
            {
                if (CompareTag("EvilGnome") || CompareTag("Wasp"))
                {
                    if (!enteredTargetRange)
                    {
                        target.enemiesInRange++;
                        enteredTargetRange = true;
                    }
                }

                // Move our position a step closer to the target
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target.playerTransform.position, step);
                print("Player spotted, chase started");
                inCombat = true;
                
                //Rotates towards target
                var targetDir = target.playerTransform.position - transform.position;
                var rotateStep = rotateSpeed * Time.deltaTime;
            
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetDir, rotateStep, 0f));
            }
            else
            {
                if (CompareTag("EvilGnome") || CompareTag("Wasp"))
                {
                    if (enteredTargetRange)
                    {
                        target.enemiesInRange--;
                        enteredTargetRange = false;
                    }
                }

                print("Where are you player?");
                inCombat = false;
            }

            //Sets velocity to prevent impacts from affecting enemies' movement
            _RB.velocity = CompareTag("Wasp") ? Vector3.zero : new Vector3(0f, _RB.velocity.y, 0f);
        }
    }

    private void OnApplicationQuit()
    {
        if (CompareTag("EvilGnome") || CompareTag("Wasp"))
        {
            target.enemiesInRange--;
        }
    }
}