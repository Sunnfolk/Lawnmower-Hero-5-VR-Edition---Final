using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private float CarSpeed = 5f;

    [SerializeField] private float TimeBeforeCarRotate = 20f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CarRotate());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 carVelocity = Vector3.right * CarSpeed;
                transform.Translate(carVelocity * Time.deltaTime);
    }

    private IEnumerator CarRotate()
    {
        yield return new WaitForSeconds(TimeBeforeCarRotate);
        transform.Rotate(180,0,180);
        StartCoroutine(CarRotate());
    }
}
